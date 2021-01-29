using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class moveHead : MonoBehaviour {

	public float speed = 0.01f; 

	// Update is called once per frame
	void Update () 
	{
		transform.Translate (0.0f, 0.0f, 0.01f*speed*Time.deltaTime);
	}
}
