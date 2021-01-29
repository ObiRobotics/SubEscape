using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{

    public float scenedelay = 7f;
    public int sceneInt = 2;

    bool nextSceneLoaded = false;

    Scene scene; 

    void Start()
    {
        StartCoroutine(StartNextScene());

        //StartCoroutine(LoadScener());
    }

    IEnumerator StartNextScene()
    {
        // Loads the next Scene (experimental scene) in the background 
        yield return new WaitForSecondsRealtime(scenedelay); 

        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneInt, LoadSceneMode.Single);

        while (!asyncLoad.isDone)
            yield return null;

        nextSceneLoaded = true;
    }

    IEnumerator LoadScener()
    {
        while(!nextSceneLoaded)
            yield return null;

        yield return new WaitForSecondsRealtime(scenedelay);
        
        SceneManager.SetActiveScene(SceneManager.GetSceneAt(sceneInt));
        //yield return new WaitForSecondsRealtime(scenedelay);
        //SceneManager.LoadScene(sceneInt);
        //SceneManager.LoadSceneAsync(sceneInt);
    }
}
