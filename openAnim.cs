using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class openAnim : MonoBehaviour {

	public void openGesture()
	{
		GetComponent<Animation> ().Play ();
		print ("Openning Speheres");
	}

}
