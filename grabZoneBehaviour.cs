using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class grabZoneBehaviour : MonoBehaviour {

    public GameObject Hand;
    //   public GameObject city;

    private int hand_in = 0;
    private GameObject act_sphere;
    private Vector3 init_sphere_pos;
    private int inside = 0;
    private float currentScale;

	public static Animation animtr; 

	void Start()
    {

		//animtr = Hand.GetComponent<Animation> ();
  //      //city = GameObject.FindGameObjectWithTag("City");

  //      //act_sphere = GameObject.FindGameObjectWithTag("sphere2");
		//init_sphere_pos = GameObject.FindGameObjectWithTag("open_environment").transform.position;

    }

    void Update()
    {
        //try{
        //if (hand_in == 1)
        //{
        //	act_sphere.transform.position = Vector3.Lerp(new Vector3(Hand.transform.position.x-0.0027f,Hand.transform.position.y-0.05f,Hand.transform.position.z+0.05f), act_sphere.transform.position, Time.deltaTime * 50f);
        //}else if (hand_in == 0)
        //{
        //	act_sphere.transform.position = Vector3.Lerp(init_sphere_pos, act_sphere.transform.position, Time.deltaTime * 50f);
        //}
        //}catch(Exception e) {
        //	Debug.Log (e);
        //}

        //        if (hand_in == 1)
        //        {
        //			act_sphere.transform.position = Vector3.Lerp(new Vector3(Hand.transform.position.x-0.0027f,Hand.transform.position.y-0.05f,Hand.transform.position.z+0.05f), act_sphere.transform.position, Time.deltaTime * 50f);
        //        }
        //
        //        else if (hand_in == 0)
        //        {
        //            act_sphere.transform.position = act_sphere_pos; 
        //        }

        //CityTransform();


    }

    //public void CityTransform()
    //{
    //    //try
    //    //{
    //    //    //currentScale = city.transform.localScale.x;

    //    //    if (inside == 1)
    //    //    {
    //            if (FingerRotate.midAng > 30f) {
    //                float currentScale = Boundary.cityScape.transform.localScale.x;

    //                Boundary.currentScale = new Vector3(Hand.transform.position.y, Hand.transform.position.y, Hand.transform.position.y);

    //            }

    ////        }


    ////    }
    ////    catch (Exception e)
    ////    {
    ////        Debug.Log(e);
    ////    }
    //}
    
    void OnTriggerEnter(Collider other)
    {

		if (other.transform.CompareTag("handTag"))
		{
			animtr.Play("fingerBend3");


			//Debug.Log("Trigger Enter...");
			//Debug.Log("Object Name: " + other.gameObject.transform.name);

			hand_in = 1; 
			
		}
		 
			
    }

    void OnTriggerStay(Collider other)
    {

        //if (other.transform.CompareTag("sphere"))
        //{
        //	act_sphere = other.gameObject;
        //Debug.Log (act_sphere.transform.name + " is active");
        //}

        if (other.transform.CompareTag("handTag"))
        {
            inside = 1;

        }
        else
        {

        }


    }

    private void OnTriggerExit(Collider other)
    {
        inside = 0;

  //      if (other.transform.CompareTag("handTag"))
		//{
		//	animtr.Play("fingerBend4");


		//	hand_in = 0; 
		//}

    }



}
