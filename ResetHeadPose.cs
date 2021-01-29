using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

public class ResetHeadPose : MonoBehaviour {

    public Transform referencePose; 

	void Start () {
		
	}
	
	void Update () {

        if (Input.GetKeyDown(KeyCode.R))
        {
            InputTracking.Recenter();
        }

        if (Input.GetKeyDown(KeyCode.X))
        {
            transform.position += new Vector3(0.0f, -0.1f, 0.0f); //+ new Vector3(0.0f,0.334f,0.0f) 
        }
        if (Input.GetKeyDown(KeyCode.A))
        {
            transform.position += new Vector3(-0.1f, 0.0f, 0.0f);
        }
        if (Input.GetKeyDown(KeyCode.S))
        {
            transform.position += new Vector3(0.0f, 0.1f, 0.0f);
        }
        if (Input.GetKeyDown(KeyCode.D))
        {
            transform.position += new Vector3(0.1f, 0.0f, 0.0f);
        }
        if (Input.GetKeyDown(KeyCode.G))
        {
            transform.position += new Vector3(0.0f, 0.0f, 0.1f);
        }
        if (Input.GetKeyDown(KeyCode.B))
        {
            transform.position += new Vector3(0.0f, 0.0f, -0.1f);
        }

    }
}
