using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightIndicatorScript : MonoBehaviour
{
    public GameObject IndicatorLight;

    void Start()
    {
        StartCoroutine(IndicatorSwitcher()); 
    }

    IEnumerator IndicatorSwitcher()
    {
        for(int i = 0; i<10; i++)
        {
            IndicatorLight.SetActive(true);
            yield return new WaitForSecondsRealtime(0.75f);
            IndicatorLight.SetActive(false);
            yield return new WaitForSecondsRealtime(0.75f);
        }

        yield return null; 
    }
}
