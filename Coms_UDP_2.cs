using UnityEngine;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Collections;
using System.Net.Sockets;
using System.Text;
using System.Net;
using System.Threading;

public class Coms_UDP_2 : MonoBehaviour
{

    //public Transform handTransform; 

    public static float[] rightData = new float[17];
    public static float[] leftData = new float[17];

    static UdpClient udp;
    Thread thread;

    public int port = 7000;

    static readonly object lockObject = new object();
    string returnData = "";
    bool precessData = false;

    void Start()
    {
        Application.runInBackground = true;

        Debug.Log("Thread starting...");
        //cubemove = cube.GetComponent<CubeMove>();
        thread = new Thread(new ThreadStart(ThreadMethod));
        thread.Start();
        //thread.IsBackground = true; 
    }

    void Update()
    {
        if (precessData)
        {
            /*lock object to make sure the data is 
             *not being accessed from multiple threads at thesame time*/
            //lock (lockObject)
            //{
                precessData = false;
                //cube.SendMessage("Move");
                // or
                //cubemove.Move();

                //Process received data
                Debug.Log("Received: " + returnData);

                //Reset it for next read(OPTIONAL)
                returnData = "";
            //}
        }
    }

    private void OnApplicationQuit()
    {
        udp.Close();
        Debug.Log("UDP channel closed correctly!");
        thread.Abort();
        Debug.Log("Thread aborted correctly!");
    }

    private void ThreadMethod()
    {
        Debug.Log("Inside thread...");

        udp = new UdpClient(port);
        while (true)
        {
            Debug.Log("Thread looping");

            try
            {
                IPEndPoint RemoteIpEndPoint = new IPEndPoint(IPAddress.Any, 0);

            byte[] receiveBytes = udp.Receive(ref RemoteIpEndPoint);
            //Debug.Log("receiveBytes");
            //Debug.Log(receiveBytes);

            /*lock object to make sure there data is 
            *not being accessed from multiple threads at thesame time*/
            //lock (lockObject)
            //{
            returnData = Encoding.ASCII.GetString(receiveBytes);
            //Debug.Log("returnData");
            //Debug.Log(returnData);

            ProcessMessage();
            //Debug.Log(returnData);
            if (returnData == "1\n")
                {
                    //Done, notify the Update function
                    precessData = true;
                }

                //}

            }
            catch (Exception e)
            {
                Debug.LogWarning(e);
            }
        }
    }

    public void ProcessMessage()
    {
        string sep = ",";
        string[] splitContent = returnData.Split(sep.ToCharArray());
        //Debug.Log(splitContent[0]);
        rightData = HandData(splitContent, 1); // Right hand
        leftData = HandData(splitContent, 20); // Left hand

        //int cntr = 0; 
        //foreach (float rData in rightData)
        //{
        //    Debug.Log(rData.ToString()+ "   " + cntr++);
        //}

        ////Debug.Log(splitContent[12]);
        //Debug.Log("right hand pos " + rightHand.xPos.ToString() + rightHand.yPos.ToString() + rightHand.zPos.ToString());
        //Debug.Log("left hand pos " + leftHand.xPos.ToString() + leftHand.yPos.ToString() + leftHand.zPos.ToString());
    }

    public float[] HandData(string[] splitContent, int startIndex)
    {
        float[] data = { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };

        for (int i = 0; i < 17; i++)
        {
            data[i] = float.Parse(splitContent[startIndex + i]);
            //Debug.Log("Data " + i.ToString() + data[i].ToString());
        }

        return data;
    }

    //public float[] HandData(string[] splitContent, int startIndex)
    //{
    //    float[] data = { };

    //    for (int i = 0; i < 17; i++)
    //    {
    //        data[i] = float.Parse(splitContent[startIndex + i]);
    //    }

    //    return data;
    //}

}
