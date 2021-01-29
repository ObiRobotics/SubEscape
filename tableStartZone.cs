using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using Mono.Cecil;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System;

public class tableStartZone : MonoBehaviour
{

	private GameObject shatterSphere;

	public Image blackScreen;
	public Animator animtr;

	// Use this for initialization
	void Start ()
	{

		shatterSphere = Resources.Load ("ShatterSphere") as GameObject;

	}
	
	// Update is called once per frame
	void Update ()
	{
		
	}

	void OnTriggerEnter (Collider other)
	{
		if (other.transform.CompareTag ("sphere")) {
			String levelName;
			String otherName = other.gameObject.transform.name;

			Destroy (other.gameObject); 
			Instantiate (shatterSphere, new Vector3 (other.transform.position.x, other.transform.position.y + 0.05f, other.transform.position.z), Quaternion.identity);
			//SceneManager.LoadScene ("ARI_02",LoadSceneMode.Single);
			Debug.Log(otherName);

			if (String.Compare (otherName, "Environment_Sphere_1") == 0) {
				levelName = "ARI_02"; 
				Debug.Log (levelName);
				StartCoroutine (Fading (levelName)); 
			} else if (String.Compare (otherName, "Environment_Sphere_2") == 0) {
				levelName = "ARI_03"; 
				Debug.Log (levelName);

				StartCoroutine (Fading (levelName)); 
			} else if (String.Compare (otherName, "Environment_Sphere_3") == 0) {
				levelName = "ARI_04"; 
				Debug.Log (levelName);

				StartCoroutine (Fading (levelName)); 
			}

		}
	
	}

	IEnumerator Fading (String levelName)
	{
		Debug.Log ("Shatter");
		animtr.SetBool ("Fade", true);
		yield return new WaitUntil (() => blackScreen.color.a == 1);
		SceneManager.LoadScene (levelName, LoadSceneMode.Single);

	}


}
