using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; 
using TMPro; 

public class PlayerSaveData : MonoBehaviour
{
    public TMP_Text welcomeText;    

    void Start()
    {
        bool playerExists = PlayerPrefs.HasKey("UserID");
        
        if(playerExists){
            welcomeText.text = "Welcome back " + PlayerPrefs.GetString("UserID");
        }else
        {
            welcomeText.text = "Create new player"; 
        }
        
    }

}
