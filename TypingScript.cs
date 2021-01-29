using UnityEngine;

public class TypingScript : MonoBehaviour
{
    public static string Letter;
    public static bool Typing;
    public AudioSource keyclick;

    //Image keyImg;
    //Color origColor;

    private void OnTriggerEnter(Collider otr)
    {
        //StartCoroutine(TypeSequence(otr));
        keyclick.Play();
        Letter = otr.gameObject.name;
        Typing = true;
    }

    private void OnTriggerExit(Collider otr)
    {
        Typing = false;
    }

    //IEnumerator TypeSequence(Collider otr)
    //{
    //    keyclick.Play();
    //    Letter = otr.gameObject.name;
    //    Typing = true;
    //    yield return new WaitForSecondsRealtime(0.25f);
    //}
}

//void OnCollisionEnter(Collision otr)
//{
//    keyclick.Play();
//    Letter = gameObject.name;
//    Typing = true;
//}

//void OnCollisionExit(Collision otr)
//{
//    Typing = false;
//}