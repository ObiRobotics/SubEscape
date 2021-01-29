using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerNewOnClick : MonoBehaviour
{

    public AudioSource clickSound; 
    public GameObject infoPanel; 
    public GameObject mainPanel; 

    void OnTriggerEnter(Collider otr)
    {
        clickSound.Play(); 
        PlayerNew(); 
    }


    public void PlayerNew()
    {
        infoPanel.SetActive(true); 
        mainPanel.SetActive(false);
    }
}
