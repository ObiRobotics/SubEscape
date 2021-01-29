using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OVR_HandController : MonoBehaviour {

    //public OVRInput.Controller myControoler;
    public Vector3 posOffset;

    
    void Start () {
     
    }
	
	void Update () {
        //transform.localPosition = OVRInput.GetLocalControllerPosition(myControoler) + posOffset;
        //transform.localRotation = OVRInput.GetLocalControllerRotation(myControoler);

        //if (OVRInput.GetDown(OVRInput.Button.PrimaryHandTrigger))
        //{
        //    Debug.Log("Button down!");
        //    CloseHand();
        //}
    }

    void CloseHand()
    {
        grabZoneBehaviour.animtr.Play("fingerBend3");
    }
}
