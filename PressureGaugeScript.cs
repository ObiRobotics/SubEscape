using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PressureGaugeScript : MonoBehaviour
{
    //bool reachedTargetRot;
    bool newTargetRot = true; 
    float randRot;
    float currentRot;
    public float startRot;
    public float endRot; 

    void Update()
    {
        if (newTargetRot)
        {
            //currentRot = transform.localRotation.eulerAngles.y;
            //randRot = Random.Range(-25f, 200f);
            StartCoroutine(Rotator()); 
            newTargetRot = false;
        }
    }

    IEnumerator Rotator()
    {
        startRot = startRot - 1f;
        transform.localRotation = Quaternion.Euler(new Vector3(0f, startRot, 0f));
        yield return new WaitForSeconds(0.005f);
        //yield return new WaitUntil(RotateFunc);
        yield return null;
        newTargetRot = true;
    }

    bool RotateFunc()
    {
        //float angularDiff = Mathf.Sqrt( (currentRot * currentRot) - (randRot * randRot));
        
        for (int i = 0; i < 100; i++)
        {
            transform.localRotation = Quaternion.Euler(new Vector3(0f, i, 0f));
        }

        return true; 
    }
}
