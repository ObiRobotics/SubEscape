using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class ServerJson_v1 : MonoBehaviour
{
    public ExperimentState expState;
    public Transform dial;
    public Transform dialTarget;

    private string dbUsername = "questuser";
    private string dbPaswrd = "Ugauga12";
    private string dbDatabase = "hand_rehab_database";

    private GameObject Forearm;
    private GameObject Hand;
    public GameObject[] Hands;
    public GameObject[] Forearms;
    private GameObject[] fingerJoints;
    private GameObject[] fingerStimJoints;
    public GameObject[] fingersR;
    public GameObject[] fingersL;
    public GameObject[] fingersStimL;
    public GameObject head;

    private List<string> headPoseList = new List<string>();
    private List<string> handPoseList = new List<string>();
    private List<string> armPoseList = new List<string>();
    private List<string> fingerBendList = new List<string>();
    private List<string> fingerPoseList = new List<string>();
    private List<string> fingerStimPoseList = new List<string>();
    private List<string> trialTimeList = new List<string>();
    private List<string> dialNTargetList = new List<string>();

    private string userIDPost;
    private string conditionPost;
    private string trialNumPost;

    //private string graspCyclePost;
    private string headPosePost;

    private string handPosePost;
    private string armPosePost;
    private string fingerBendPost;
    private string fingerPosePost;
    private string fingerStimPosePost;
    private string trialTimePost;
    private string dialNTargetPost;

    // Get these values from the main exp script (GameController.cs)
    //private int numberOfCloseCycles = 7;
    //private float graspRange = 4.9f;

    public int trialnum = -1;
    public bool eitherHand;

    //public string condition = "Gain_77%";
    private string headerPost;

    private string setURL3 = "http://www.bhamxr.com:1234/JoesTask_v4.php";

    private float startTime = 0f;

    private bool startOfTrial = false;
    private int oi = 0;
    private int oix = 0;

    private void Start()
    {
        headerPost =
        "UserID, Condition, trialNumber, HandPosX, HandPosY, HandPosZ, HandRotX, HandRotY, HandRotZ," +
        "ForearmPosX, ForearmPosY, ForearmPosZ, ForearmRotX, ForearmRotY, ForearmRotZ," +
        "HandCloseCycles, GraspRangem," +
        "TmbProxZRot, TmbMidZRot, TmbDistZRot," +
        "IdxProxZRot, IdxMidZRot, IdxDistZRot," +
        "MidProxZRot, MidMidZRot, MidDistZRot," +
        "RngProxZRot, RngMidZRot, RngDistZRot," +
        "PnkProxZRot, PnkMidZRot, PnkDistZRot";

        // Determine participant handedness and select the opposite hand as the target hand
        string hand = PlayerPrefs.GetString("hand");
        if (eitherHand)
        {
            if (hand.Contains("R"))
            {
                Forearm = Forearms[0];
                Hand = Hands[0];
                fingerJoints = fingersL;
                fingerStimJoints = fingersStimL;
            }
            else if (hand.Contains("L"))
            {
                Forearm = Forearms[1];
                Hand = Hands[1];
                fingerJoints = fingersR;
            }
        }
        else // Always use the left hand
        {
            Forearm = Forearms[0];
            Hand = Hands[0];
            fingerJoints = fingersL;
            fingerStimJoints = fingersStimL;
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.V))
        {
            userIDPost = PlayerPrefs.GetString("userID") + "_" +
                         PlayerPrefs.GetString("age") + "_" +
                         PlayerPrefs.GetString("gender") + "_" +
                         PlayerPrefs.GetString("hand");
            headPosePost = "HeadPose";
            armPosePost = "ArmPose";
            fingerPosePost = "FingerPose";
            fingerStimPosePost = "FingerStimPose";
            trialTimePost = "Time";
            dialNTargetPost = "DialnTargetAngle";
            string groupCondition = "Reward";

            conditionPost = "_" + groupCondition + "_" +
                (expState.exPonent * 1000f).ToString("F0") + "_" +
                expState.actualTrialNum.ToString("F0") + "_" +
                (expState.dialGaining * 10).ToString("F0");
            trialNumPost = trialnum.ToString();

            //if (expState.uploadData) // end of trial
            //{
            Debug.Log("Upload Command Sent!!!");
            StartCoroutine(Upload2());

            //    expState.uploadData = false;
            //}
        }

        if (expState.startTrial[0] == 1)
        {
            trialnum++;
            //GameController.startTrial = false;
            startOfTrial = true;
            startTime = Time.time;
            //graspCyclePost = numberOfCloseCycles.ToString() + "," + graspRange.ToString();
        }
    }

    private void FixedUpdate()
    {
        /*---------------------------------------- Trial data start -------------------------------------------*/
        if (startOfTrial)
        {
            userIDPost = PlayerPrefs.GetString("userID") + "_" +
                         PlayerPrefs.GetString("age") + "_" +
                         PlayerPrefs.GetString("gender") + "_" +
                         PlayerPrefs.GetString("hand");

            headPosePost = "HeadPose";
            armPosePost = "ArmPose";
            fingerPosePost = "FingerPose";
            fingerStimPosePost = "FingerStimPose";
            trialTimePost = "Time";
            dialNTargetPost = "DialnTargetAngle";

            string groupCondition = "";
            if (expState.group == 0)
            {
                groupCondition = "NoReward";
            }
            else
            {
                groupCondition = "Reward";
            }

            conditionPost = "_" + groupCondition + "_" +
                            (expState.exPonent * 1000f).ToString("F0") + "_" +
                            expState.actualTrialNum.ToString("F0") + "_" +
                            (expState.dialGaining * 10).ToString("F0");
            trialNumPost = trialnum.ToString();

            oi = 0;
            oix = 0;

            startTime = Time.time;

            startOfTrial = false;
        }

        // : = to separate the frames
        // | = to separate each digit's joint

        headPoseList.Add(head.transform.position.x.ToString("F3") + "," +
                         head.transform.position.y.ToString("F3") + "," +
                         head.transform.position.z.ToString("F3") + "," +
                         head.transform.localEulerAngles.x.ToString("F3") + "," +
                         head.transform.localEulerAngles.y.ToString("F3") + "," +
                         head.transform.localEulerAngles.z.ToString("F3") + ":"); // : = to separate the frames

        armPoseList.Add(Forearm.transform.position.x.ToString("F3") + "," +
                        Forearm.transform.position.y.ToString("F3") + "," +
                        Forearm.transform.position.z.ToString("F3") + "," +
                        Forearm.transform.localEulerAngles.x.ToString("F3") + "," +
                        Forearm.transform.localEulerAngles.y.ToString("F3") + "," +
                        Forearm.transform.localEulerAngles.z.ToString("F3") + ":"); // : = to separate the frames

        dialNTargetList.Add(dial.localRotation.eulerAngles.y.ToString("F3") + "," +
                            dialTarget.localRotation.eulerAngles.y.ToString("F3") + "," +
                            expState.score.ToString() + ":"); // : = to separate the frames
        //Debug.Log(dial.localRotation.eulerAngles.y.ToString("F3"));

        foreach (GameObject fingerJoint in fingerJoints)
        {
            if (oi == fingerJoints.Length)
            {
                fingerPoseList.Add(fingerJoint.transform.position.x.ToString("F3") + "," +
                                   fingerJoint.transform.position.y.ToString("F3") + "," +
                                   fingerJoint.transform.position.z.ToString("F3") + "," +
                                   fingerJoint.transform.localEulerAngles.z.ToString("F3") + ":"); // : = to separate the frames
                oi = 0;
            }
            else
            {
                fingerPoseList.Add(fingerJoint.transform.position.x.ToString("F3") + "," +
                                   fingerJoint.transform.position.y.ToString("F3") + "," +
                                   fingerJoint.transform.position.z.ToString("F3") + "," +
                                   fingerJoint.transform.localEulerAngles.z.ToString("F3") + "|"); // | = to separate each digit's joint
            }
            oi++;
        }

        foreach (GameObject fingerStimJoint in fingerStimJoints)
        {
            if (oix == fingerStimJoints.Length)
            {
                fingerStimPoseList.Add(fingerStimJoint.transform.position.x.ToString("F3") + "," +
                                   fingerStimJoint.transform.position.y.ToString("F3") + "," +
                                   fingerStimJoint.transform.position.z.ToString("F3") + "," +
                                   fingerStimJoint.transform.localEulerAngles.z.ToString("F3") + ":"); // : = to separate the frames
                oix = 0;
            }
            else
            {
                fingerStimPoseList.Add(fingerStimJoint.transform.position.x.ToString("F3") + "," +
                                   fingerStimJoint.transform.position.y.ToString("F3") + "," +
                                   fingerStimJoint.transform.position.z.ToString("F3") + "," +
                                   fingerStimJoint.transform.localEulerAngles.z.ToString("F3") + "|"); // | = to separate each digit's joint
            }
            oix++;
        }

        float currTime = Time.time - startTime;
        //trialTimePost = trialTimePost + currTime + "\n";
        trialTimeList.Add(currTime.ToString() + ":");

        if (expState.uploadData) // end of trial
        {
            //Debug.Log("Trial time is over!!!");
            StartCoroutine(Upload2());

            expState.uploadData = false;
        }

        /*---------------------------------------- Trial data end ---------------------------------------------*/
    }

    private IEnumerator Upload2()
    {
        // # The separators (",") might be wrong and might need to be changed to (":")
        string[] stringArray0 = headPoseList.ToArray();
        headPosePost = string.Join(",", stringArray0);
        string[] stringArray1 = fingerPoseList.ToArray();
        fingerPosePost = string.Join(",", stringArray1);
        string[] stringArray2 = armPoseList.ToArray();
        armPosePost = string.Join(",", stringArray2);
        string[] stringArray4 = trialTimeList.ToArray();
        trialTimePost = string.Join(",", stringArray4);
        string[] stringArray5 = dialNTargetList.ToArray();
        dialNTargetPost = string.Join(",", stringArray5);
        string[] stringArray6 = fingerStimPoseList.ToArray();
        fingerStimPosePost = string.Join(",", stringArray6);

        WWWForm form = new WWWForm();
        //form.AddField("dbUsername", dbUsername);
        //form.AddField("dbPass", dbPaswrd);
        //form.AddField("dbDataBase", dbDatabase);

        form.AddField("postUserID", userIDPost);
        form.AddField("postUserCondition", conditionPost);
        form.AddField("postTrialNum", trialNumPost);
        form.AddField("postHeadPose", headPosePost);
        form.AddField("postArmPose", armPosePost);
        form.AddField("postFingerPose", fingerPosePost);
        form.AddField("postFingerStimPose", fingerStimPosePost);
        form.AddField("postDialNTargetScore", dialNTargetPost);
        form.AddField("postTrialTime", trialTimePost);

        //form.AddField("postHeaders", headerPost);
        //form.AddField("postGraspCycleRange", graspCyclePost);

        UnityWebRequest www = UnityWebRequest.Post(setURL3, form);
        yield return www.SendWebRequest();

        if (www.isNetworkError || www.isHttpError)
        {
            Debug.Log(www.error);
        }
        // else
        // {
        //     Debug.Log("Form upload complete!");
        // }

        // Empty text fields for next trials (potential for issues with next trial)
        //handPoseList.Clear();
        //armPoseList.Clear();
        //fingerBendList.Clear();
        //trialTimeList.Clear();
        headPoseList.Clear();
        fingerPoseList.Clear();
        fingerStimPoseList.Clear();
        armPoseList.Clear();
        trialTimeList.Clear();
        dialNTargetList.Clear();
    }

    private string List2Array(List<string> dataFromTrial)
    {
        string[] trialDataArr = dataFromTrial.ToArray();
        string msg = "";

        foreach (string td in trialDataArr)
        {
            msg += td;
        }

        return msg;
    }

    private IEnumerator List2ArrayRoutine()
    {
        handPosePost = List2Array(handPoseList);
        armPosePost = List2Array(armPoseList);
        fingerBendPost = List2Array(fingerBendList);
        trialTimePost = List2Array(trialTimeList);
        yield return null;
    }

    private List<string> RecordPose(GameObject[] objPoses)
    {
        List<string> poseList = new List<string>();

        foreach (GameObject fingerJoint in objPoses)
        {
            if (oi == fingerJoints.Length)
            {
                poseList.Add(fingerJoint.transform.position.x.ToString("F3") + "," +
                                   fingerJoint.transform.position.y.ToString("F3") + "," +
                                   fingerJoint.transform.position.z.ToString("F3") + "," +
                                   fingerJoint.transform.localEulerAngles.z.ToString("F3") + ":"); // : = to separate the frames
                oi = 0;
            }
            else
            {
                poseList.Add(fingerJoint.transform.position.x.ToString("F3") + "," +
                                   fingerJoint.transform.position.y.ToString("F3") + "," +
                                   fingerJoint.transform.position.z.ToString("F3") + "," +
                                   fingerJoint.transform.localEulerAngles.z.ToString("F3") + "|"); // | = to separate each digit's joint
            }
            oi++;
        }

        return poseList;
    }
}