using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ResetPlayerPrefs : MonoBehaviour
{

    public int ExperimentScene = 3;
    public bool debugMode = false;

    public GameObject newPlayerPanel; 
    public GameObject infoPanel; 


    // Debug version will delete the player prefs if they exist but normal version shouldn't do that but instead just continue.
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
    }

    public void PlayerNew()
    {
        // Close this panel and start new panel; 
        newPlayerPanel.SetActive(false);
        infoPanel.SetActive(true); 
    }
}