using System.Collections;
using System.IO;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;

public class ServerTest_v1 : MonoBehaviour
{
    private string setURL = "http://www.bhamxr.com:1234/upload-manager.php";
    //public Texture2D tex = new Texture2D(2,2);

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Z))
        {
            //byte[] byteArray = File.ReadAllBytes(@"D:\SampleImage.png");
            byte[] byteArray = File.ReadAllBytes(@"C:\Users\danhq\Pictures\gblob.png");
            string str = Encoding.UTF8.GetString(byteArray);
            Debug.Log("Image loaded!!!");
            Debug.Log(str);

            //You can then load it to a texture
            //Texture2D tex = new Texture2D(2, 2);
            //tex.LoadImage(byteArray);

            StartCoroutine(UploadImage(byteArray));
            //StartCoroutine(UploadImage(str));
        }
    }

    //IEnumerator UploadImage(string imageBytes)
    private IEnumerator UploadImage(byte[] imageBytes)
    {
        WWWForm form = new WWWForm();
        form.AddBinaryData("photo", imageBytes, "gblob.png", "image/png");
        //form.AddField("postStartScr", imageBytes);

        UnityWebRequest www = UnityWebRequest.Post(setURL, form);
        yield return www.SendWebRequest();

        if (www.isNetworkError || www.isHttpError)
        {
            Debug.Log(www.error);
        }
        else
        {
            Debug.Log("Image uploaded!!!");
        }
    }
}