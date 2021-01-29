using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class swipeScript : MonoBehaviour {

	public switchEnvirAnim _switchEnvrAnim; 
	public switchEnvirAnim _switchEnvrAnim_2; 
	public switchEnvirAnim _switchEnvrAnim_3;

	private string[] squence =  new string[]{"A", "D", "S"}; //
	private int x = 0;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}


	void OnTriggerEnter(Collider other)
	{		


		if (x == 1) //if(Input.GetKeyDown(KeyCode.A))
		{
			_switchEnvrAnim.switchGesture(); 
		}
		if (x == 2) //if(Input.GetKeyDown(KeyCode.D))
		{
			_switchEnvrAnim_2.switchGesture_2(); 
		}
		if (x == 3) //if(Input.GetKeyDown(KeyCode.S))
		{
			_switchEnvrAnim_3.switchGesture_3(); 
		}

		//Debug.Log("Trigger Enter...");
		//Debug.Log("Object Name: " + other.gameObject.transform.name);
		Debug.Log (x);
		//string[] squence = new string[]{"A", "D", "S"}; 
		//squence++;
		x++; // Upcount counter
		// Reset counter
		if (x == 4) {
			x = 0; 
		}
			
	}

}
