using UnityEngine;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Collections;
using System.Net.Sockets;
using System.Text;
using System.Net;
using System.Threading;

public class Coms_UDP : MonoBehaviour {

    //public Transform handTransform; 

    public gloveDatas rightHand;
    public gloveDatas leftHand; 

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
            lock (lockObject)
            {
                precessData = false;
                //cube.SendMessage("Move");
                // or
                //cubemove.Move();

                //Process received data
                Debug.Log("Received: " + returnData);

                //Reset it for next read(OPTIONAL)
                returnData = "";
            }
        }
    }

    private void OnApplicationQuit()
    {
        udp.Close(); 
    }
    
    private void ThreadMethod()
    {
        udp = new UdpClient(port);
        while (true)
        {
            IPEndPoint RemoteIpEndPoint = new IPEndPoint(IPAddress.Any, 0);

            byte[] receiveBytes = udp.Receive(ref RemoteIpEndPoint);

            /*lock object to make sure there data is 
            *not being accessed from multiple threads at thesame time*/
            lock (lockObject)
            {
                returnData = Encoding.ASCII.GetString(receiveBytes);
                //Debug.Log(returnData);
                ProcessMessage();

                //Debug.Log(returnData);

                if (returnData == "1\n")
                {
                    //Done, notify the Update function
                    precessData = true;
                }
            }
        }
    }

    public void ProcessMessage()
    {
        string sep = ",";
        string[] splitContent = returnData.Split(sep.ToCharArray());
        //Debug.Log(splitContent[0]);
        rightHand.AssignData(HandData(splitContent, 0)); // Right hand
        leftHand.AssignData(HandData(splitContent, 19)); // Left hand
        
        //Debug.Log(splitContent[12]);
        Debug.Log("right hand pos " + rightHand.xPos.ToString() + rightHand.yPos.ToString() + rightHand.zPos.ToString()) ;
        Debug.Log("left hand pos " + leftHand.xPos.ToString() + leftHand.yPos.ToString() + leftHand.zPos.ToString());
    }

    public float[] HandData(string[] splitContent, int startIndex)
    {
        float[] data = {};

        for (int i=0; i<17; i++)
        {
            data[i] = float.Parse(splitContent[startIndex + i]);
        }

        return data;
    }

}
