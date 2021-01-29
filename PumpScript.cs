using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PumpScript : MonoBehaviour
{

    [Header("0 for Lever or 1 for Sphere")]
    public int pumpType = 1;

    public float ForceMultiplier = 100000f;
    public GameObject FingerObj;

    public static float pumping; 

    Rigidbody rb; 

    void Start()
    {
        rb = GetComponent<Rigidbody>();    
    }

    
    void FixedUpdate()
    {
        if (pumpType == 0)
        {
            pumping = Input.GetAxis("Vertical") * ForceMultiplier * Time.deltaTime;
            rb.AddForce(transform.forward * pumping);
        }
        else if (pumpType == 1)
        {
            pumping = Input.GetAxis("Vertical") * ForceMultiplier * Time.deltaTime;
            //Debug.Log(pumping); 
            transform.localScale = new Vector3(transform.localScale.x + pumping, transform.localScale.y + pumping, transform.localScale.z + pumping); 

            if(transform.localScale.x >= 0.15f)
                transform.localScale = new Vector3(0.15f, 0.15f, 0.15f); 
            if(transform.localScale.x <= 0.075f)
                transform.localScale = new Vector3(0.075f, 0.075f, 0.075f);
        }
        else if (pumpType == 2)
        {
            pumping = (FingerObj.transform.localRotation.eulerAngles.z +90f) % 360f; //Input.GetAxis("Vertical") * ForceMultiplier * Time.deltaTime;
            pumping = Mathf.Clamp(pumping, 25f, 95f);
            pumping = pumping / 95f;

            if (transform.localScale.x >= 0.15f)
                transform.localScale = new Vector3(0.15f, 0.15f, 0.15f);
            if (transform.localScale.x <= 0.075f)
                transform.localScale = new Vector3(0.075f, 0.075f, 0.075f);

            transform.localScale = new Vector3(transform.localScale.x * (pumping * -ForceMultiplier), 
                                               transform.localScale.y * (pumping * -ForceMultiplier), 
                                               transform.localScale.z * (pumping * -ForceMultiplier));

            //Debug.Log(pumping * -ForceMultiplier);

        }
    }
}
