using UnityEngine;

public class StimulusHand : MonoBehaviour
{
    public ExperimentState expState;
    public Transform[] digits;
    public Transform[] realDigits;
    public TMPro.TMP_Text displayText;

    [Range(-180f, 180f)]
    public float angularOffset = 5f;

    [Range(-4f, 4f)]
    public float angScale = 1f;

    //[Range(0f,1.5f)]
    //public float gain;
    private float mcpAngleExp;

    public float[] initMinJointAngles;
    public float[] maxJointAngles;

    [Range(-2f, 2f)]
    public float[] proxScaleDown = new float[3] { 1f, 1f, 0.75f };

    //public Animation handOpenClose;
    //AnimationState handState;

    public bool newAnimApproach = false;

    private void Start()
    {
        //handState = handOpenClose.GetComponent<AnimationState>();
    }

    private void Update()
    {
        if (newAnimApproach)
        {
            for (int i = 0; i < digits.Length - 1; i++)
            {
                mcpAngleExp = realDigits[i].localEulerAngles.z;

                digits[i].localEulerAngles = new Vector3(realDigits[i].localEulerAngles.x,
                                                realDigits[i].localEulerAngles.y,
                                                mcpAngleExp);
            }
        }
        else
        {
            for (int i = 0; i < digits.Length - 1; i++) // After the third digit joint (i+=3) means except the thumb
            {
                if (i < 12 & expState.block == 1) // Do distorted mapping for adaptation blocks
                {
                    // Create exponential function for z-axis joint angle using this formula:
                    // f(x) = (x + (x^n + offset)) * scale
                    mcpAngleExp = ((realDigits[i].localEulerAngles.z +
                                  (Mathf.Pow(realDigits[i].localEulerAngles.z, angScale))) +
                                  angularOffset) * proxScaleDown[i % 3]; // i%3 makes sure that i doesn't exceed the index of proxScaleDown variable

                    // Clamp value between min and max angles of the z-axis joint to avoid weird finger bending backwards
                    //if(mcpAngleExp > ((maxJointAngles[i] + angularOffset) * proxScaleDown[i % 3]))
                    //{
                    //    mcpAngleExp = maxJointAngles[i];
                    //}
                }
                else // do a 1-to-1 mapping between the real and virtual finger for baseline and washout blocks
                {
                    mcpAngleExp = realDigits[i].localEulerAngles.z;
                }

                digits[i].localEulerAngles = new Vector3(realDigits[i].localEulerAngles.x,
                                                         realDigits[i].localEulerAngles.y,
                                                         mcpAngleExp);
            }
        }
    }
}

//if (i <= 1) // Don't apply any gain to the thumb
//{
//    digits[i].localEulerAngles = new Vector3(realDigits[i].localEulerAngles.x,
//                                             realDigits[i].localEulerAngles.y,
//                                             realDigits[i].localEulerAngles.z);
//}
//else
//{
//float mcpAngleExp = realDigits[i].localEulerAngles.z * expState.handGain; // Original way
//float mcpAngleExp = Mathf.Pow(realDigits[i].localEulerAngles.z, expState.exPonent) + angularOffset; // Previous exponential way

//mcpAngleExp = realDigits[i].localEulerAngles.z.Map(initMinJointAngles[i] + minOffset, maxJointAngles[i],
//                                                   initMinJointAngles[i], maxJointAngles[i] * angScale) + angularOffset; // New mapping way

//mcpAngleExp = Mathf.Pow(realDigits[i].localEulerAngles.z, minOffset); // New scale factor way (simplest one so far) <- Doesn't work
//mcpAngleExp = Mathf.Exp(realDigits[i].localEulerAngles.z) + angularOffset; // <- Doesn't work

//float zero2oneClamp = realDigits[i].localEulerAngles.z.Map(initMinJointAngles[i], -85f, 0f, 1f); // <- Doesn't seem to work either 1/2
//mcpAngleExp = realDigits[i].localEulerAngles.z * angleFunction.Evaluate(zero2oneClamp * minOffset); // <- Doesn't seem to work either 2/2

////float normedAngle = (realDigits[i].localEulerAngles.z - initMinJointAngles[i]) / (maxJointAngles[i] - initMinJointAngles[i]) / realDigits[i].localEulerAngles.z;
//float normedAngle = ((Mathf.Abs(realDigits[i].localEulerAngles.z) - Mathf.Abs(initMinJointAngles[i])) /
//                     (Mathf.Abs(maxJointAngles[i]) - Mathf.Abs(initMinJointAngles[i])));
//Debug.Log("Norm: " + normedAngle.ToString("F3"));
//mcpAngleExp = (realDigits[i].localEulerAngles.z * angleFunction.Evaluate(normedAngle * minOffset)) + angularOffset; //

//mcpAngleExp = (realDigits[i].localEulerAngles.z * minOffset) + angularOffset; // <- Doesn't work properly becaue the value gets stepped linearly

// mcpAngleExp = Mathf.Pow(realDigits[i].localEulerAngles.z, minOffset) + angularOffset; // <- Too difficult to adjust and results in weird angle sof the fingers

//mcpAngleExp = (maxJointAngles[i] / 1f + (minOffset * Mathf.Exp(realDigits[i].localEulerAngles.z - (maxJointAngles[i]/2)))) + angularOffset; // Logistic function

//float normedAngle = Mathf.Abs(realDigits[i].localEulerAngles.z) / maxJointAngles[i];
//double unboundNorm = (realDigits[i].localEulerAngles.z);
//displayText.text = "Angle: " + realDigits[i].localEulerAngles.z.ToString("F3") + "\n" +
//                   "Norm: " + unboundNorm.ToString("F3");
//float normedAngle = realDigits[i].localEulerAngles.z.Map(Mathf.Abs(initMinJointAngles[i]), Mathf.Abs(maxJointAngles[i]), 0f, 1f);
////normedAngle = Mathf.Abs(4f - normedAngle);
//displayText.text = "N.Angle: " + normedAngle.ToString("F3");