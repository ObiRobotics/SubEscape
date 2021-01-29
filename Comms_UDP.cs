using UnityEngine;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Collections;
using System.Net.Sockets;
using System.Text;
using System.Net;
using System.Threading;
//using System.IO.Ports;
//using UnityEngine.UI;

public class Comms_UDP : MonoBehaviour {

    //public static float xPos;
    //public static float yPos;
    //public static float zPos;
    //public static float press1;
    //public static float press2;
    //public static float press3;
    //public static float press4;
    //public static float press5;
    //public static float flex1;
    //public static float flex2;
    //public static float flex3;
    //public static float flex4;
    //public static float flex5;
    //public static float xOrt;
    //public static float yOrt;
    //public static float zOrt;
    //public static float wOrt;

    public static float[] rightData;
    public static float[] leftData;

    // receiving Thread
    public Thread receiveThread;
    // udpclient object
    public UdpClient client;
    public int port;
    public string lastReceivedUDPPacket = "";
    public string allReceivedUDPPackets = "";

    //public gloveDatas rightHand;
    //public gloveDatas leftHand;

    public static float datas = 0.0f; 

    // start from shell
    private static void Main()
    {
        Comms_UDP receiveObj = new Comms_UDP();
        receiveObj.init();

        string text = "";
        do
        {
            text = Console.ReadLine();
        } while (!text.Equals("exit"));
    }
    // start from unity3d
    public void Start()
    {
        Application.runInBackground = true; 
        init();
    }

    void Update()
    {

    }

    // init
    public void init()
    {
        // Endpunkt definieren, von dem die Nachrichten gesendet werden.
        print("UDPSend.init()");

        // define port
        port = 7000;

        // status
        //print("Sending to 127.0.0.1 : " + port);
        //print("Test-Sending to this Port: nc -u 127.0.0.1  " + port + "");

        // ----------------------------
        // Abhören
        // ----------------------------
        // Lokalen Endpunkt definieren (wo Nachrichten empfangen werden).
        // Einen neuen Thread für den Empfang eingehender Nachrichten erstellen.
        receiveThread = new Thread(
            new ThreadStart(ReceiveData));
        receiveThread.IsBackground = true;
        receiveThread.Start();

    }

    // receive thread
    public void ReceiveData()
    {
        client = new UdpClient(port);
        IPEndPoint anyIPR = new IPEndPoint(IPAddress.Any, 0);

        while (true)
        {

            try
            {
                // Bytes empfangen.
                byte[] dataR = client.Receive(ref anyIPR);

                //string textr = Encoding.UTF8.GetString(dataR);
                string textr = Encoding.ASCII.GetString(dataR);

                ProcessMessage(textr);

                //Debug.Log(textr);
                //datas = float.Parse(textr);
            }
            catch (Exception err)
            {
                print(err.ToString());
            }
        }
    }

    public void ProcessMessage(string dataR)
    {
        string sep = ",";
        string[] splitContent = dataR.Split(sep.ToCharArray());
        rightData = HandData(splitContent, 1);
        leftData = HandData(splitContent, 20);

        //rightHand.AssignData(rightData); // Right hand
        //leftHand.AssignData(leftData); // Left hand
    }

    public float[] HandData(string[] splitContent, int startIndex)
    {
        float[] data = {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0};

        for (int i = 0; i < 17; i++)
        {
            data[i] = float.Parse(splitContent[startIndex + i]);
            //Debug.Log("index " + i.ToString() + data[i].ToString());
        }

        return data;
    }
}
