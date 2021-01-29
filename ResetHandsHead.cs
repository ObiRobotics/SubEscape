using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResetHandsHead : MonoBehaviour {


    public Transform rightHand, leftHand, headObject; 

	void Start () {
		
	}
	
	void Update ()
    {

        if (Input.GetKeyDown(KeyCode.X))
        {
            headObject.position = new Vector3(transform.position.x, transform.position.y+0.65f, transform.position.z-0.85f);
            Obi_HandReader.offsetPos = transform.position;
            //Obi_HandReader.EulerOffset2 = transform.rotation.eulerAngles; 
        }
		
	}

}
