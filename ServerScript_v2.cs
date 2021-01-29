using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

[System.Serializable]
public class DataClass
{
    // Length of the float is the trial duration (15s) divided by the frame time step (1/60th) plus a few extra frames for safety
    [SerializeField] public string conditionInfo = string.Empty;

    [SerializeField] public string[] headPose = new string[1001];
    [SerializeField] public string[] handPose = new string[1001];
    [SerializeField] public string[] fingData = new string[1001];
    [SerializeField] public string[] vfingData = new string[1001];
    [SerializeField] public string[] dialData = new string[1001];
    [SerializeField] public string[] trialTime = new string[1001];
}

public class ServerScript_v2 : MonoBehaviour
{
    private DataClass expData = new DataClass();

    // Variables

    #region
    public ExperimentState expState;
    public Transform dial;
    public Transform dialTarget;

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
    private List<string> jointInfoList = new List<string>();

    private string userIDPost;
    private string conditionPost;
    private string trialNumPost;

    public int trialnum = -1;
    public bool eitherHand;

    private string setURL4 = "http://www.bhamxr.com:1234/JoesTaskJson_v2.php";

    private float startTime = 0f;

    private bool startOfTrial = false;
    private int oi = 0;
    private int oix = 0;
    private int frameNum = 0; 
    #endregion

    private void Start()
    {
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
            
            string groupCondition = "Reward";
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
                (expState.dialGaining * 10).ToString("F0") + "_" +
                trialnum.ToString();

            StartCoroutine(Upload2());
            Debug.Log("Upload Command Sent!!!");

        }

        if (expState.startTrial[0] == 1)
        {
            headPoseList.Clear();
            fingerPoseList.Clear();
            fingerStimPoseList.Clear();
            armPoseList.Clear();
            trialTimeList.Clear();
            dialNTargetList.Clear();

            trialnum++;
            //GameController.startTrial = false;
            startOfTrial = true;
            startTime = Time.time;
            //graspCyclePost = numberOfCloseCycles.ToString() + "," + graspRange.ToString();
        }
    }

    private void FixedUpdate()
    {
        /*---------------------------------------- Trial data collection start -------------------------------------------*/
        #region
        if (startOfTrial)
        {
            userIDPost = PlayerPrefs.GetString("userID") + "_" +
                         PlayerPrefs.GetString("age") + "_" +
                         PlayerPrefs.GetString("gender") + "_" +
                         PlayerPrefs.GetString("hand");

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
                            (expState.dialGaining * 10).ToString("F0") + "_" +
                             expState.actualTrialNum.ToString("F0") + "_" +
                             trialnum.ToString();

            frameNum = 0; 
            startTime = Time.time;
            startOfTrial = false;
        }

        headPoseList.Add(head.transform.position.x.ToString("F3") + ";" +
                         head.transform.position.y.ToString("F3") + ";" +
                         head.transform.position.z.ToString("F3") + ";" +
                         head.transform.localEulerAngles.x.ToString("F3") + ";" +
                         head.transform.localEulerAngles.y.ToString("F3") + ";" +
                         head.transform.localEulerAngles.z.ToString("F3"));

        armPoseList.Add(Forearm.transform.position.x.ToString("F3") + ";" +
                        Forearm.transform.position.y.ToString("F3") + ";" +
                        Forearm.transform.position.z.ToString("F3") + ";" +
                        Forearm.transform.localEulerAngles.x.ToString("F3") + ";" +
                        Forearm.transform.localEulerAngles.y.ToString("F3") + ";" +
                        Forearm.transform.localEulerAngles.z.ToString("F3")); 

        dialNTargetList.Add(dial.localRotation.eulerAngles.y.ToString("F3") + ";" +
                            dialTarget.localRotation.eulerAngles.y.ToString("F3") + ";" +
                            expState.score.ToString()); 

        for (int fi = 0; fi < fingerJoints.Length; fi++)
        {
            fingerPoseList.Add(frameNum.ToString() + ";" + fingerJoints[fi].name + ";" +
                               fingerJoints[fi].transform.position.x.ToString("F3") + ";" +
                               fingerJoints[fi].transform.position.y.ToString("F3") + ";" +
                               fingerJoints[fi].transform.position.z.ToString("F3") + ";" +
                               fingerJoints[fi].transform.localEulerAngles.z.ToString("F3")); 

            fingerStimPoseList.Add(frameNum.ToString() + ";" + fingerStimJoints[fi].name + ";" +
                                   fingerStimJoints[fi].transform.position.x.ToString("F3") + ";" +
                                   fingerStimJoints[fi].transform.position.y.ToString("F3") + ";" +
                                   fingerStimJoints[fi].transform.position.z.ToString("F3") + ";" +
                                   fingerStimJoints[fi].transform.localEulerAngles.z.ToString("F3")); 
        }
        
        frameNum++;

        float currTime = Time.time - startTime;
        trialTimeList.Add(currTime.ToString("F3"));
        #endregion
        /*---------------------------------------- Trial data collection end ---------------------------------------------*/
        if (expState.uploadData) // end of trial
        {
            StartCoroutine(Upload2());
            expState.uploadData = false;
        }
    }

    private IEnumerator Upload2()
    {
        // Convert to json and send to another site on the server
        expData.conditionInfo = userIDPost + conditionPost;
        expData.headPose = headPoseList.ToArray();
        expData.fingData = fingerPoseList.ToArray();
        expData.vfingData = fingerStimPoseList.ToArray();
        expData.handPose = armPoseList.ToArray();
        expData.dialData = dialNTargetList.ToArray();
        expData.trialTime = trialTimeList.ToArray();

        WWWForm form2 = new WWWForm();
        string jsonString = JsonConvert.SerializeObject(expData, Formatting.Indented);
        form2.AddField("postUserID", expData.conditionInfo);
        form2.AddField("postJsonData", jsonString);

        UnityWebRequest www2 = UnityWebRequest.Post(setURL4, form2);
        yield return www2.SendWebRequest();

        if (www2.isNetworkError || www2.isHttpError)
        {
            Debug.Log(www2.error);
        }

        // Empty text fields for next trials (potential for issues with next trial)
        headPoseList.Clear();
        fingerPoseList.Clear();
        fingerStimPoseList.Clear();
        armPoseList.Clear();
        dialNTargetList.Clear();
        trialTimeList.Clear();
    }
}

//------------------------------------------------------------------------------------------
// -------------------------------- Unused functions ---------------------------------------
//------------------------------------------------------------------------------------------
//private string List2Array(List<string> dataFromTrial)
//{
//    string[] trialDataArr = dataFromTrial.ToArray();
//    string msg = "";

//    foreach (string td in trialDataArr)
//    {
//        msg += td;
//    }

//    return msg;
//}

//private IEnumerator List2ArrayRoutine()
//{
//    handPosePost = List2Array(handPoseList);
//    armPosePost = List2Array(armPoseList);
//    fingerBendPost = List2Array(fingerBendList);
//    trialTimePost = List2Array(trialTimeList);
//    yield return null;
//}
//private List<string> RecordPose(GameObject[] objPoses)
//{
//    List<string> poseList = new List<string>();

//    foreach (GameObject fingerJoint in objPoses)
//    {
//        if (oi == fingerJoints.Length)
//        {
//            poseList.Add(fingerJoint.transform.position.x.ToString("F3") + "," +
//                               fingerJoint.transform.position.y.ToString("F3") + "," +
//                               fingerJoint.transform.position.z.ToString("F3") + "," +
//                               fingerJoint.transform.localEulerAngles.z.ToString("F3") + ":"); // : = to separate the frames
//            oi = 0;
//        }
//        else
//        {
//            poseList.Add(fingerJoint.transform.position.x.ToString("F3") + "," +
//                               fingerJoint.transform.position.y.ToString("F3") + "," +
//                               fingerJoint.transform.position.z.ToString("F3") + "," +
//                               fingerJoint.transform.localEulerAngles.z.ToString("F3") + "|"); // | = to separate each digit's joint
//        }
//        oi++;
//    }

//    return poseList;
//}
//------------------------------------------------------------------------------------------
//------------------------------------------------------------------------------------------
//------------------------------------------------------------------------------------------


//------------------------------------------------------------------------------------------
// Old way of saving data to a text file. <- This has now been replaced with a json file----
//------------------------------------------------------------------------------------------
//// # The separators (",") might be wrong and might need to be changed to (":")
//string[] stringArray0 = headPoseList.ToArray();
//headPosePost = string.Join(",", stringArray0);
//string[] stringArray1 = fingerPoseList.ToArray();
//fingerPosePost = string.Join(",", stringArray1);
//string[] stringArray2 = armPoseList.ToArray();
//armPosePost = string.Join(",", stringArray2);
//string[] stringArray4 = trialTimeList.ToArray();
//trialTimePost = string.Join(",", stringArray4);
//string[] stringArray5 = dialNTargetList.ToArray();
//dialNTargetPost = string.Join(",", stringArray5);
//string[] stringArray6 = fingerStimPoseList.ToArray();
//fingerStimPosePost = string.Join(",", stringArray6);

//WWWForm form = new WWWForm();

//form.AddField("postUserID", userIDPost);
//form.AddField("postUserCondition", conditionPost);
////form.AddField("postTrialNum", trialNumPost);
//form.AddField("postHeadPose", headPosePost);
//form.AddField("postArmPose", armPosePost);
//form.AddField("postFingerPose", fingerPosePost);
//form.AddField("postFingerStimPose", fingerStimPosePost);
//form.AddField("postDialNTargetScore", dialNTargetPost);
//form.AddField("postTrialTime", trialTimePost);

//UnityWebRequest www = UnityWebRequest.Post(setURL3, form);
//yield return www.SendWebRequest();

//if (www.isNetworkError || www.isHttpError)
//{
//    Debug.Log(www.error);
//}
//------------------------------------------------------------------------------------------
//------------------------------------------------------------------------------------------
//------------------------------------------------------------------------------------------