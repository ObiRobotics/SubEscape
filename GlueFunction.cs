using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlueFunction : MonoBehaviour {

    private Collider obj1;
    private Collider obj2;
    private int counter = 0;
    private Collider obj2Collider;

    private bool readRadius; 
    public bool glue; 

    private void OnTriggerEnter(Collider other)
    {
        if (counter == 0 && other.gameObject.layer == LayerMask.NameToLayer("GraspableObjects"))
        {
            obj1 = other;
            counter++;
            Debug.Log("Object 1 assigned");
            readRadius = true; 
        }
        else if (counter == 1 && other.gameObject.layer == LayerMask.NameToLayer("GraspableObjects"))
        {
            obj2 = other; 
            counter++;
            Debug.Log("Object 2 assigned");
            counter = 0;
            Debug.Log("Counter reset");
        }
    }

    private void Update()
    {
        if (readRadius)
        {
            float radius = obj1.transform.localScale.magnitude;

            if (obj2.enabled)
            {
                glue = true;
                GlueObjects(obj1, obj2, radius);
                readRadius = false; 
            }
        }
    }

    private void GlueObjects(Collider obj1, Collider obj2, float radius)
    {
        obj2.transform.parent = obj1.transform;
        obj2.attachedRigidbody.useGravity = false;
        obj2.attachedRigidbody.isKinematic = true;
        //obj2.enabled = false;
        //obj2.transform.position = new Vector3(obj1.transform.position.x,obj1.transform.position.y, obj1.transform.position.z + radius);
        //obj2.transform.localRotation = Quaternion.Euler(new Vector3(0.0f, 45.0f, 45.0f));
    }

}
