using UnityEngine;

public class triggerCollision : MonoBehaviour
{
    private ContactPoint[] cntPoints;

    private void OnCollisionEnter(Collision col)
    {
        cntPoints = col.contacts;
        Debug.Log("Name: " + col.gameObject.name + "\n Length: " + cntPoints.Length);
    }
}