using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using System;

public class SaveAndLoad : MonoBehaviour
{

    public TMP_Text buttonText;
    public int Scene2Load;
    public int AlternativeScene;
    bool startGame; 

    void Update()
    {
        if (KeyboardScript.doneWithForm == 1 & startGame)
        {
            SceneManager.LoadSceneAsync(Scene2Load, LoadSceneMode.Single);
            startGame = false; 
        }
        else if(KeyboardScript.doneWithForm == 2 & startGame)
        {
            SceneManager.LoadSceneAsync(AlternativeScene, LoadSceneMode.Single);
            startGame = false; 
        }
        else if(startGame)
        {
            buttonText.text = "Please answer all the questions first";
            Invoke("ClearText",5f);
            startGame = false;
        }
    }

    private void ClearText()
    {
        buttonText.text = "";
    }

    void OnTriggerEnter(Collider otr)
    {
        startGame = true;
    }
}


