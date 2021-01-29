using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class Attracktor : MonoBehaviour {

    public float onPoint = 0.01f;
    public Vector3 offset;

    private bool attracktionComplete = false;


    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("AddOn"))
        {                
                other.collider.attachedRigidbody.isKinematic = true;
                other.collider.attachedRigidbody.useGravity = false;
                other.collider.enabled = false;
                other.collider.transform.parent = transform;
        }
    }
    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag("AddOn") && !attracktionComplete)
        {
            // Start attract coroutine 
            StartCoroutine(Attract(other));
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("AddOn"))
        {
            other.attachedRigidbody.isKinematic = false;
            other.attachedRigidbody.useGravity = true;
            other.transform.parent = null;
            other.enabled = true;
        }
    }

    IEnumerator Attract(Collider other)
    {
        other.gameObject.transform.position = Vector3.MoveTowards(other.gameObject.transform.position,transform.position, 0.0075f);
        //float distance = Vector3.Distance(other.gameObject.transform.position, transform.position + offset);
        //if (distance < onPoint)
        //{
        //    attracktionComplete = true; 
        //}
        yield return null; 
    }
}
