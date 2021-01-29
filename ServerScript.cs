using System.Collections;
using UnityEngine;

public class ServerScript : MonoBehaviour
{
    //[SerializeField]
    //private string AccessKeyID = "AKIARMYOCDO3STQI2Q42";
    //[SerializeField]
    //private string AccessKey = "kp9szSqJ9sq2AayhMXyXkY+pOHPtw+JootJ1YA0Q";

    public string SendMgs;
    public string ReadMsg;

    //string setURL = "http://localhost:80/PostName.php?name=";
    //string getURL = "http://localhost:80/ReadName.php";

    //string setURL = "https://s3.eu-central-1.amazonaws.com/com.datasbuck.mybucket/PostName.php?name=";
    //string getURL = "https://s3.eu-central-1.amazonaws.com/com.datasbuck.mybucket/ReadName.php";

    private string setURL = "http://bhamxr.com/PostName.php?name=";
    private string getURL = "http://bhamxr.com/ReadName.php";

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.J))
        {
            StartCoroutine(SendMessage(SendMgs));
            Debug.Log("Message sent: " + SendMgs);
        }
        if (Input.GetKeyDown(KeyCode.K))
        {
            StartCoroutine(GetMessage());
            Debug.Log("Message received: " + ReadMsg);
        }
    }

    private IEnumerator SendMessage(string msg)
    {
        string URL = setURL + msg;
        WWW www = new WWW(URL);
        yield return www;
    }

    private IEnumerator GetMessage()
    {
        string URL = getURL;
        WWW www = new WWW(URL);
        yield return www;

        ReadMsg = www.text;
    }
}