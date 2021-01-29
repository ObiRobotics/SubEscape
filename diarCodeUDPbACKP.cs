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

public class diarCodeUDPbACKP : MonoBehaviour
{

    // receiving Thread
    public Thread receiveThread;
    // udpclient object
    public UdpClient client;
    public int port;
    public string lastReceivedUDPPacket = "";
    public string allReceivedUDPPackets = "";

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
    private void init()
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
    private void ReceiveData()
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
                Debug.Log(textr);
                //datas = float.Parse(textr);
            }
            catch (Exception err)
            {
                print(err.ToString());
            }
        }
    }


}
