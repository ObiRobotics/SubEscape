using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GestureRecognition : MonoBehaviour
{

    public GameObject HandModel_1; // Open hand (default)
    public GameObject HandModel_2; // Desired hand gesture (currently set by user in the model manually) 

    public int handID;

    private string jointName;
    private string[] jointNames = new string[15] { "thumbprox", "thumbmid", "thumbdist", "indexprox", "indexmid", "indestdist", "midprox", "midmid", "middist", "rngprox", "rngmid", "rngdist", "litprox", "litmid", "litdist" };

    private GameObject thumbprox, thumbmid, thumbdist, indexprox, indexmid, indexdist, midprox, midmid, middist, rngprox, rngmid, rngdist, litprox, litmid, litdist;
    private GameObject[] Joints = new GameObject[15];

    private float[] hand;
    private float[] initialFlexValues;
    private float[] finalFlexValues;
    private float[] initVRJointAngles;
    private float[] finalVRJointAngles;
    private float[] jointAngleConstants = new float[15];

    private bool Calibrated;


    void Start()
    {
        // Get joint game objects from the model hand *************************************************************
        Joints = new GameObject[] { thumbprox, thumbmid, thumbdist, indexprox, indexmid, indexdist, midprox, midmid, middist, rngprox, rngmid, rngdist, litprox, litmid, litdist };

        var joints_1 = HandModel_1.GetComponentsInChildren<GameObject>();
        var joints_2 = HandModel_2.GetComponentsInChildren<GameObject>();

        // Get initial vr hand model joint angles 
        foreach (var joint in joints_1)
        {
            for (int i = 0; i < jointNames.Length; i++)
            {
                if (handID == 0)
                {
                    jointName = joint.name + "_r";
                }
                else
                {
                    jointName = joint.name + "_l";
                }

                if (jointName == jointNames[i])
                {
                    Joints[i] = joint;
                    // Get joint initial angles 
                    initVRJointAngles[i] = joint.transform.localEulerAngles.z % 360;
                }
            }
        }

        // Get final vr hand model joint angles 
        foreach (var joint in joints_2)
        {
            for (int i = 0; i < jointNames.Length; i++)
            {
                if (handID == 0)
                {
                    jointName = joint.name + "_r";
                }
                else
                {
                    jointName = joint.name + "_l";
                }

                if (jointName == jointNames[i])
                {
                    Joints[i] = joint;
                    // Get joint initial angles 
                    finalVRJointAngles[i] = joint.transform.localEulerAngles.z % 360;
                }
            }
        }


    }

    void Update()
    {
        GetFlexReadings();
        BendDigits();
    }


    private void GetFlexReadings()
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

    private void BendDigits()
    {
        //Get initial digit flex sensor values 
        if (Input.GetKey(KeyCode.M))
        {
            initialFlexValues[0] = hand[8];
            initialFlexValues[1] = hand[9];
            initialFlexValues[2] = hand[10];
            initialFlexValues[3] = hand[11];
            initialFlexValues[4] = hand[12];

        }
        //Get final digit flex sensor values 
        if (Input.GetKey(KeyCode.N))
        {
            for (int i = 0; i < 5; i++)
            {
                finalFlexValues[i] = hand[8 + i];
            }
        }
        if (Input.GetKeyDown(KeyCode.C))
        {
            int j = -1;
            for (int i = 0; i < finalVRJointAngles.Length; i++)
            {
                if (i % 3 == 0)
                {
                    j++;
                }
                jointAngleConstants[i] = (finalVRJointAngles[i] - initVRJointAngles[i]) % 360f / (finalFlexValues[j] - initialFlexValues[j]); // For every gesture we get a set of jointAngleConstants out, which we can use to recognise gestures using a more sophisticated method
            }
            Calibrated = true;
        }

        // Bend digits 
        if (Calibrated)
        {
            int k = -1;
            for (int i = 0; i < 15; i++)
            {
                if (i % 3 == 0)
                {
                    k++;
                }
                Joints[i].transform.localEulerAngles = new Vector3(Joints[i].transform.localEulerAngles.x, Joints[i].transform.localEulerAngles.y, (hand[8 + k] * (jointAngleConstants[i])) + initVRJointAngles[i]);
            }
        }
    }





}
