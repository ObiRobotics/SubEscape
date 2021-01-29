using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class closeEnvir : MonoBehaviour {


	public Animation closeEnv;

	public void closeGesture()
	{
		//GetComponent<Animation> ().Play ();
		closeEnv.Play("closeEnvirAnim");
		//GetComponent<Animation
		print ("Closing SpehereZZZ");
	}
		
}
