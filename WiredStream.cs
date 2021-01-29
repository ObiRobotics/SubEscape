using UnityEngine;
using System.Collections;
//using System.IO.Ports;
using System.Collections.Generic;

using System;
using System.Linq;
using System.Globalization;

public class WiredStream : MonoBehaviour
{
    //public static float[] rightHand = new float[5];
    //public static float[] leftHand = new float[5];
    //public static string[] datas = new string[5];

    //float[] initVals = new float[5];
    //float[] finlVals = new float[5];

    //public int handID;
    //public string portName;
    //public string receivedstring;

    //public float minAngle = 5f;
    //public float maxAngle = 90f;

    //public bool calibrated = false;

    //SerialPort stream;

    void Start()
    {
        //stream = new SerialPort(portName, 115200);
        //float startTime = Time.realtimeSinceStartup;
        //while (!stream.IsOpen || Time.realtimeSinceStartup - startTime > 2f)
        //{
        //    Debug.Log("Trying to open port!");
        //    stream.Open(); //Open the Serial Stream.
        //}
    }
    private void OnApplicationQuit()
    {
        //stream.Close();
        //Debug.Log("Port closed correctly!");
    }

    void Update()
    {
        //receivedstring = stream.ReadLine(); //Read the information
        //stream.BaseStream.Flush(); //Clear the serial information so we assure we get new information.

        //datas = receivedstring.Split(','); //My arduino script returns a 5 part value (IE: 12,30,18,3,66)

        //if (datas[0] != "") ; //&& datas[1] != "" && datas[2] != "" && datas[3] != "" && datas[4] != "") //Check if all values are recieved
        //{
        //    //CalibrateData(datas);
        //    if (Input.GetKey(KeyCode.UpArrow))
        //    {
        //        int cntr = 0;
        //        foreach (string ds in datas)
        //        {
        //            initVals[cntr] = float.Parse(ds);
        //            cntr++;
        //        }
        //    }
        //    if (Input.GetKey(KeyCode.DownArrow))
        //    {
        //        int cntr = 0;
        //        foreach (string ds in datas)
        //        {
        //            finlVals[cntr] = float.Parse(ds);
        //            cntr++;
        //        }
        //        Debug.Log("Calibrated!!!");
        //        calibrated = true;
        //    }

        //    if (calibrated)
        //        StreamData(datas);

        //    stream.BaseStream.Flush(); //Clear the serial information so we assure we get new information.
        //}
    }

    //void CalibrateData(string[] datas)
    //{
    //    if (Input.GetKey(KeyCode.J))
    //    {
    //        Debug.Log("J key pressed");
    //        int cntr = 0;
    //        foreach (string ds in datas)
    //        {
    //            initVals[cntr] = float.Parse(ds);
    //            cntr++;
    //        }
    //    }
    //    if (Input.GetKey(KeyCode.H))
    //    {
    //        Debug.Log("H key pressed");
    //        int cntr = 0;
    //        foreach (string ds in datas)
    //        {
    //            finlVals[cntr] = float.Parse(ds);
    //            cntr++;
    //        }
    //        calibrated = true; 
    //    }

    //}

    //float Map(float ds, float initVal, float finlVal)
    //{
    //    //float mappedVal = 0f;

    //    //mappedVal = minAngle + (ds - initVal) * (maxAngle - minAngle) / (finlVal - initVal);
    //    ////low2 + (value - low1) * (high2 - low2) / (high1 - low1)

    //    //return mappedVal;
    //}

    void StreamData(string[] datas)
    {
        //if (handID == 0)
        //{
        //    int cntr = 0;
        //    foreach (string ds in datas)
        //    {
        //        rightHand[cntr] = Map(float.Parse(ds), initVals[cntr], finlVals[cntr]);
        //        //Debug.Log(rightHand[cntr]);
        //        cntr++;
        //    }
        //}
        //else if (handID == 1)
        //{
        //    int cntr = 0;
        //    foreach (string ds in datas)
        //    {
        //        leftHand[cntr] = Map(float.Parse(ds), initVals[cntr], finlVals[cntr]);
        //        //Debug.Log(leftHand[cntr]);
        //        cntr++;
        //    }
        //}
    }
}
