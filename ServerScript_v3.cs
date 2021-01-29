using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class ServerScript_v3 : MonoBehaviour
{
    public GameObject Hand;
    public GameObject Forearm;
    public GameObject[] fingers;
    List<string> trialData = new List<string>();

    string userIDPost;
    string conditionPost;
    string trialNumPost;
    string handPosePost;
    string armPosePost;
    string fingerBendPost;
    string graspCyclePost;
    string trialTimePost;

    public string partid;
    public string trialnum;
    public string condition = "Gain_10%";
    public Vector3[] palmPosY;
    public Vector3[] palmPosZ;

    // Get these values from the main exp script (GameController.cs)
    int numberOfCloseCycles = 15;
    float graspRange = 8.2f;
    int userID = 59;

    string setURL = "http://www.bhamxr.com/AcceptUnityForm.php";
    string setURL2 = "http://www.bhamxr.com/AcceptUnityForm_v2.php";
    string setURL3 = "http://www.bhamxr.com/AcceptUnityForm_v3.php";

    float startTime = 0f;

    // Add header once to the data
    string headerPost =
        "UserID, Condition, trialNumber, HandPosX, HandPosY, HandPosZ, HandRotX, HandRotY, HandRotZ," +
        "ForearmPosX, ForearmPosY, ForearmPosZ, ForearmRotX, ForearmRotY, ForearmRotZ," +
        "HandCloseCycles, GraspRangem," +
        "TmbProxZRot, TmbMidZRot, TmbDistZRot," +
        "IdxProxZRot, IdxMidZRot, IdxDistZRot," +
        "MidProxZRot, MidMidZRot, MidDistZRot," +
        "RngProxZRot, RngMidZRot, RngDistZRot," +
        "PnkProxZRot, PnkMidZRot, PnkDistZRot";

    void FixedUpdate()
    {
        /*---------------------------------------- Trial data start -------------------------------------------*/
        userIDPost += userID.ToString();
        conditionPost += condition;
        trialNumPost += trialnum;
        handPosePost += Hand.transform.position.x.ToString() + "," +
                     Hand.transform.position.y.ToString() + "," +
                     Hand.transform.position.z.ToString() + "," +
                     Hand.transform.localEulerAngles.x.ToString() + "," +
                     Hand.transform.localEulerAngles.y.ToString();
        armPosePost += Forearm.transform.position.x.ToString() + "," +
                     Forearm.transform.position.y.ToString() + "," +
                     Forearm.transform.position.z.ToString() + "," +
                     Forearm.transform.localEulerAngles.x.ToString() + "," +
                     Forearm.transform.localEulerAngles.y.ToString();
        graspCyclePost += numberOfCloseCycles.ToString() + "," + graspRange.ToString();
        trialTimePost += (Time.time - startTime) + ",";

        foreach (GameObject finger in fingers)
        {
            fingerBendPost += finger.transform.localEulerAngles.z.ToString() + ",";
        }
        /*---------------------------------------- Trial data end ---------------------------------------------*/


        if (Input.GetKeyDown(KeyCode.H))
        {
            StartCoroutine(Upload2());
            //trialData.Clear();
            startTime = Time.time; // Reset the start timer on new trial 

            Debug.Log("Uploading... ");

        }

        if (Input.GetKeyDown(KeyCode.J))
        {
            StartCoroutine(SendMessage(partid, trialnum, condition));
            Debug.Log("Message sent: " + partid);
        }
    }

    IEnumerator Upload2()
    {
        WWWForm form = new WWWForm();
        form.AddField("postUserID", userIDPost);
        form.AddField("postUserCondition", conditionPost);
        form.AddField("postTrialNum", trialNumPost);
        form.AddField("postHandPose", handPosePost);
        form.AddField("postArmPose", armPosePost);
        form.AddField("postFingerAngles", fingerBendPost);
        form.AddField("postGraspCycleRange", graspCyclePost);
        form.AddField("postTrialTime", trialTimePost);
        form.AddField("postHeaders", headerPost);

        UnityWebRequest www = UnityWebRequest.Post(setURL3, form);
        yield return www.SendWebRequest();

        if (www.isNetworkError || www.isHttpError)
        {
            Debug.Log(www.error);
        }
        else
        {
            Debug.Log("Form upload complete!");
        }

        // Empty text fields for next trials (potential for issues with next trial)
        userIDPost = "";
        conditionPost = "";
        trialNumPost = "";
        handPosePost = "";
        armPosePost = "";
        fingerBendPost = "";
        graspCyclePost = "";
        trialTimePost = "";
        headerPost = "";

    }




    IEnumerator SendData(List<string> dataFromTrial)
    {
        string[] trialDataArr = dataFromTrial.ToArray();
        string msg = "";

        foreach (string td in trialDataArr)
        {
            msg += td;
            yield return null;
        }

        // Create form and add data to it 
        WWWForm form = new WWWForm();
        form.AddField("postMessage", msg);

        // Send the form to the server 
        WWW www = new WWW(setURL2, form);

        Debug.Log("Upload % " + www.uploadProgress);
        Debug.Log("response: " + www.responseHeaders);

        yield return www;
    }

    IEnumerator Upload(List<string> dataFromTrial)
    {

        string[] trialDataArr = dataFromTrial.ToArray();
        string msg = "";

        foreach (string td in trialDataArr)
        {
            msg += td;
            yield return null;
        }

        WWWForm form = new WWWForm();
        form.AddField("postMessage", msg);

        UnityWebRequest www = UnityWebRequest.Post(setURL2, form);
        yield return www.SendWebRequest();

        if (www.isNetworkError || www.isHttpError)
        {
            Debug.Log(www.error);
        }
        else
        {
            Debug.Log("Form upload complete!");
        }
    }

    IEnumerator SendMessage(string part_id, string trialnum, string condition)
    {
        WWWForm form = new WWWForm();
        form.AddField("participantIDPost", part_id);
        form.AddField("trialNumPost", trialnum);
        form.AddField("conditionTypePost", condition);

        string palmY_msg = ConvertArray2String(palmPosY);
        string palmZ_msg = ConvertArray2String(palmPosZ);

        form.AddField("palmy", palmY_msg);
        form.AddField("palmz", palmZ_msg);

        WWW www = new WWW(setURL, form);

        yield return null;
    }

    string ConvertArray2String(Vector3[] positionArr)
    {
        string palm_msg = null;
        foreach (Vector3 val in positionArr)
        {
            palm_msg += val.x.ToString() + "," +
                           val.y.ToString() + "," +
                           val.z.ToString() + "\n";
        }
        return palm_msg;
    }

}

