using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
using UnityEngine.UI; 

public class SendData2Server_v1 : MonoBehaviour
{
    public Button PostBucketButton = null;

    void Start()
    {
        PostBucketButton.onClick.AddListener(() => { PostObject(); });
    }

    void PostObject()
    {
        StartCoroutine(Upload());
    }

    IEnumerator Upload()
    {
        //byte[] myData = System.Text.Encoding.UTF8.GetBytes("This is some test data");
        string myData = "This is some test data";
        //UnityWebRequest www = UnityWebRequest.Put("http://www.my-server.com/upload", myData);
        UnityWebRequest www = UnityWebRequest.Put("http://localhost:80/", myData);

        yield return www.SendWebRequest();

        if (www.isNetworkError || www.isHttpError)
        {
            Debug.Log(www.error);
        }
        else
        {
            Debug.Log("Upload complete!");
        }
    }
}