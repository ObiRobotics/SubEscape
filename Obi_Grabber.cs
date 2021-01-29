using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Obi_Grabber : MonoBehaviour
{

//    void Start()
//    {

//    }

//    void Update()
//    {
//        if (grasping)
//        {
//            GraspObject(graspedObject);
//            grasping = false;
//        }
//        if (!grasping && grasped)
//        {
//            ReleaseObject(graspedObject);
//        }
//    }



        

//    Vector3 GetAngularVelocity()
//{
//    Quaternion deltaRotation = handRotation * Quaternion.Inverse(prevHandRotation);
//    return new Vector3(Mathf.DeltaAngle(0, deltaRotation.eulerAngles.x), Mathf.DeltaAngle(0, deltaRotation.eulerAngles.y), Mathf.DeltaAngle(0, deltaRotation.eulerAngles.z));
//}

//private void OnTriggerStay(Collider other)
//{
//    if (other.gameObject.tag == "grabTag")
//    {
//        graspedObject = other;
//        grasping = true;
//        grasped = true;
//    }
//}
////private void OnTriggerExit(Collider other)
////{
////    grasping = false;
////}
//public void GraspObject(Collider other)
//{
//    if (digitAngles[1] > 30f)
//    {

//        other.transform.parent = transform;
//        if (other.attachedRigidbody)
//        {
//            other.attachedRigidbody.useGravity = false;
//            other.attachedRigidbody.isKinematic = true;
//        }
//    }
//}
//public void ReleaseObject(Collider other)
//{
//    if (digitAngles[1] < 30f)
//    {
//        Collider[] ts = GetComponentsInChildren<Collider>();
//        foreach (Collider t in ts)
//        {
//            if (t.gameObject.tag == "grabTag")
//            {
//                Rigidbody rb = GetComponent<Rigidbody>();
//                //Vector3 handVel = transform.InverseTransformDirection(rb.velocity);

//                t.transform.parent = null;
//                t.attachedRigidbody.isKinematic = false;
//                t.attachedRigidbody.velocity = handVel * 1.5f;
//                t.attachedRigidbody.angularVelocity = GetAngularVelocity();
//                t.attachedRigidbody.useGravity = true;
//                //Invoke("GravityFunction(other)", 1f);
//            }
//        }
//        grasped = false;
//    }
//}

//public void GravityFunction(Collider other)
//{
//    //other.attachedRigidbody.useGravity = true;
//    other.attachedRigidbody.isKinematic = false;
//}

}
