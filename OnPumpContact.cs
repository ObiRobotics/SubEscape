using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnPumpContact : MonoBehaviour
{
    public GameObject RightHandPump;
    public GameObject LeftHandPump;

    bool unassigned = false;

    bool rightHand = false; 

    void Start()
    {
        if (RightHandPump.activeInHierarchy | LeftHandPump.activeInHierarchy)
        {
            RightHandPump.SetActive(false);
            LeftHandPump.SetActive(false);
        }

        string hand = PlayerPrefs.GetString("hand");
        if (hand.Contains("R"))
        {
            rightHand = true;
        }
        else if (hand.Contains("L"))
        {
            rightHand = false; 
        }
        else
        {
            rightHand = false;
        }

    }

    void OnTriggerEnter(Collider other)
    {
        if(!rightHand)
        {
            try
            {
                RightHandPump.SetActive(true);
            }
            catch (UnassignedReferenceException e)
            {
                unassigned = true;
            }
        }

        if(rightHand)
        {
            try
            {
                LeftHandPump.SetActive(true);
            }
            catch (UnassignedReferenceException e)
            {
                unassigned = true; 
            }
        }

        // Self deactivate 
        Collider col = gameObject.GetComponent<Collider>();
        col.enabled = false; 

    }

    void SelfDestructDelay()
    {
      transform.parent.gameObject.SetActive(false);
    }
}
