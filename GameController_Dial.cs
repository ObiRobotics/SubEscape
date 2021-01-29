using Boo.Lang;
using TMPro;
using UnityEngine;
using System.Linq;
using System;

public class GameController_Dial : MonoBehaviour
{
    public bool patient;
    public ExperimentState expState;
    public float closeSpeedFactor = 1f;
    public float openSpeedFactor = 1f;
    public Transform[] virtualDigits;
    public Transform[] realDigits;
    public TMP_Text minMaxVals;

    public static bool calibrated = false;

    private List<float> bendState = new List<float>();
    //private float currentAngle;
    private float normalizedAngle;
    private float[] bendArr = new float[7500];
    private int arrIdx;
    //private float mappedVal = 0f;

    public float handMin = 0.06f; // 0.053f <- real hand
    public float handMax = 0.1f; // 0.104f <- real hand
    float rHandMin = 0.075f;
    float rHandMax = 0.1f;
    float vHandMin = 0.075f;
    float vHandMax = 0.1f;

    //float[] realHandMinMax = new float[1200];
    //float[] virtualHandMinMax = new float[1200];
    List<float> realHandMinMax = new List<float>();
    List<float> virtualHandMinMax = new List<float>();
    int rdx = 0;
    int vdx = 0; 

    private void Start()
    {
        minMaxVals.text = "Welcome to Sub Escape \n" +
                          "Familiarise yourself with the space \n" +
                          "Open and close your hand to see how \n " +
                          "it affects the dial infront of you!!!";
        Invoke("ChangeState", expState.initDelay);
    }

    private void ChangeState()
    {
        calibrated = true;
        minMaxVals.text = "";
    }

    private void Update()
    {
        if (patient) // if not a patient then the bendState[0] param should be 11.05
        {
            expState.dialGaining = 20f;
            expState.exPonent = 1.0185f;

            if (arrIdx < bendArr.Length)
            {
                bendArr[arrIdx] = AutoCalibration();
                //minMaxVals.text = "Avr. angle: " + bendArr[arrIdx].ToString("F4");

                expState.bendState[0] = Mathf.Min(bendArr);
                //expState.bendState[1] = bendArr.Max();
                arrIdx++;
            }
            else
            {
                //Debug.Log("Arr reset!!!");
                arrIdx = 0;
            }
        }
        else
        {
            expState.bendState[0] = -0.1121f; // <- Why this value? How can this be improved?
        }

        HandConfiguration();
        StimHandConfiguration();
    }


    private void HandConfiguration()
    {
        Vector3 currentHandData = new Vector3();

        for (int i = 0; i < realDigits.Length - 1; i++)
        {
            currentHandData += realDigits[16].InverseTransformPoint(realDigits[i].position);
        }
        float currentAngle = (currentHandData.x * 0.067f) +
                       (currentHandData.y * 0.067f) +
                       (currentHandData.z * 0.067f);
        currentAngle = Mathf.Abs(currentAngle);

        RealHandAutoCalib(currentAngle);


        //float mappedVal = currentAngle.Map(handMin, handMax, 0.1f, 180f);
        float mappedVal = currentAngle.Map(rHandMin, rHandMax,0.1f, 180f);

        expState.virtDialAngle = mappedVal;
    }

    void RealHandAutoCalib(float inputVal)
    {
        realHandMinMax.Add(inputVal);
        if (realHandMinMax.Min() < rHandMin)
            rHandMin = realHandMinMax.Min();
        if (realHandMinMax.Max() > rHandMax)
            rHandMax = realHandMinMax.Max();

        if (rdx >= 1100)
        {
            realHandMinMax.Clear();
            rdx = 0;
        }
        rdx++;
    }
    void VirtualHandAutoCalib(float inputVal)
    {
        virtualHandMinMax.Add(inputVal);
        if (virtualHandMinMax.Min() < vHandMin)
            vHandMin = virtualHandMinMax.Min();
        if (virtualHandMinMax.Max() > vHandMax)
            vHandMax = virtualHandMinMax.Max();

        if (vdx >= 1100)
        {
            virtualHandMinMax.Clear();
            vdx = 0;
        }
        vdx++;
    }

    private void StimHandConfiguration()
    {
        Vector3 currentHandData = new Vector3();
        for (int i = 0; i < virtualDigits.Length - 1; i++)
        {
            currentHandData += virtualDigits[16].InverseTransformPoint(virtualDigits[i].position);
        }
        float currentAngle = (currentHandData.x * 0.067f) +
                       (currentHandData.y * 0.067f) +
                       (currentHandData.z * 0.067f);
        currentAngle = Mathf.Abs(currentAngle);
        
        VirtualHandAutoCalib(currentAngle);

        float mappedVal = currentAngle.Map(vHandMin, vHandMax, 0.1f, 179.9f); // Expanded this 180f to 190f to make the dial move the full 180 degree from the virtual hand movements 

        expState.dialAngle = mappedVal;
    }

    private float AutoCalibration()
    {
        Vector3 openHandData = new Vector3();
        for (int i = 0; i < realDigits.Length - 1; i++)
        {
            openHandData += realDigits[16].InverseTransformPoint(realDigits[i].position);
        }
        return (openHandData.x * 0.067f) +
               (openHandData.y * 0.067f) +
               (openHandData.z * 0.067f);
    }

    private void HandBaselineCalib()
    {
        if (OnButton.calibrate & OnButton.calibrationStep == 0) // Input.GetKeyDown(KeyCode.I) |
        {
            Vector3 openHandData = new Vector3();
            for (int i = 0; i < realDigits.Length - 1; i++)
            {
                openHandData += realDigits[16].InverseTransformPoint(realDigits[i].position);
            }
            expState.bendState[0] = (openHandData.x * 0.067f) +
                                    (openHandData.y * 0.067f) +
                                    (openHandData.z * 0.067f);
            OnButton.calibrate = false;
        }
        if (OnButton.calibrate & OnButton.calibrationStep == 1) // Input.GetKeyDown(KeyCode.O) |
        {
            Vector3 closeHandData = new Vector3();
            for (int i = 0; i < realDigits.Length - 1; i++)
            {
                closeHandData += realDigits[15].InverseTransformPoint(realDigits[i].position);
            }

            expState.bendState[1] = (closeHandData.x * 0.067f) +
                                    (closeHandData.y * 0.067f) +
                                    (closeHandData.z * 0.067f);
            calibrated = true;
            OnButton.calibrate = false;
        }
    }
}

// Update (part of)
// 1st. Get hand baseline open and close states
//if (!calibrated)
//{
//    HandBaselineCalib();
//}
//else
//{
//bendState.Add(AutoCalibration());
//expState.bendState[0] = Mathf.Abs(bendState.Min() * 100f);
//expState.bendState[1] = Mathf.Abs(bendState.Max() * 100f);
//if (calibrated)
//{
//    //minMaxVals.text = "Min: " + (Mathf.Abs(expState.bendState[1] * 100f)).ToString("F2") +
//    //                  " Max: " + (Mathf.Abs(expState.bendState[0] * 100f)).ToString("F2");
//}

// HandConfiguration (part of)
//minMaxVals.text = currentAngle.ToString("F3") +" \n"+
//                  mappedVal.ToString("F3");

//normalizedAngle = Mathf.Abs((currentAngle - (Mathf.Abs(expState.bendState[1]) * expState.bendPerc[1])) /
//                 ((Mathf.Abs(expState.bendState[0]) * expState.bendPerc[0]) - (Mathf.Abs(expState.bendState[1]) * expState.bendPerc[1])));

//Debug.Log("Nang: " + normalizedAngle.ToString("F2")); // normalized range is from 0.0-(close) to 1.35-(open)

//# Think about improving this part as it doesn't work so well at the moment. Perhaps use exponential to improve grading of motion and
//# Solve issue with the hand closing state resulting in jumps in the dial etc.
//# Perhaps also limit the dial motion to 180 degrees instead of 360 degrees as it is currently.
//expState.dialAngle = (normalizedAngle * openSpeedFactor) + closeSpeedFactor; // Mathf.Pow(normalizedAngle, 2) * closeSpeedFactor; //