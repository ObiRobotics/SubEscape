using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;


public class SendData2Server_v2 : MonoBehaviour
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
        List<IMultipartFormSection> formData = new List<IMultipartFormSection>();
        formData.Add(new MultipartFormDataSection("field1=foo&field2=bar"));
        formData.Add(new MultipartFormFileSection("my file data", "myfile.txt"));

        UnityWebRequest www = UnityWebRequest.Post("http://localhost:80/", formData);
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

    //IEnumerator Upload()
    //{
    //    //byte[] myData = System.Text.Encoding.UTF8.GetBytes("This is some test data");
    //    string myData = "This is some test data";
    //    //UnityWebRequest www = UnityWebRequest.Put("http://www.my-server.com/upload", myData);
    //    UnityWebRequest www = UnityWebRequest.Put("http://localhost:80/", myData);

    //    yield return www.SendWebRequest();

    //    if (www.isNetworkError || www.isHttpError)
    //    {
    //        Debug.Log(www.error);
    //    }
    //    else
    //    {
    //        Debug.Log("Upload complete!");
    //    }
    //}
}