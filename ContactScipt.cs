using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ContactScipt : MonoBehaviour
{
    public int digitNum = 0; 
    public static bool[] inContact = new bool[5];
    public static GameObject grabbedObj;

    void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.tag == "grabTag")
        { inContact[digitNum] = true;
            grabbedObj = other.gameObject; 
        }
    }
    void OnCollisionStay(Collision other)
    {
        if (other.gameObject.tag == "grabTag")
            inContact[digitNum] = true;
    }
    void OnCollisionExit(Collision other)
    {
        if (other.gameObject.tag == "grabTag")
            inContact[digitNum] = false;
    }

}
