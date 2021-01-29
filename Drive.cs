using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Drive : MonoBehaviour {

    public float speed = 2.0f;
    public float turnSpeed = 1.0f;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKey(KeyCode.T)) { transform.position += transform.forward * speed * Time.deltaTime; }
        if (Input.GetKey(KeyCode.G)) { transform.position -= transform.forward * speed * Time.deltaTime; }
        if (Input.GetKey(KeyCode.F)) { transform.rotation *= Quaternion.AngleAxis(-turnSpeed, Vector3.up); }
        if (Input.GetKey(KeyCode.H)) { transform.rotation *= Quaternion.AngleAxis(turnSpeed, Vector3.up); }

    }
}
