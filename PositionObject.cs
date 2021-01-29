using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PositionObject : MonoBehaviour
{
    public Transform[] hands;
    public Vector3 offsetPos; 

    void Update()
    {
        //if() // Attach sphere to the correct hand
        transform.position = hands[0].position + offsetPos;
    }
}
