using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using CoherentNoise.Generation.Combination;
using System.Security.Cryptography;
using System.Xml.Linq;

public class gestureController : MonoBehaviour {

	public openAnim _openAnim;
	public switchEnvirAnim _switchEnvrAnim; 
	public switchEnvirAnim _switchEnvrAnim_2; 
	public switchEnvirAnim _switchEnvrAnim_3;

	private string[] squence = {"A", "D", "S"};
	private int x = 1;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        
		if(Input.GetButtonDown("Fire1"))
		{
				_openAnim.openGesture();
		}
			

		if(Input.GetKeyDown(KeyCode.A))
		{
			_switchEnvrAnim.switchGesture(); 
		}

		if(Input.GetKeyDown(KeyCode.D))
		{
			_switchEnvrAnim_2.switchGesture_2(); 
		}
		if(Input.GetKeyDown(KeyCode.S))
		{
			_switchEnvrAnim_3.switchGesture_3(); 
		}



    }




}
