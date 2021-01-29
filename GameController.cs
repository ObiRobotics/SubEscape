//using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class GameController : MonoBehaviour
{
    // All variables 
    #region
    //[Range(0.95f, 1.05f)]
    //public float gainValue = 1f;
    //public static float maxGain = 1.05f;
    //public static float minGain = 0.95f;
    bool auto_starter = true;
    bool handClosing = false;
    bool handRangeDone;
    float avProxJoints = 0f;
    float leverAngle;
    float pumpScale;
    float[] angularJointSpeed = new float[15];
    float[] closeMargin = new float[15];
    float[] closePose = new float[15];
    float[] digitFloats = new float[15];
    float[] openMargin = new float[15];
    float[] openPose = new float[15];
    float[] prevDigitFloats = new float[15];
    int blockNum = 0;
    int leverSide = 0;
    List<float> digitAngles = new List<float>();
    Material panelMat;
    Material[] AalarmMat = new Material[2];
    private float[] bendingState = new float[2];
    public AudioSource[] AlarmWaterAud;
    public AudioSource[] InstructAud;
    public AudioSource[] NegativeFeedbackAud;
    public AudioSource[] PositiveFeedbackAud;
    public AudioSource[] PumpRelaxes;
    public AudioSource[] PumpSqueezes;
    public AudioSource[] WaterAud;
    public AudioSource[] WelcomeAud;
    public GameObject AlarmGlassLeft;
    public GameObject AlarmGlassRight;
    public GameObject ControlPanel;
    public GameObject LeverHandle;
    public GameObject LeverPump;
    public GameObject SpherePump;
    public Light[] AlarmLights;
    public ParticleSystem WaterBubbles;
    public ParticleSystem[] Particles_WaterLeak;
    public static bool startTrial, endTrial;
    public static int leverCycles;
    public static string gestureName = "";
    public TMP_Text blockLabels;
    public TMP_Text trialLabels;
    public TMPro.TMP_Text debugDisplay;
    public TMPro.TMP_Text txtCounter;
    public TMPro.TMP_Text txtInstruction;
    public Transform[] digits; // 3rd entry = index finger proximal joint 
    string handLR = "";
    string handStatings = "";

    #endregion

    // Standard Unity functions (Awake, Start and FixedUpate)
    #region
    void Awake()
    {
        foreach (ParticleSystem par in Particles_WaterLeak)
        {
            par.Stop();
        }
    }
    void Start()
    {
        trialLabels.text = "";
        blockLabels.text = "";

        LeverPump.SetActive(false);

        handLR = PlayerPrefs.GetString("hand");
        panelMat = ControlPanel.GetComponent<MeshRenderer>().material;
        AalarmMat[0] = AlarmGlassLeft.GetComponent<MeshRenderer>().material;
        AalarmMat[1] = AlarmGlassRight.GetComponent<MeshRenderer>().material;

        StartCoroutine(WelcomeSequence());
    }
    void FixedUpdate()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            StartCoroutine(IntroSequencer());
            auto_starter = false;
        }

        if (handRangeDone)
        {
            HandOpenCloseCycler2();
        }
        //HandOpenCloseSpeed();
    }

    int HandOpenCloseCycler2()
    {
        // Debug code 
        #region
        if (handLR.Contains("R"))
        {
            avProxJoints = (digits[15].localEulerAngles.z +
                               digits[18].localEulerAngles.z +
                               digits[21].localEulerAngles.z +
                               digits[24].localEulerAngles.z +
                               digits[27].localEulerAngles.z) * 0.2f; // i.e. divided by 5 fingers 1/5=0.2f
        }
        else
        {
            avProxJoints = (digits[0].localEulerAngles.z +
                                 digits[3].localEulerAngles.z +
                                 digits[6].localEulerAngles.z +
                                 digits[9].localEulerAngles.z +
                                 digits[12].localEulerAngles.z) * 0.2f;
        }

        float dist1 = Mathf.Abs(bendingState[0] - (avProxJoints * SliderScript.gainSliderValue));
        float dist2 = Mathf.Abs(bendingState[1] - (avProxJoints * SliderScript.gainSliderValue));

        if (dist1 >= 0f && dist1 <= 5f)
        {
            //Debug.Log("Hand is Open");
            handStatings = "Open";
        }

        if (dist2 >= 0f && dist2 <= 5f)
        {
            //Debug.Log("Hand is Closed");
            handStatings = "Closed";
        }

        debugDisplay.text = dist1.ToString("F3") + "\t" + dist2.ToString("F3") + "\n" + handStatings;

        #endregion

        if (handStatings.Contains("Closed") & leverSide <= 0) //(GestureDetector.gestureState.Contains("Close"))
        {
            int randPump = Random.Range(0, 2);
            PumpSqueezes[randPump].Play();

            leverSide = 1;
        }
        if (handStatings.Contains("Open") & leverSide > 0) //(GestureDetector.gestureState.Contains("Open"))
        {
            //if (leverSide > 0)
            //{
            leverCycles++;
            txtCounter.text = "[ " + leverCycles.ToString() + " ]";

            int randPump = Random.Range(0, 2);
            PumpRelaxes[randPump].Play();

            leverSide = -1;
            //}
        }

        return leverCycles;

    }
    #endregion

    // Main experiment sequences 
    #region
    IEnumerator WelcomeSequence()
    {
        txtInstruction.text = "Welcome!";
        WelcomeAud[0].Play();
        while (WelcomeAud[0].isPlaying)
            yield return null;
        yield return new WaitForSecondsRealtime(2.5f);

        txtInstruction.text = "Look around!";
        WelcomeAud[1].Play();
        while (WelcomeAud[1].isPlaying)
            yield return null;
        yield return new WaitForSecondsRealtime(2.5f);

        txtInstruction.text = "Control Panel!";
        StartCoroutine(HighlightObject(panelMat, 1f)); // Blink control panel table 
        WelcomeAud[2].Play();
        while (WelcomeAud[2].isPlaying)
            yield return null;
        yield return new WaitForSecondsRealtime(2.5f);

        txtInstruction.text = "Next we measure the hand baseline!!!";
        yield return new WaitForSecondsRealtime(5f);
        StartCoroutine(BaselineSequencer()); // Start baseline >>> intro >>> experiment sequence 
    }
    IEnumerator BaselineSequencer()
    {
        // *************************************************************************************
        // *************************************************************************************
        // *************************************************************************************

        // Implement min-max hand openning values 
        // This could be the GestureDetector.Save() for close (min) and open (max) hand states 
        // ?Use the start button to confirm hand states?
        //?Gain = threshold?

        // Instruct to open and close hand for a few times to get a baseline min and max hand openning value
        string whichHand;
        if (handLR.Contains("R"))
        {
            whichHand = "Left";
        }
        else
        {
            whichHand = "Right";
        }

            txtInstruction.text = "Open your " + whichHand + " hand and with your OTHER hand press the start button to confirm";
        while (!ButtonScript.pressing)
            yield return null;
        gestureName = "OpenGesture";
        // Save average proximal finger joint angles when the hand is open 
        if (handLR.Contains("R"))
        {
        bendingState[0] = (digits[15].localEulerAngles.z +
                          digits[18].localEulerAngles.z +
                          digits[21].localEulerAngles.z +
                          digits[24].localEulerAngles.z +
                          digits[27].localEulerAngles.z) * 0.2f;
        }
        else
        {
        bendingState[0] = (digits[0].localEulerAngles.z +
                          digits[3].localEulerAngles.z +
                          digits[6].localEulerAngles.z +
                          digits[9].localEulerAngles.z +
                          digits[12].localEulerAngles.z) * 0.2f;          
        }
        ButtonScript.pressing = false; // Safeguard to avoid errornous calibration of open and close hand states with one button press. 
        GestureDetector.saveGesture = true;

        yield return new WaitForSecondsRealtime(2f);

        txtInstruction.text = "Close your " + whichHand + " hand and with your OTHER hand press the start button to confirm";
        while (!ButtonScript.pressing)
            yield return null;
        gestureName = "CloseGesture";
        // Save average proximal finger joint angles when the hand is closed 
        if (handLR.Contains("R"))
        {
        bendingState[1] = (digits[15].localEulerAngles.z +
                          digits[18].localEulerAngles.z +
                          digits[21].localEulerAngles.z +
                          digits[24].localEulerAngles.z +
                          digits[27].localEulerAngles.z) * 0.2f;
        }
        else
        {
        bendingState[1] = (digits[0].localEulerAngles.z +
                          digits[3].localEulerAngles.z +
                          digits[6].localEulerAngles.z +
                          digits[9].localEulerAngles.z +
                          digits[12].localEulerAngles.z) * 0.2f;
        }

        ButtonScript.pressing = false; // Safeguard to avoid errornous calibration of open and close hand states with one button press. 
        GestureDetector.saveGesture = true;

        // *************************************************************************************
        // *************************************************************************************
        // *************************************************************************************

        PositiveFeedbackAud[1].Play();
        txtInstruction.text = "Well done";
        handRangeDone = true;

        yield return new WaitForSecondsRealtime(4f);

        StartCoroutine(IntroSequencer());
    }
    IEnumerator IntroSequencer() // Formerly "GameSequencer()"
    {
        // Here is the complete game mechanics timeline 

        // Initial intro -> Look around to find and press the red start button on your control panel
        InstructAud[0].Play();
        txtInstruction.text = "Press Start!";
        //bool task_complete_01 = LookAroundCheker();
        while (!ButtonScript.pressing)
            yield return null;

        PositiveFeedbackAud[0].Play();
        txtInstruction.text = "";
        yield return new WaitForSecondsRealtime(5f);

        // Mission statement -> Now, find the hand powered lever on your left. This should only be used in the unlikely event of an emergency. 
        InstructAud[1].Play();
        txtInstruction.text = "Find pump!";
        LeverPump.SetActive(true);
        while (InstructAud[1].isPlaying)
            yield return null;
        yield return new WaitForSecondsRealtime(3f);

        InstructAud[2].Play(); // -> Try to operate the lever by closing and opening your hand
        txtInstruction.text = "Operate pump!";

        // Reset the hand pump level cycle counter
        leverCycles = 0;

        while (InstructAud[2].isPlaying)
            yield return null;
        yield return new WaitForSecondsRealtime(2f);

        while (leverCycles < 7)
            yield return null;
        PositiveFeedbackAud[1].Play();
        txtInstruction.text = ":)";
        yield return new WaitForSecondsRealtime(2f);

        // Start game drama (Submarine leaking animation) 
        while (leverCycles < 12)
            yield return null;
        GameDrama();
        PressureGaugeScript_02.pressureGauge = -20f; // Change the dial on the pressure gauge >>> Make this smooth, with a sssh sound 

        while (AlarmWaterAud[0].isPlaying)
            yield return null;
        yield return new WaitForSecondsRealtime(1f);

        //StartCoroutine(PlayAud(AlarmWaterAud[1], 0f));
        AlarmWaterAud[1].Play(); // Emergency, leak damage detected 
        txtInstruction.text = ":/";
        LightChanger();

        while (AlarmWaterAud[1].isPlaying)
            yield return null;
        txtInstruction.text = "Auto-pump activated!";
        AlarmWaterAud[2].Play(); // Motor pumpt activated!
        AlarmWaterAud[6].Play(); // Add motor and failing sound

        while (AlarmWaterAud[6].isPlaying)
            yield return null;
        yield return new WaitForSecondsRealtime(2f);

        leverCycles = 12;
        txtInstruction.text = "Auto-pump failed!"; // Motor failed!
        AlarmWaterAud[3].Play();

        while (AlarmWaterAud[3].isPlaying)
            yield return null;
        yield return new WaitForSecondsRealtime(2f);

        leverCycles = 12;
        txtInstruction.text = "Manual-pump required!"; // Manual pump operation advised
        AlarmWaterAud[4].Play();
        yield return new WaitForSecondsRealtime(2f);

        while (AlarmWaterAud[4].isPlaying)
            yield return null;
        yield return new WaitForSecondsRealtime(2f);

        leverCycles = 12;
        txtInstruction.text = "Open and close hand to pump!"; // Instructions to open pump -> open,close hand
        AlarmWaterAud[5].Play();
        //StartCoroutine(PlayAud(AlarmWaterAud[7], 13f)); // Repeat instruction to open and close hand

        //while (leverAngle > 25f)
        //    yield return null;
        //PositiveFeedbackAud[0].Play();

        while (leverCycles < 20)
            yield return null;
        PositiveFeedbackAud[1].Play();
        txtInstruction.text = ":)";

        // Orange col
        Color col_yellow = new Color(1f, 1f, 0f, 1f);
        LightChanger(col_yellow);

        while (leverCycles < 30)
            yield return null;

        Color col_green = new Color(0.05f, 0.9f, 0.05f, 1f);
        LightChanger(col_green);

        GameDramaOff();

        PositiveFeedbackAud[2].Play(); // Well done, disaster averted. 

        //Now the experiment begins 
        txtInstruction.text = "Now the experiment begins!!!";
        yield return new WaitForSecondsRealtime(4f);

        if (blockNum == 0)
            blockLabels.text = blockNum.ToString();
        else
            blockLabels.text = blockLabels.text + "\n" + blockNum.ToString();
        StartCoroutine(ExperimentSequencer());
    }
    IEnumerator ExperimentSequencer()
    {

        Color col_red = new Color(1f, 0f, 0f, 1f);
        Color col_green = new Color(0.05f, 0.9f, 0.05f, 1f);
        Color col_yellow = new Color(1f, 1f, 0f, 1f);

        txtInstruction.text = "Press the start button to begin the experiment!";
        while (!ButtonScript.pressing)
            yield return null;

        trialLabels.text = "";

        int numTrials = 6; // Normally 6
                           // Include block loop nesting the trial loop below 
        for (int i = 0; i < numTrials; i++)
        {
            // # Currently the gain value is assigned randomly per block. This should be improved in the future to follow a proper experimental design
            SliderScript.gainSliderValue = Random.Range(0.9f, 1.1f);

            if (i == 0)
                trialLabels.text = i.ToString();
            else
                trialLabels.text = trialLabels.text + "\n" + i.ToString();

            startTrial = true;
            txtInstruction.text = "Open and close your hand to fix the leak!";

            // Reset trial variables 
            leverCycles = 0;

            // Game drama (Leak animation)
            GameDrama();

            // Warning color 
            LightChanger(col_red);

            while (leverCycles < 20)
                yield return null;

            txtInstruction.text = "Good, continue!";
            LightChanger(col_yellow);

            while (leverCycles < 30)
                yield return null;

            LightChanger(col_green);

            GameDramaOff();

            PositiveFeedbackAud[2].Play(); // Well done, disaster averted. 
            txtInstruction.text = "Leak fixed :D";
            endTrial = true;
            yield return new WaitForSecondsRealtime(5f); // short between-trial-break 
        }

        txtInstruction.text = "Block complete! \n " +
                              "To continue, press the start button!";

        while (!ButtonScript.pressing)
            yield return null;

        blockNum++;
        if (blockNum == 0)
            blockLabels.text = blockNum.ToString();
        else
            blockLabels.text = blockLabels.text + "\n" + blockNum.ToString();

        StartCoroutine(ExperimentSequencer());

        /* 
            > Need to create a new sequence for the experiment (ExperimentSequencer)
            > This should following this sequence:
                //- Present stimulus leak (i.e. task goal)
                //- Sound alarm and instruct participants to take action 
                - Include block loop // # Outstanding 
                - Count grasp cycles w/o experimental condition (gain) // # Outstanding 
                //- Provide audio-visual feedback when task is complete
                //- repeat cycle ... // # Outstanding 
        */

    }
    #endregion

    // Functional functions
    #region 
    void SliderMove()
    {
        //gainSlider.value = 
    }

    void GameDramaOff()
    {
        // Stop particle effects and their sounds
        foreach (ParticleSystem par in Particles_WaterLeak)
        {
            par.Stop();
        }
        foreach (AudioSource audioS in WaterAud)
        {
            audioS.Stop();
        }
        WaterBubbles.Play();
        //WaterBubbles.startLifetime = 50f;
        //WaterBubbles.startSpeed = 0.2f;
    }
    void GameDrama()
    {
        for(int i = 0; i<Particles_WaterLeak.Length; i++)
        {
            StartCoroutine(ParticleStarter(Particles_WaterLeak[i], 2f));
        }
        WaterBubbles.Stop();
        //WaterBubbles.startLifetime = 10f; 
        //WaterBubbles.startSpeed = 1f; 
        StartCoroutine(PlayAud(WaterAud[0], 1f));
        StartCoroutine(PlayAud(WaterAud[1], 2f));
        StartCoroutine(PlayAud(AlarmWaterAud[0], 2f)); // Alarm sound twice
    }

    float[] HandOpenCloseSpeed()
    {
        foreach (Transform d in digits)
        {
            digitAngles.Add(Mathf.Abs(d.localEulerAngles.z));
        }
        digitFloats = digitAngles.ToArray();

        int s = 0;
        foreach (float diff in digitFloats)
        {
            angularJointSpeed[s] = (diff - prevDigitFloats[s]) / Time.deltaTime;
            s++;
        }

        //Debug.Log("Index MiP Speed: " + angularJointSpeed[4].ToString());

        prevDigitFloats = digitAngles.ToArray();
        digitAngles.Clear();
        return angularJointSpeed;
    }
    int HandOpenCloseCycler()
    {
        if (GestureDetector.gestureState.Contains("Close"))
        {
            leverSide = 1;
        }
        if (GestureDetector.gestureState.Contains("Open"))
        {
            if (leverSide > 0)
            {
                leverCycles++;
                txtCounter.text = "[ " + leverCycles.ToString() + " ]";
                leverSide = -1;
            }
        }

        #region
        //int c = 0;

        //foreach (Transform d in digits)
        //{
        //    float digitVal = Mathf.Abs(d.localEulerAngles.z); 

        //    //if (digitVal > closePose[c] & digitVal < openPose[c])
        //    //{
        //    //    // Hand in between open and close phase
        //    //}
        //    if (digitVal <= closePose[c]) // | digitVal < closePose[c] + closeMargin[c])
        //    {
        //        // Hand is in closed phase
        //        leverSide = 1; 
        //    }
        //    if (digitVal >= openPose[c]) // + openMargin[c] | digitVal > openPose[c] - openMargin[c])
        //    {
        //        //// Hand is in open phase
        //        //if (leverSide > 0)
        //        //{
        //        //    leverCycles++;
        //        //    txtCounter.text = "[ " + leverCycles.ToString() + " ]";
        //        //    leverSide = -1; 
        //        //}
        //    }
        //    c++;
        //}
        #endregion

        return leverCycles;
    }
    void LightChanger(Color col)
    {
        foreach (Material aMat in AalarmMat)
        {
            aMat.color = col;
            aMat.SetColor("_EmissionColor", col * 3f);
        }
        foreach (Light lgt in AlarmLights)
        {
            lgt.color = col;
        }
    }
    void LightChanger()
    {
        foreach (Material aMat in AalarmMat)
        {
            aMat.color = Color.red;
            aMat.SetColor("_EmissionColor", Color.red * 3f);
        }
        foreach (Light lgt in AlarmLights)
        {
            lgt.color = new Color(1f, 0f, 0f, 1f * 10f);
        }
    }
    IEnumerator ParticleStarter(ParticleSystem part, float delay)
    {
        yield return new WaitForSecondsRealtime(delay);
        part.Play();
    }
    IEnumerator PlayAud(AudioSource aud, float delay)
    {
        yield return new WaitForSecondsRealtime(delay);
        aud.Play();
    }
    IEnumerator PlayAud(AudioSource aud, TMPro.TMP_Text txt, string strng, float delay)
    {
        yield return new WaitForSecondsRealtime(delay);
        aud.Play();
        txt.text = strng;
    }
    IEnumerator PlayAud(TMPro.TMP_Text txt, string strng, float delay)
    {
        yield return new WaitForSecondsRealtime(delay);
        txt.text = strng;
    }
    IEnumerator HighlightObject(Material _mat, float delay)
    {
        _mat.EnableKeyword("_EMISSION");

        _mat.SetColor("_EmissionColor", Color.red * 1f);
        yield return new WaitForSecondsRealtime(delay);

        for (int i = 0; i < 10; i++)
        {
            _mat.SetColor("_EmissionColor", Color.red * 0f);
            yield return new WaitForSecondsRealtime(delay / delay);

            _mat.SetColor("_EmissionColor", Color.red * 1f);
            yield return new WaitForSecondsRealtime(delay / delay);
        }
        _mat.DisableKeyword("_EMISSION");
    }
    void PumpCycler()
    {
        pumpScale = Mathf.Sqrt(SpherePump.transform.localScale.x * SpherePump.transform.localScale.x);
        //Debug.Log("Scale: " + pumpScale.ToString()); 

        if (pumpScale > 0.085f)
        {
            leverSide = 1;
        }
        if (leverSide > 0 & pumpScale < 0.045f)
        {
            leverCycles++;
            txtCounter.text = "[ " + leverCycles.ToString() + " ]";

            leverSide = -1;
        }
    }
    void LeverCycler()
    {
        leverAngle = LeverHandle.transform.localRotation.eulerAngles.x;
        if (leverAngle > 180f)
            leverAngle = leverAngle - 360f;

        if (leverAngle > 30f)
        {
            leverSide = 1;
        }
        if (leverSide > 0 & leverAngle < -30f)
        {
            leverCycles++;
            txtCounter.text = "[ " + leverCycles.ToString() + " ]";

            leverSide = -1;
        }
    }
    IEnumerator HandOpenCloseRanger()
    {
        // Instruct participant to close the target hand and press a button with the other hand 
        txtInstruction.text = "Close your target hand and press the start button with your other hand to confirm!";
        while (!ButtonScript.pressing)
            yield return null;
        ButtonScript.pressing = false;

        int k = 0;
        foreach (Transform d in digits)
        {
            closePose[k] = Mathf.Abs(d.localEulerAngles.z);
            k++;
        }

        // Instruct participant to open the target hand and press a button with the other hand 
        txtInstruction.text = "Open your target hand and press the start button with your other hand to confirm!";
        while (!ButtonScript.pressing)
            yield return null;
        ButtonScript.pressing = false;
        int l = 0;
        foreach (Transform d in digits)
        {
            openPose[l] = Mathf.Abs(d.localEulerAngles.z);
            l++;
        }

        for (int i = 0; i < 15; i++)
        {
            // 5% of this value is the error margin
            closeMargin[i] = closePose[i] * 0.05f;
            openMargin[i] = openPose[i] * 0.05f;
        }

        txtInstruction.text = "Ok, hand range calibrated!";
        yield return new WaitForSecondsRealtime(3f);

        txtInstruction.text = "If you like to repeat, press the start button again!";
        bool waitTimeUp = false;
        yield return new WaitForSecondsRealtime(5f);
        waitTimeUp = true;

        while (!ButtonScript.pressing | !waitTimeUp)
            yield return null;

        handRangeDone = true;
        txtInstruction.text = "Ok, hand range calibrated!";
        yield return null;
    }
    #endregion
}