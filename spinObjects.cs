using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.CompilerServices;
using System;

public class spinObjects : MonoBehaviour {

	public GameObject[] spinObject;

	private Vector3 rotAngles; 
	private float angularSpeed = 25f;

	void Update () {

		rotAngles = new Vector3 (0f, Time.deltaTime * angularSpeed, 0f);
			
		try{
            foreach (GameObject i in spinObject)
            {
                i.transform.Rotate(rotAngles);
            }
		}catch(Exception e)
		{
			Debug.Log (e);
		}
	}
}
