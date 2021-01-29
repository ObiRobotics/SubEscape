using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectCombiner : MonoBehaviour {


    private Collider obj1;
    private Collider obj2;
    private int counter = 0;
    private Collider obj2Collider; 

    private void OnTriggerEnter(Collider other)
    {
        if (counter == 0 && other.gameObject.layer == LayerMask.NameToLayer("GraspableObjects"))
        {
            obj1 = other;
            counter++;
            Debug.Log("Object 1 assigned");
        }
        else if (counter == 1 && other.gameObject.layer == LayerMask.NameToLayer("GraspableObjects"))
        {

            other.transform.parent = obj1.transform;
            other.attachedRigidbody.useGravity = false;
            other.attachedRigidbody.isKinematic = true;
            other.enabled = false;
            other.transform.position = obj1.transform.position;
            other.transform.localRotation = Quaternion.Euler(new Vector3(0.0f, 45.0f, 45.0f));

            //obj2Collider = obj2.GetComponent<Collider>();
            counter++;
            Debug.Log("Object 2 assigned");
            counter = 0;
            Debug.Log("Counter reset");
            CombineObjects(); 
        }
    }

    private void CombineObjects()
    {
        //obj2.enabled = false;
        //obj2.transform.parent = obj1.transform;
        //obj2.transform.position = obj1.transform.position;
        //obj2.transform.localRotation = Quaternion.Euler(new Vector3(0.0f, 45.0f, 45.0f));

        Debug.Log("Objects combined");
    }

}
