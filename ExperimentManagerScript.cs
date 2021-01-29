using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ExperimentManagerScript : MonoBehaviour
{
    public ExperimentState expState;
    public AudioSource endExperimentSound;

    public GameObject TargetArch;
    public GameObject HandReal;
    public GameObject HandVirtual; 

    private Collider targetArchCollider;

    public float trialDuration = 5f;
    public float[] handGains;
    public float[] exponents;
    public float[] TargetSpeeds;
    public float[] trackingdiff;
    public int baselineBlocks = 2;
    public int adaptationBlocks = 5;
    public int washoutBlocks = 2;
    public int trials = 6;
    public TMPro.TMP_Text displayText;
    public TMPro.TMP_Text progressText;

    private bool endExperiment;
    private Coroutine startExperimentRoutine;

    public static int totalTrialNum;
    private TrialStruct trialData;

    public struct TrialStruct
    {
        public int[] propTargets;
        public int[] dialGain;
        public int[] targSpeeds;
        public int[] difficulty;

        public int[] handGainList;
        public int[] dialGainList;
        public int[] targSpeedList;
        public int[] targSizeList;

        public int[] targSpeeds_b;
        public int[] targSpeeds_a;
        public int[] targSpeeds_w;
    }

    private void Awake()
    {
        // Randomly select whether participant is in reward vs no-reward group
        expState.group = Random.Range(0, 2);
    }

    private void Start()
    {
        expState.trialDuration = trialDuration;
        targetArchCollider = TargetArch.GetComponent<Collider>();
        targetArchCollider.enabled = false;

        ResetExperimentParameters();

        //SequenceGenerator();
        BlockGenerator();
    }

    private void ResetExperimentParameters()
    {
        expState.currentTrial = 0;
        expState.score = 0;
        expState.scaleFactor = new Vector3(1f, 1f, 1f);
        expState.exPonent = exponents[0];
        expState.dialGaining = handGains[0];
    }

    private void Update()
    {
        if (expState.startTrial[0] == 1)
        {
            expState.endTrial = false;
            startExperimentRoutine = StartCoroutine(StartExperiment());
            expState.startTrial[0] = 0;
        }
        if (expState.pause)
        {
            targetArchCollider.enabled = false;

            if (startExperimentRoutine != null)
                StopCoroutine(startExperimentRoutine);
        }
        if ((expState.currentTrial) >= totalTrialNum-1 & !endExperiment | Input.GetKeyDown(KeyCode.J))
        {
            progressText.text = "Done: " + "100.00" + " %";
            StartCoroutine(EndExperimentSequence());
            displayText.text = "Experiment \n finished \n :)";
            endExperiment = true;
        }
    }

    void ShowRealHand(bool showReal)
    {
        MeshRenderer realHandRend = HandReal.GetComponent<MeshRenderer>();
        MeshRenderer virtualHandRend = HandVirtual.GetComponent<MeshRenderer>();

        Color handCol = realHandRend.material.color;  

        if(showReal)
        {
            realHandRend.material.color = new Color(handCol.r, handCol.g, handCol.b, 1f); // Alpha = 1 = visible
            virtualHandRend.material.color = new Color(handCol.r, handCol.g, handCol.b, 0f); // Alpha = 0 = invisible
        }
        else
        {
            realHandRend.material.color = new Color(handCol.r, handCol.g, handCol.b, 0f); // Alpha = 0 = invisible
            virtualHandRend.material.color = new Color(handCol.r, handCol.g, handCol.b, 1f); // Alpha = 1 = visible
        }
    }

    private IEnumerator EndExperimentSequence()
    {
        StopCoroutine(startExperimentRoutine);
        expState.pause = true;
        for (int e = 0; e < expState.startTrial.Length; e++)
        {
            expState.startTrial[e] = 0;
        }

        endExperimentSound.Play();
        while (endExperimentSound.isPlaying)
            yield return null;

        SceneManager.LoadSceneAsync(5, LoadSceneMode.Single);
        StopAllCoroutines();
        //Application.Quit();
    }

    private IEnumerator StartExperiment()
    {
        if (expState.currentTrial < (baselineBlocks * trials))
        {
            // Swith to real hand 
            //ShowRealHand(true);

            //blockText.text = "Baseline";
            expState.block = 0;
        }
        if (expState.currentTrial >= (baselineBlocks * trials) & expState.currentTrial <= (baselineBlocks * trials) + (adaptationBlocks * trials))
        {
            // Swith to real hand 
            //ShowRealHand(false);

            //blockText.text = "Adaptation";
            expState.block = 1;
        }
        if (expState.currentTrial >= (baselineBlocks * trials) + (adaptationBlocks * trials))
        {
            // Swith to real hand 
            //ShowRealHand(true);
            //blockText.text = "Washout";
            expState.block = 2;
        }

        displayText.text = "Start!!!";
        //expState.startTrial = false;
        targetArchCollider.enabled = true;

        //#New blocked way
        expState.angularSpeed = TargetSpeeds[trialData.targSpeedList[expState.currentTrial]];
        //expState.bendPerc[0] = handGains[trialData.handGainList[expState.currentTrial]];
        expState.dialGaining = handGains[trialData.handGainList[expState.currentTrial]];
        expState.exPonent = exponents[trialData.dialGainList[expState.currentTrial]];
        expState.trackDifficulty = trackingdiff[trialData.targSizeList[expState.currentTrial]];

        yield return new WaitForSecondsRealtime(trialDuration);
        expState.endTrial = true;
        expState.uploadData = true;

        targetArchCollider.enabled = false;
        expState.angularSpeed = 0f;
        displayText.text = "Stop!!!";

        yield return new WaitForSecondsRealtime(2f);

        expState.currentTrial++;

        float percentageDone = 100f * (float.Parse(expState.currentTrial.ToString()) / float.Parse(totalTrialNum.ToString()));
        progressText.text = "Done: " + Mathf.Round(percentageDone).ToString("F0") + " %";

        yield return null;
    }

    private void BlockGenerator()
    {
        int baselineTrials = baselineBlocks * trials;
        int adaptationTrials = adaptationBlocks * trials;
        int washoutTrials = washoutBlocks * trials;

        totalTrialNum = (baselineBlocks + adaptationBlocks + washoutBlocks) * trials;
        expState.numTrials = totalTrialNum; 

        trialData.propTargets = new int[totalTrialNum];
        trialData.dialGain = new int[totalTrialNum];
        trialData.targSpeeds = new int[totalTrialNum];
        trialData.difficulty = new int[totalTrialNum];

        // Create baseline trials:
        IEnumerable<int> handGainList_b = Enumerable.Repeat(0, baselineTrials);
        IEnumerable<int> dialGainList_b = Enumerable.Repeat(0, baselineTrials);
        IEnumerable<int> targSizeList_b = Enumerable.Repeat(0, baselineTrials);
        trialData.targSpeeds_b = expState.ShuffleArray(expState.SequenceLooper(baselineTrials, TargetSpeeds.Length));

        // Create adaptation trials:
        IEnumerable<int> handGainList_a = Enumerable.Repeat(1, adaptationTrials);
        IEnumerable<int> dialGainList_a = Enumerable.Repeat(1, adaptationTrials);
        IEnumerable<int> targSizeList_a = Enumerable.Repeat(0, adaptationTrials);
        trialData.targSpeeds_a = expState.ShuffleArray(expState.SequenceLooper(adaptationTrials, TargetSpeeds.Length));

        // Create washout trials:
        IEnumerable<int> handGainList_w = Enumerable.Repeat(0, washoutTrials);
        IEnumerable<int> dialGainList_w = Enumerable.Repeat(0, washoutTrials);
        IEnumerable<int> targSizeList_w = Enumerable.Repeat(0, washoutTrials);
        trialData.targSpeeds_w = expState.ShuffleArray(expState.SequenceLooper(washoutTrials, TargetSpeeds.Length));

        //#Convert all of the above to a long array of int for each experimental parameter
        trialData.handGainList = new int[totalTrialNum];
        trialData.dialGainList = new int[totalTrialNum];
        trialData.targSpeedList = new int[totalTrialNum];
        trialData.targSizeList = new int[totalTrialNum];

        int[] cpDat_b = handGainList_b.Cast<int>().ToArray();
        int[] cpDat_a = handGainList_a.Cast<int>().ToArray();
        int[] cpDat_w = handGainList_w.Cast<int>().ToArray();
        cpDat_b.CopyTo(trialData.handGainList, 0);
        cpDat_a.CopyTo(trialData.handGainList, cpDat_b.Length);
        cpDat_w.CopyTo(trialData.handGainList, cpDat_b.Length + cpDat_a.Length);

        cpDat_b = dialGainList_b.Cast<int>().ToArray();
        cpDat_a = dialGainList_a.Cast<int>().ToArray();
        cpDat_w = dialGainList_w.Cast<int>().ToArray();
        cpDat_b.CopyTo(trialData.dialGainList, 0);
        cpDat_a.CopyTo(trialData.dialGainList, cpDat_b.Length);
        cpDat_w.CopyTo(trialData.dialGainList, cpDat_b.Length + cpDat_a.Length);

        cpDat_b = trialData.targSpeeds_b;
        cpDat_a = trialData.targSpeeds_a;
        cpDat_w = trialData.targSpeeds_w;
        cpDat_b.CopyTo(trialData.targSpeedList, 0);
        cpDat_a.CopyTo(trialData.targSpeedList, cpDat_b.Length);
        cpDat_w.CopyTo(trialData.targSpeedList, cpDat_b.Length + cpDat_a.Length);

        cpDat_b = targSizeList_b.Cast<int>().ToArray();
        cpDat_a = targSizeList_a.Cast<int>().ToArray();
        cpDat_w = targSizeList_w.Cast<int>().ToArray();
        cpDat_b.CopyTo(trialData.targSizeList, 0);
        cpDat_a.CopyTo(trialData.targSizeList, cpDat_b.Length);
        cpDat_w.CopyTo(trialData.targSizeList, cpDat_b.Length + cpDat_a.Length);
    }

    private void SequenceGenerator()
    {
        totalTrialNum = (handGains.Length * exponents.Length * TargetSpeeds.Length * trackingdiff.Length * trials) / 3;

        trialData.propTargets = new int[totalTrialNum];
        trialData.dialGain = new int[totalTrialNum];
        trialData.targSpeeds = new int[totalTrialNum];
        trialData.difficulty = new int[totalTrialNum];

        trialData.propTargets = expState.SequenceLooper(totalTrialNum, handGains.Length);
        trialData.dialGain = expState.SequenceLooper(totalTrialNum, exponents.Length);
        trialData.targSpeeds = expState.SequenceLooper(totalTrialNum, TargetSpeeds.Length);
        trialData.difficulty = expState.SequenceLooper(totalTrialNum, trackingdiff.Length);

        trialData.propTargets = expState.ShuffleArray(trialData.propTargets);
        trialData.dialGain = expState.ShuffleArray(trialData.dialGain);
        trialData.targSpeeds = expState.ShuffleArray(trialData.targSpeeds);
        trialData.difficulty = expState.ShuffleArray(trialData.difficulty);
    }
}

//#Former way
//expState.angularSpeed = TargetSpeeds[trialData.targSpeeds[expState.currentTrial]];
//expState.handGain = handGains[trialData.propTargets[expState.currentTrial]];
//expState.dialGaining = exponents[trialData.dialGain[expState.currentTrial]];
//expState.trackDifficulty = trackingdiff[trialData.difficulty[expState.currentTrial]];

//displayText.text = "Speed: " + expState.angularSpeed.ToString("F2") + "\n" +
//                   "Difficulty: " + (1f - expState.trackDifficulty).ToString("F2") + "\n" +
//                   "Dial Gain: " + expState.exPonent.ToString("F2") + "\n" +
//                   "Hand Gain: " + expState.bendPerc[0].ToString("F2");