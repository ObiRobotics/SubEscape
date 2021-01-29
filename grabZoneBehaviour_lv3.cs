using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class grabZoneBehaviour_lv3 : MonoBehaviour
{

    public GameObject hand;
    private GameObject sphere_prefab;
    private GameObject act_sphere;

    private Animation animtr;

    private int hand_in = 0;


    void Start()
    {
        sphere_prefab = Resources.Load("Environment_Sphere_2") as GameObject;
        act_sphere = gameObject.transform.GetChild(0).gameObject;

        animtr = hand.GetComponent<Animation>();

    }

    void Update()
    {
        if (!act_sphere.activeInHierarchy)
            act_sphere = Instantiate(sphere_prefab, Vector3.zero, Quaternion.identity, this.gameObject.transform) as GameObject;

        if (hand_in == 1)
            act_sphere.GetComponent<SphereBehaviour_lv3>().handGrabbed = true;

    }

    void OnTriggerEnter(Collider other)
    {

        if (other.transform.CompareTag("handTag"))
        {
            animtr.Play("fingerBend3");
            hand_in = 1;
        }


    }

    void OnTriggerStay(Collider other)
    {

        
        if (other.transform.CompareTag("handTag"))
        {
            //animtr.Play("fingerBend4");
            //hand_in = 0;
        }


    }

    private void OnTriggerExit(Collider other)
    {
        if (other.transform.CompareTag("handTag"))
        {
            //animtr.Play("fingerBend4");
            hand_in = 0;
        }
    }



}
