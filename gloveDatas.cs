using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class gloveDatas : MonoBehaviour
{

        public float xPos;
        public float yPos;
        public float zPos;
        public float press1;
        public float press2;
        public float press3;
        public float press4;
        public float press5;
        public float flex1;
        public float flex2;
        public float flex3;
        public float flex4;
        public float flex5;
        public float xOrt;
        public float yOrt;
        public float zOrt;
        public float wOrt;

        public void AssignData(float[] allData)
        {
            xPos = allData[0];
            yPos = allData[1];
            zPos = allData[2];
            press1 = allData[3];
            press2 = allData[4];
            press3 = allData[5];
            press4 = allData[6];
            press5 = allData[7];
            flex1 = allData[8];
            flex2 = allData[9];
            flex3 = allData[10];
            flex4 = allData[11];
            flex5 = allData[12];
            xOrt = allData[13];
            yOrt = allData[14];
            zOrt = allData[15];
            wOrt = allData[16];
        }
}
