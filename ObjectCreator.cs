using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectCreator : MonoBehaviour
{

    GameObject[] baseObjects;// = new GameObject[26];
    GameObject[] targetObjects; 

    void Start()
    {

        baseObjects = GetComponentsInChildren<GameObject>();

        int i = 0; 
        foreach(GameObject baseObj in baseObjects)
        {
            if(baseObj.name.Contains("row"))
            {
                continue;
            }
            else
            {
                targetObjects[i] = GameObject.CreatePrimitive(PrimitiveType.Cube);
                i++;

                // Configure target objects to correspond to base object transforms
                targetObjects[i].transform.position = baseObj.transform.position;
                targetObjects[i].transform.rotation = baseObj.transform.rotation;
                targetObjects[i].transform.localScale = baseObj.transform.localScale;
            }
        }
    }

    void Update()
    {
        
    }
}
