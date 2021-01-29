using UnityEngine;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Collections;
using System.Net.Sockets;
using System.Text;
using System.Net;
using System.Threading;

class UDP
{

    private static UdpClient udp;
    private Thread thread;

    private static readonly object lockObject = new object();
    private string returnData = "";
    private bool precessData = false;

    private IPEndPoint RemoteIpEndPoint; 

    public int port = 7000;

    public static float[] rightData, leftData, rlData;

    public UDP()
    {
        Init_UDP();
    }

    public void Init_UDP()
    {
        udp = new UdpClient(port);
    }

    public void SendUDP_Message(string msg)
    {
        //send_buffer_1 = Encoding.ASCII.GetBytes(msg);
        //sock.SendTo(send_buffer_1, endPoint);
        //Debug.Log("*** UDP Class sends message: " + msg + " \n");
    }

    public float[] ReceiveUDP_Message()
    {
        RemoteIpEndPoint = new IPEndPoint(IPAddress.Any, 0); // replace 0 with port number
        byte[] receiveBytes = udp.Receive(ref RemoteIpEndPoint);
        returnData = Encoding.ASCII.GetString(receiveBytes);
        return rlData = ProcessMessage(); 
    }

    public float[] ProcessMessage()
    {
        string sep = ",";
        string[] splitContent = returnData.Split(sep.ToCharArray());
        //Debug.Log(splitContent[0]);
        rightData = HandData(splitContent, 1); // Right hand
        leftData = HandData(splitContent, 20); // Left hand

        // Create one large array to contain all data
        float[] newArray = new float[rightData.Length + leftData.Length];
        Array.Copy(rightData, newArray, rightData.Length);
        Array.Copy(leftData, 0, newArray, rightData.Length, leftData.Length);

        return newArray; 

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
            Debug.Log("index " + i.ToString() + data[i].ToString());
        }

        return data;
    }

}

public class Coms_UDP_3 : MonoBehaviour
{

    private UDP _udp; 

    void Start()
    {
        _udp.port = 7000;
        //Application.runInBackground = true;

        ////cubemove = cube.GetComponent<CubeMove>();
        //thread = new Thread(new ThreadStart(ThreadMethod));
        //thread.Start();
        ////thread.IsBackground = true; 
    }

    private void Update()
    {
        float[] arelDat = _udp.ReceiveUDP_Message();
        Debug.Log("All data: " + arelDat[4].ToString() + "\n");
    }


}
