using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class tableScript : MonoBehaviour {


	public openAnim _openAnim;
	public closeAnim _closeAnim;
	public closeEnvir _closeEnv; 

	private int ex = 0; 

	void OnTriggerEnter(Collider other)
	{		
		if (ex ==1) {
			_openAnim.openGesture ();
		}

		if(ex == 2){
			_closeAnim.closeGesture ();
			_closeEnv.closeGesture (); 
		}

		Debug.Log (ex);

		ex++; 
		if (ex == 3) {
			ex = 0; 
		}
	}

}
