using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grab_Throw : MonoBehaviour
{

    private GameObject grabbedObject;
    private bool grabbing;

    private Quaternion lastRotation, currentRotation;

    //public OVRInput.Controller controller;
    public string buttonName;
    public LayerMask grabMask;
    public float grabRadius;

    // Update is called once per frame
    void Update()
    {

        //if (!grabbing && Input.GetAxis(buttonName) == 1)
        if (!grabbing && Input.GetButtonDown(buttonName))
        {
            Debug.Log("Trigger Down!");
            GrabObject();
        }
        if (grabbing && Input.GetAxis(buttonName) < 1)
        {
            DropObject();
        }

        if (grabbedObject != null)
        {
            lastRotation = currentRotation;
            currentRotation = grabbedObject.transform.rotation;
        }
    }

    void GrabObject()
    {
        grabbing = true;

        RaycastHit[] hits;

        hits = Physics.SphereCastAll(transform.position, grabRadius, transform.forward, 0f, grabMask);

        if (hits.Length > 0)
        {
            int closesHit = 0;

            for (int i = 0; i < hits.Length; i++)
            {
                if (hits[i].distance < hits[closesHit].distance)
                {
                    closesHit = i;
                }
            }

            grabbedObject = hits[closesHit].transform.gameObject;
            grabbedObject.GetComponent<Rigidbody>().isKinematic = true;
            grabbedObject.transform.position = transform.position;
            grabbedObject.transform.parent = transform; // Lock grabbed object to the parent i.e. the hand
        }


    }

    void DropObject()
    {
        grabbing = false;

        if (grabbedObject != null)
        {
            grabbedObject.transform.parent = null;
            grabbedObject.GetComponent<Rigidbody>().isKinematic = false;
            //grabbedObject.GetComponent<Rigidbody>().velocity = OVRInput.GetLocalControllerVelocity(controller); // Apply the velocity of the hand to the object at release 
            grabbedObject.GetComponent<Rigidbody>().angularVelocity = GetAngularVelocity();//OVRInput.GetLocalControllerAngularVelocity(controller); // Apply the velocity of the hand to the object at release 

            grabbedObject = null;
        }

    }

    Vector3 GetAngularVelocity()
    {
        Quaternion deltaRotation = currentRotation * Quaternion.Inverse(lastRotation);
        return new Vector3(Mathf.DeltaAngle(0, deltaRotation.eulerAngles.x), Mathf.DeltaAngle(0, deltaRotation.eulerAngles.y), Mathf.DeltaAngle(0, deltaRotation.eulerAngles.z));
    }

}
