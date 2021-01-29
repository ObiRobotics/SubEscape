using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrecisionGrasp : MonoBehaviour
{
    public GameObject indexFing;
    public GameObject thumbFing;

    Rigidbody rb; 

    void Update()
    {
        bool getrb = true; 
        if(ContactScipt.inContact[0] && ContactScipt.inContact[1])
        {
            //float pinchAngle = Vector3.SignedAngle(thumbFing.transform.position, indexFing.transform.position, Vector3.up);
            //Debug.Log("Obj name: " + ContactScipt.grabbedObj.name);

            if (getrb)
            {
                ContactScipt.grabbedObj.transform.parent = transform;
                rb = ContactScipt.grabbedObj.GetComponent<Rigidbody>();
                rb.useGravity = false;
                rb.isKinematic = true;
                getrb = false;
            }

        }
        else
        {
            if (ContactScipt.grabbedObj != null)
            {
                ContactScipt.grabbedObj.transform.parent = null;
                rb.useGravity = true;
                rb.isKinematic = false;
            }
        }


    }

}
