using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RocketFunction : MonoBehaviour {

    private Rigidbody rb;
    public bool active;
    public float thrust = 1.0f; 

    void Start () {
        rb = GetComponent<Rigidbody>(); 
	}
	
	void Update () {

        if (active)
        {
            ActivateRocket();
            Invoke("DeactivateRocket", 1.0f);
        }
        
	}

    private void ActivateRocket()
    {
        rb.AddRelativeForce(Vector3.up * thrust,ForceMode.Impulse); 
    }
    private void DeactivateRocket()
    {
        //rb.AddRelativeForce(Vector3.up * 0.0f);
        active = false; 
    }

    private void OnTriggerEnter(Collider other)
    {
        active = true;
    }
}
