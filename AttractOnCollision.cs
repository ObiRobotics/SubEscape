using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttractOnCollision : MonoBehaviour
{
    void Start()
    {
        
    }

    void Update()
    {
        
    }

    private void OnCollisionEnter(Collision other)
    {
        transform.parent = other.transform;
        other.transform.localPosition = new Vector3(transform.position.x, transform.position.y + (other.transform.position.y/2), transform.position.z);
    }
}
