/* Obi Robotics Ltd (see obirobotics.co.uk for more details) 

Author: Diar Karim
Date: 14/05/2018
Version: 1.0
Contact: diarkarim@gmail.com
License: obirobotics.co.uk for license

*/

using System;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using System.Collections;
//using System.IO.Ports;
//using UnityEngine.UI;
//using System.Net.Sockets;
//using System.Text;
//using System.Net;
//using System.Threading;

public class FingerRotate : MonoBehaviour
{
    public int handID;
    private int cnter = 0;

    private float InitTmb1, InitTmb2, InitIdx1, InitIdx2, InitIdx3, InitMid1, InitMid2, InitMid3, InitRng1, InitRng2, InitRng3, InitPnk1, InitPnk2, InitPnk3;

    private float[] hand;
    float[] digitAngles = new float[5];

    //public Comms_UDP CommsObject; 
    //public gloveDatas hand;

    public float bendConstant = -50f;

    // Declare variables
    //private GameObject[] digits = new GameObject[15]; 
    private GameObject thumbprox, thumbmid, thumbdist, indexprox, indexmid, indexdist, midprox, midmid, middist, rngprox, rngmid, rngdist, litprox, litmid, litdist;

    // Initial finger rotations
    private float init_litproxY, init_litproxZ;

    private bool grasping, grasped = false;
    private bool capCurrent = false;

    private Collider graspedObject;

    private Vector3 prevPosition;

    [HideInInspector]
    public static float thumAng, indxAng, midAng, rngAng, litAng;
    private float[] initDigitAngles = new float[5]; //, initindxAng, initmidAng, initrngAng, initlitAng;

    private Vector3 handScaleY, handCityPos, handPosRec, currPosRec, currScaleRec, handVel, prevHandVel;
    private Vector3 handScaleRec = new Vector3(0, 0, 0);

    Quaternion prevHandRotation, handRotation;

    public bool city_on = false;

    private Vector3 thumbConstants;
    private Vector3 fingConstants;

    private int kay = 0;

    public float bendings; 
    void Start()
    {
        // In the future replace the below codes with for loops with arrays of game objects etc 
        //foreach (GameObject digi in digits)
        //{
        //    if (gameObject.name == "LeftHand")
        //    {

        //    }
        //    else if (gameObject.name == "RightHand")
        //    {

        //    }
        //}

        // Get digit gameobjects
        if (gameObject.name == "LeftHand")
        {
            thumbprox = GameObject.FindGameObjectWithTag("thumbprox_l");
            thumbmid = GameObject.FindGameObjectWithTag("thumbmid_l");
            thumbdist = GameObject.FindGameObjectWithTag("thumbdist_l");
            indexprox = GameObject.FindGameObjectWithTag("indexprox_l");
            indexmid = GameObject.FindGameObjectWithTag("indexmid_l");
            indexdist = GameObject.FindGameObjectWithTag("indexdist_l");
            midprox = GameObject.FindGameObjectWithTag("midprox_l");
            midmid = GameObject.FindGameObjectWithTag("midmid_l");
            middist = GameObject.FindGameObjectWithTag("middist_l");
            rngprox = GameObject.FindGameObjectWithTag("rngprox_l");
            rngmid = GameObject.FindGameObjectWithTag("rngmid_l");
            rngdist = GameObject.FindGameObjectWithTag("rngdist_l");
            litprox = GameObject.FindGameObjectWithTag("litprox_l");
            litmid = GameObject.FindGameObjectWithTag("litmid_l");
            litdist = GameObject.FindGameObjectWithTag("litdist_l");
        }
        else if (gameObject.name == "RightHand")
        {
            thumbprox = GameObject.FindGameObjectWithTag("thumbprox_r");
            thumbmid = GameObject.FindGameObjectWithTag("thumbmid_r");
            thumbdist = GameObject.FindGameObjectWithTag("thumbdist_r");
            indexprox = GameObject.FindGameObjectWithTag("indexprox_r");
            indexmid = GameObject.FindGameObjectWithTag("indexmid_r");
            indexdist = GameObject.FindGameObjectWithTag("indexdist_r");
            midprox = GameObject.FindGameObjectWithTag("midprox_r");
            midmid = GameObject.FindGameObjectWithTag("midmid_r");
            middist = GameObject.FindGameObjectWithTag("middist_r");
            rngprox = GameObject.FindGameObjectWithTag("rngprox_r"); 
            rngmid = GameObject.FindGameObjectWithTag("rngmid_r"); 
            rngdist = GameObject.FindGameObjectWithTag("rngdist_r");
            litprox = GameObject.FindGameObjectWithTag("litprox_r");
            litmid = GameObject.FindGameObjectWithTag("litmid_r");
            litdist = GameObject.FindGameObjectWithTag("litdist_r");
        }

        InitTmb1 = thumbmid.transform.localEulerAngles.z;
        InitTmb2 = thumbdist.transform.localEulerAngles.z;
        InitIdx1 = indexprox.transform.localEulerAngles.z;
        InitIdx2 = indexmid.transform.localEulerAngles.z;
        InitIdx3 = indexdist.transform.localEulerAngles.z;
        InitMid1 = midprox.transform.localEulerAngles.z;
        InitMid2 = midmid.transform.localEulerAngles.z;
        InitMid3 = middist.transform.localEulerAngles.z;
        InitRng1 = rngprox.transform.localEulerAngles.z;
        InitRng2 = rngmid.transform.localEulerAngles.z;
        InitRng3 = rngdist.transform.localEulerAngles.z;
        InitPnk1 = litprox.transform.localEulerAngles.z;
        InitPnk2 = litmid.transform.localEulerAngles.z;
        InitPnk3 = litdist.transform.localEulerAngles.z;

        // Arbitrary constants to affect amount of finger bending  
        float thumbConstant1 = 0.5f;
        float thumbConstant2 = 1.0f;
        float thumbConstant3 = 0.5f;
        thumbConstants = new Vector3(thumbConstant1, thumbConstant2, thumbConstant3);
        float fingConstant1 = 0.75f;
        float fingConstant2 = 1.0f;
        float fingConstant3 = 0.5f;
        fingConstants = new Vector3(fingConstant1, fingConstant2, fingConstant3);

    }

    void Update()
    {

        if (handID == 0)
        {
            hand = Coms_UDP_2.rightData;
        }
        if (handID == 1)
        {
            hand = Coms_UDP_2.leftData;
        }

        // Compute hand velocity
        handVel = (transform.position - prevHandVel) / Time.deltaTime;
        prevHandVel = transform.position;

        handRotation = transform.rotation;
        prevHandRotation = transform.rotation;

        BendDigits();

        //if (city_on)
        //{
        //    CityScale();
        //}

        if (grasping)
        {
            GraspObject(graspedObject);
            grasping = false;
        }
        if (!grasping && grasped)
        {
            ReleaseObject(graspedObject);
        }

        //prevPosition = transform.position;
        //if (cnter > 0)
        //{
        //    HandVelocity();
        //}
        //cnter++;

        //Debug.Log("Ring finger bend: " + hand[11].ToString());

        
    }



    Vector3 GetAngularVelocity()
    {
        Quaternion deltaRotation = handRotation * Quaternion.Inverse(prevHandRotation);
        return new Vector3(Mathf.DeltaAngle(0, deltaRotation.eulerAngles.x), Mathf.DeltaAngle(0, deltaRotation.eulerAngles.y), Mathf.DeltaAngle(0, deltaRotation.eulerAngles.z));
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag == "grabTag")
        {
            graspedObject = other;
            grasping = true;
            grasped = true;
        }
    }
    //private void OnTriggerExit(Collider other)
    //{
    //    grasping = false;
    //}

    public void BendDigits()
    {
        //Get initial digit bend offset
        if (kay < 100)
        {
            initDigitAngles[0] = hand[8];
            initDigitAngles[1] = hand[9];
            initDigitAngles[2] = hand[10];
            initDigitAngles[3] = hand[11];
            initDigitAngles[4] = hand[12];

            kay++;
        }

        // Assign digit bend angles to variables
        for (int i = 0; i<5; i++)
        {
            digitAngles[i] = (hand[8+i] - initDigitAngles[i]) * bendConstant; 
        }

        // Apply digit angles to digits 
        bendThumb(thumbprox, thumbmid, thumbdist, digitAngles[0], thumbConstants, InitTmb1, InitTmb2);
        bendFinger(indexprox, indexmid, indexdist, digitAngles[1], fingConstants, InitIdx1, InitIdx2, InitIdx3);
        bendFinger(midprox, midmid, middist, digitAngles[2], fingConstants, InitMid1, InitMid2, InitMid3);
        bendFinger(rngprox, rngmid, rngdist, bendings, fingConstants, InitRng1, InitRng2, InitRng3);
        bendFinger(litprox, litmid, litdist, digitAngles[4], fingConstants, InitPnk1, InitPnk2, InitPnk3);

        Debug.Log("Ring Finger: " + digitAngles[3].ToString());
    }

    public void bendThumb(GameObject digit_1, GameObject digit_2, GameObject digit_3, float bend_angle, Vector3 bend_constants, float bendOffset1, float bendOffset2)
    {
        //float bendAngle = float.Parse(bend_angle);
        //digit_1.transform.localEulerAngles = new Vector3(bend_constant_1 * bend_angle, digit_1.transform.localEulerAngles.y, digit_1.transform.localEulerAngles.z); 
        digit_2.transform.localEulerAngles = new Vector3(digit_2.transform.localEulerAngles.x, digit_2.transform.localEulerAngles.y, (bend_constants.y * bend_angle) + bendOffset1);
        digit_3.transform.localEulerAngles = new Vector3(digit_3.transform.localEulerAngles.x, digit_3.transform.localEulerAngles.y, (bend_constants.z * bend_angle) + bendOffset2);
    }

    public void bendFinger(GameObject digit_1, GameObject digit_2, GameObject digit_3, float bend_angle, Vector3 bend_constants, float bendOffset1, float bendOffset2, float bendOffset3)
    {
        //float bendAngle = float.Parse(bend_angle);
        digit_1.transform.localEulerAngles = new Vector3(digit_1.transform.localEulerAngles.x, digit_1.transform.localEulerAngles.y, (bend_constants.x * bend_angle) + bendOffset1);
        digit_2.transform.localEulerAngles = new Vector3(digit_2.transform.localEulerAngles.x, digit_2.transform.localEulerAngles.y, (bend_constants.z * bend_angle) + bendOffset2);
        digit_3.transform.localEulerAngles = new Vector3(digit_3.transform.localEulerAngles.x, digit_3.transform.localEulerAngles.y, (bend_constants.y * bend_angle) + bendOffset3);
    }

    public void GraspObject(Collider other)
    {
        if (digitAngles[1] < -30f)
        {

            other.transform.parent = transform;
            if (other.attachedRigidbody)
            {
                other.attachedRigidbody.useGravity = false;
                other.attachedRigidbody.isKinematic = true;
            }
        }
    }
    public void ReleaseObject(Collider other)
    {
        if (digitAngles[1] > -30f)
        {
            Collider[] ts = GetComponentsInChildren<Collider>();
            foreach (Collider t in ts)
            {
                if (t.gameObject.tag == "grabTag")
                {
                    Rigidbody rb = GetComponent<Rigidbody>();
                    //Vector3 handVel = transform.InverseTransformDirection(rb.velocity);

                    t.transform.parent = null;
                    t.attachedRigidbody.isKinematic = false;
                    t.attachedRigidbody.velocity = handVel * 1.5f;
                    t.attachedRigidbody.angularVelocity = GetAngularVelocity();
                    t.attachedRigidbody.useGravity = true;
                    //Invoke("GravityFunction(other)", 1f);
                }
            }
            grasped = false;
        }
    }

    public void GravityFunction(Collider other)
    {
        //other.attachedRigidbody.useGravity = true;
        other.attachedRigidbody.isKinematic = false;
    }

    //public void HandVelocity()
    //{
    //    handVel = transform.position-prevPosition/Time.deltaTime;
    //    Debug.Log(handVel);
    //    cnter = 0;
    //}

    //void CityScale()
    //{
    //    if (Boundary.cityScape != null)
    //    {

    //        if (midAng > 55f)
    //        {

    //            //if (indxAng > 55)
    //            //{
    //            //Vector3 handScaleY = new Vector3(transform.position.y, transform.position.y, transform.position.y);
    //            //Vector3 handCityPos = new Vector3(transform.position.y, transform.position.y, transform.position.y);
    //            //capCurrent = false;

    //            Boundary.cityScape.transform.localScale = handScaleRec + new Vector3(transform.position.y, transform.position.y, transform.position.y);
    //            //+ handScaleRec;
    //            Boundary.cityScape.transform.localPosition = new Vector3(transform.position.x, Boundary.cityScape.transform.localPosition.y, transform.position.z) - handPosRec;
    //            //}
    //            currPosRec = new Vector3(Boundary.cityScape.transform.localPosition.x, Boundary.cityScape.transform.localPosition.y, Boundary.cityScape.transform.localPosition.z);
    //            currScaleRec = new Vector3(Boundary.cityScape.transform.localScale.x, Boundary.cityScape.transform.localScale.y, Boundary.cityScape.transform.localScale.z);
    //        }
    //        else
    //        {
    //            handPosRec = new Vector3(transform.position.x, Boundary.cityScape.transform.localPosition.y, transform.position.z) - currPosRec;
    //            handScaleRec = currScaleRec - new Vector3(transform.position.y, transform.position.y, transform.position.y);
    //            //Vector3 posDiff = new Vector3(transform.position.x - handPosRec.x, Boundary.cityScape.transform.localPosition.y - handPosRec.y, transform.position.z - handPosRec.z);
    //        }
    //        //if (!capCurrent)
    //        //{
    //        //    Boundary.cityScape.transform.localScale = new Vector3(transform.position.y, transform.position.y, transform.position.y) - handScaleY;
    //        //    Boundary.cityScape.transform.localPosition = new Vector3(transform.position.x, Boundary.cityScape.transform.localPosition.y, transform.position.z) - handCityPos;
    //        //    if (indxAng > 55 && !capCurrent)
    //        //    {
    //        //        capCurrent = true;
    //        //    }
    //        //}

    //    }
    //}

}
