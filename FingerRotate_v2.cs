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

public class FingerRotate_v2 : MonoBehaviour
{
    public int handID;
    public bool wireless; 

    private int cnter = 0;

    private float InitTmb3, InitTmb1, InitTmb2, InitIdx1, InitIdx2, InitIdx3, InitMid1, InitMid2, InitMid3, InitRng1, InitRng2, InitRng3, InitPnk1, InitPnk2, InitPnk3;

    private float[] hand;
    public static float[] digitAngles = new float[5];

    //public Comms_UDP CommsObject; 
    //public gloveDatas hand;

    // Declare variables
    //private GameObject[] Joints = new GameObject[15]; 
    private GameObject thumbprox, thumbmid, thumbdist, indexprox, indexmid, indexdist, midprox, midmid, middist, rngprox, rngmid, rngdist, litprox, litmid, litdist;
    private GameObject[] Joints = new GameObject[15];

    // Initial finger rotations
    private float init_litproxY, init_litproxZ;

    private bool grasping, grasped = false;
    private bool capCurrent = false;

    private Collider graspedObject;

    private Vector3 prevPosition;

    [HideInInspector]
    public static float thumAng, indxAng, midAng, rngAng, litAng;
    private float[] initDigitAngles; //, initindxAng, initmidAng, initrngAng, initlitAng;

    private Vector3 handScaleY, handCityPos, handPosRec, currPosRec, currScaleRec, handVel, prevHandVel;
    private Vector3 handScaleRec = new Vector3(0, 0, 0);

    Quaternion prevHandRotation, handRotation;

    private Vector3 thumbConstants;
    private Vector3 fingConstants;

    private int kay = 0;

    private float[] finalDigitAngles = new float[5];  // PIP     MIP       DIP
    private float[] finalVRHandDigits = new float[15] {-70f%360,-15f%360, -75f%360,      // thumb joints
                                                       -70f%360, -90f%360, -90f%360,     // index joints
                                                       -70f%360, -107f%360, -85f%360,    // mid joints
                                                       -70f%360, -90f%360, -90f%360,     // ring joints
                                                       -70f%360, -100f%360, -90f%360 };  // pinky joints
    private float[] initVRHandDigits = new float[15];
    private float[] digitConstant = new float[15];

    private bool Calibrated = false;
    private float[] bendAngles; 

    void Start()
    {
        initDigitAngles = new float[5];
        hand = new float[5];
        bendAngles = new float[5];

        // Get digit gameobjects
        //if (gameObject.name == "LeftHand")
        if(handID == 1)
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
        //else if (gameObject.name == "RightHand")
        else if (handID == 0)
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

        InitTmb3 = thumbprox.transform.localEulerAngles.z;
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

        initVRHandDigits = new float[]{ InitTmb3%360, InitTmb1%360, InitTmb2%360, InitIdx1%360, InitIdx2%360, InitIdx3%360, InitMid1%360, InitMid2%360,
                                        InitMid3%360, InitRng1%360,InitRng2%360,InitRng3%360,InitPnk1%360,InitPnk2%360,InitPnk3%360};
        Joints = new GameObject[] { thumbprox, thumbmid, thumbdist, indexprox, indexmid, indexdist, midprox, midmid, middist, rngprox,rngmid,rngdist, litprox, litmid, litdist};

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
        if (wireless)
        {
            if (handID == 0)
            {
                hand = Coms_UDP_2.rightData;
            }
            if (handID == 1)
            {
                hand = Coms_UDP_2.leftData;
            }
        }
        else
        {
            if (handID == 0)
            {
                //hand = WiredStream.rightHand;
            }
            if (handID == 1)
            {
                //hand = WiredStream.leftHand;
            }
        }

            // Compute hand velocity
        handVel = (transform.position - prevHandVel) / Time.deltaTime;
        prevHandVel = transform.position;

        handRotation = transform.rotation;
        prevHandRotation = transform.rotation;

        BendDigits();

        if (grasping)
        {
            GraspObject(graspedObject);
            grasping = false;
        }
        if (!grasping && grasped)
        {
            ReleaseObject(graspedObject);
        }
    }

    public void BendDigits()
    {
        //Get initial digit bend offset
        if (Input.GetKey(KeyCode.M))
        {
            initDigitAngles[0] = hand[0];
            initDigitAngles[1] = hand[1];
            initDigitAngles[2] = hand[2];
            initDigitAngles[3] = hand[3];
            initDigitAngles[4] = hand[4];

        }
        if (Input.GetKey(KeyCode.N)) 
        {
            for (int i = 0; i < 5; i++)
            {
                finalDigitAngles[i] = hand[i]; 
            }
        }
        if (Input.GetKeyDown(KeyCode.C))
        {
            int j = -1;
            for(int i = 0; i<finalVRHandDigits.Length; i++)
            {
                if (i % 3 == 0)
                {
                    j++;
                }
                digitConstant[i] = (finalVRHandDigits[i] - initVRHandDigits[i])%360f/(finalDigitAngles[j] - initDigitAngles[j]);
            }
            Calibrated = true;
        }

        if (Calibrated)
        {
            int k = -1;
            for (int i = 0; i < 15; i++)
            {
                if (i % 3 == 0)
                {
                    k++;
                }

                float xrot = Mathf.Round(Joints[i].transform.localEulerAngles.x);
                float yrot = Mathf.Round(Joints[i].transform.localEulerAngles.y);
                float zrot = Mathf.Round((hand[k] * (digitConstant[i])) + initVRHandDigits[i]);

                Quaternion jointQuat = Quaternion.Euler(xrot, yrot, zrot);
                //jointQuat.Normalize();

                float norm = Mathf.Sqrt(jointQuat.x * jointQuat.x
                                        + jointQuat.y * jointQuat.y
                                        + jointQuat.z * jointQuat.z
                                        + jointQuat.w * jointQuat.w);

                if (Mathf.Abs(norm - 1.0f) <= (1e-2 + 1e-4))
                {
                    Joints[i].transform.localEulerAngles = new Vector3(xrot, yrot, zrot);
                    //Joints[i].transform.localRotation = jointQuat; 
                }

            }
        }
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
    Vector3 GetAngularVelocity()
    {
        Quaternion deltaRotation = handRotation * Quaternion.Inverse(prevHandRotation);
        return new Vector3(Mathf.DeltaAngle(0, deltaRotation.eulerAngles.x), Mathf.DeltaAngle(0, deltaRotation.eulerAngles.y), Mathf.DeltaAngle(0, deltaRotation.eulerAngles.z));
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
                    t.attachedRigidbody.useGravity = true;
                    t.attachedRigidbody.velocity = handVel * 1.5f;
                    t.attachedRigidbody.angularVelocity = GetAngularVelocity();
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

}

//float xrot = Joints[i].transform.localEulerAngles.x;
//float yrot = Joints[i].transform.localEulerAngles.y;
//float zrot = (hand[8 + k] * (digitConstant[i])) + initVRHandDigits[i];
//Quaternion jointQuat = Quaternion.Euler(xrot, yrot, zrot);

//float norm = Mathf.Sqrt(jointQuat.x * jointQuat.x
//                        + jointQuat.y * jointQuat.y
//                        + jointQuat.z * jointQuat.z
//                        + jointQuat.w * jointQuat.w);

//if (Mathf.Abs(norm - 1.0f) <= (1e-2 + 1e-4))
//{
//    Joints[i].transform.rotation = jointQuat;
//}

