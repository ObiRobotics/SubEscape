using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement; 

public class PlayerResumeOnClick : MonoBehaviour
{

    public AudioSource clickSound; 
    public int ExperimentScene = 3; 
    public bool debugMode = false; 
    public GameObject infoPanel; 
    public GameObject mainPanel; 

    void OnTriggerEnter(Collider otr)
    {
        clickSound.Play(); 
        PlayerResume(); 
    }


    public void PlayerResume()
    {
        if (PlayerPrefs.HasKey("userID"))
        {
            if (debugMode)
            {
                PlayerPrefs.DeleteAll();
            }
            else
            {
                AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(ExperimentScene, LoadSceneMode.Single);
            }
        }
        else
        {
            // Player doesn't exist so go to the user info panel 
            // Invoke("NewPlayer",1f);
            NewPlayer(); 
        }
    }

    void NewPlayer()
    {
        infoPanel.SetActive(true); 
        mainPanel.SetActive(false);
    }
}
