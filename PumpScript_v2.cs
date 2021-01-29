using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PumpScript_v2 : MonoBehaviour
{
    public float PumpSpeed = 1.0f;
    public Vector3Int MyAxis;
    public int posOrneg = 1; 

    void Start()
    {
        
    }

    void Update()
    {
        float pressure = Input.GetAxis("Horizontal") * PumpSpeed * posOrneg;

        if(MyAxis.x > 0)
            transform.position =  new Vector3(transform.position.x + pressure, transform.position.y, transform.position.z); 
        if(MyAxis.y > 0)
            transform.position = new Vector3(transform.position.x, transform.position.y + pressure, transform.position.z);
        if (MyAxis.z > 0)
            transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.z + pressure);
    }
}
