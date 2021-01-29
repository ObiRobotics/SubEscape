using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class closeAnim : MonoBehaviour {

	public void closeGesture()
	{
		GetComponent<Animation> ().Play ();
		print ("Closing Speheres");
	}

}
