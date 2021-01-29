using UnityEngine;

public class OnHit : MonoBehaviour
{
    public AudioSource clickSound;

    private void OnCollisionEnter()
    {
        clickSound.Play();
    }
}