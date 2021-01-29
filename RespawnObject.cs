using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RespawnObject : MonoBehaviour {

    public GameObject toberespawned;
    public Vector3 origPosition = new Vector3(0.107f,1.245f,0.045f);


    private void OnCollisionEnter(Collision other)
    {
        toberespawned = other.gameObject;
        Destroy(other.gameObject, 3f);
        Invoke("SpawnObj", 2f);
    }
    
    private void SpawnObj()
    {
        Instantiate(toberespawned, origPosition, Quaternion.identity);
    }
}
