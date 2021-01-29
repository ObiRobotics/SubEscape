using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeepObject : MonoBehaviour {

	void Start () {
        DontDestroyOnLoad(gameObject); 
	}
	
}
