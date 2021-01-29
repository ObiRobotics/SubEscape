using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SphereBehaviour_lv3 : MonoBehaviour {

    public GameObject hand;
    internal bool handGrabbed, sphereShot;
    private Vector3 previousPosition, currentPosition, velocity;

    // Use this for initialization
    void Start () {
        handGrabbed = false;
        sphereShot = false;
        previousPosition = transform.position;
    }

    // Update is called once per frame
    void Update () {
        currentPosition = transform.position;
        velocity = (currentPosition - previousPosition) / Time.deltaTime;
        //Debug.Log(velocity.magnitude);

        if (handGrabbed && !sphereShot)
        {
            gameObject.transform.position = Vector3.Lerp(new Vector3(hand.transform.position.x - 0.0027f, hand.transform.position.y - 0.05f, hand.transform.position.z + 0.05f), gameObject.transform.position, Time.deltaTime * 50f);
            
        }

        if (velocity.magnitude >= 8 && !sphereShot)
        {
            sphereShot = true;
            Rigidbody rigidBody = gameObject.AddComponent<Rigidbody>();
            rigidBody.mass = 1;
            rigidBody.velocity = 8*velocity.normalized;
        }
        if (sphereShot)
        {
            Debug.Log(gameObject.GetComponent<Rigidbody>().velocity.magnitude);

            Destroy(this.gameObject, 5.0f);
        }


        previousPosition = currentPosition;
    }

    private void OnDestroy()
    {
        Instantiate(this.gameObject, Vector3.zero, Quaternion.identity, this.gameObject.transform.parent);
    }
}
