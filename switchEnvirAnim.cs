using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class switchEnvirAnim : MonoBehaviour {

	public Animation switchSpheres_1; 
	public Animation switchSpheres_2; 
	public Animation switchSpheres_3;
    public Animation benFinger;

    public void switchGesture()
	{
		//GetComponent<Animation> ().Play ();
		switchSpheres_1.Play("switchSelect_1");
		switchSpheres_1.Play ("switchSelect_1", PlayMode.StopAll);
		//GetComponent<Animation
		print ("Switching Speheres 1");
	}

	public void switchGesture_2()
	{
		//GetComponent<Animation> ().Play ();
		switchSpheres_2.Play("switchSelect_2");
		switchSpheres_2.Play ("switchSelect_2", PlayMode.StopAll);
		//GetComponent<Animation
		print ("Switching Speheres 2");
	}

	public void switchGesture_3()
	{
		//GetComponent<Animation> ().Play ();
		switchSpheres_3.Play("switchSelect_3");
		switchSpheres_3.Play ("switchSelect_3", PlayMode.StopAll);
		//GetComponent<Animation
		print ("Switching Speheres 3");
	}

    public void fingerBen()
    {
        //GetComponent<Animation> ().Play ();
        benFinger.Play("fingerBendzCP");
        benFinger.Play("fingerBendzCP", PlayMode.StopAll);
        //GetComponent<Animation
        print("Bend Finger");
    }

}
