using UnityEngine;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Collections;
using System.Net.Sockets;
using System.Text;
using System.Net;
using System.Threading;

public class Coms_UDP_2_old : MonoBehaviour
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

        //cubemove = cube.GetComponent<CubeMove>();
        Debug.Log("Reached thread. ");
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
    }

    private void ThreadMethod()
    {

        Debug.Log("Reached inside . ");
        udp = new UdpClient(port);
        while (true)
        {
            Debug.Log("Reached loop. ");

            IPEndPoint RemoteIpEndPoint = new IPEndPoint(IPAddress.Any, 0);

            byte[] receiveBytes = udp.Receive(ref RemoteIpEndPoint);

            /*lock object to make sure there data is 
            *not being accessed from multiple threads at thesame time*/
            //lock (lockObject)
            //{
            returnData = Encoding.ASCII.GetString(receiveBytes);
            //Debug.Log(returnData);
            ProcessMessage();
            Debug.Log("Data received: " + returnData);
            if (returnData == "1\n")
                {
                    //Done, notify the Update function
                    precessData = true;
                }

            //}
        }
    }

    public void ProcessMessage()
    {
        string sep = ",";
        string[] splitContent = returnData.Split(sep.ToCharArray());
        //Debug.Log(splitContent[0]);
        rightData = HandData(splitContent, 1); // Right hand
        leftData = HandData(splitContent, 20); // Left hand

        //Debug.Log(rightData[14] + rightData[15] + rightData[16] + rightData[17]);
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
