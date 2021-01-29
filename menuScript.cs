using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class menuScript : MonoBehaviour {
   
	public Button startText; 
	public Button exitText; 
	public Text displayedText;  
	public InputField typedStuff; 
    

	void Start () 
	{		
		startText = startText.GetComponent<Button> ();
		exitText = exitText.GetComponent<Button> ();
		displayedText = displayedText.GetComponent<Text> ();
		typedStuff = typedStuff.GetComponent<InputField> ();
	}

	public void StartLevel()
	{
		Application.LoadLevel (1); 
	}

	public void ExitGame()
	{
		Application.Quit (); 
	}

	public void readKeyboard()
	{
		Debug.Log (typedStuff.text); 
		displayedText.text = typedStuff.text; 
	}



}
