using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Obi_HandReader : MonoBehaviour
{
    private float[] hand = new float[17];
    public int handID;

    public Vector3 EulerOffset = new Vector3(0f, 180f, 180f);
    public Vector3 EulerOffset2 = new Vector3(0f, 75f, 0.0f);

    private Vector3 currentEuler = new Vector3(0, 0, 0);
    private Quaternion currentRotation = new Quaternion();

    public static Vector3 offsetPos = new Vector3();

    private Vector3 currentPosition = new Vector3(0, 0, 0);

    float ortX = 0f;
    float ortY = 0f;
    float ortZ = 0f;
    float ortW = 0f;
    float initOrtX, initOrtY, initOrtZ, initOrtW;
    
    public float alphaPos = 0.8f;
    public float scalePos = 2.5f;
    public float alphaRot = 1.0f;

    public bool AllowMovement = false;
    public bool AllowRotation = false;


    public Transform startPos;

    private Vector3 wristPosition;
    private Vector3 initWristSum;
    private Vector3 initWristPos;
    private int kay = 0;

    private Quaternion initOrient;
    private Quaternion wristQuaternion;

    private void Start()
    {
        initWristSum = new Vector3();
    }

    void Update()
    {
        HandTransformer();
    }

    private void HandTransformer()
    {

        if (handID == 0) // i.e. Right hand 
        {
            hand = Coms_UDP_2.rightData;
            //Debug.Log(hand[0]);
        }
        else // Left hand 
        {
            hand = Coms_UDP_2.leftData;
        }

        GetSensorOffsets();

        // ******************************************************************
        // All things Position **********************************************

        // Get hand position and subtrackt initial position 
        wristPosition = new Vector3(-hand[1], hand[2], hand[0]) - initWristPos;
        currentPosition = (wristPosition + startPos.position) * alphaPos + currentPosition * (1.0f - alphaPos);

        if(AllowMovement)
            transform.position = currentPosition;

        // *********************************************************************
        // All things Orientation **********************************************

        ortX = hand[13];
        ortY = hand[14];
        ortZ = hand[15];
        ortW = hand[16];
        //transform.rotation = new Quaternion(ortX, ortZ, ortY, ortW) * Quaternion.Inverse(EulerOffset);
        wristQuaternion = Quaternion.Euler(EulerOffset2) * (new Quaternion(ortX, -ortZ, ortY, ortW)) * Quaternion.Euler(EulerOffset);
        //wristQuaternion = wristQuaternion * Quaternion.Inverse(initOrient); // Take away initial orientation offset, assuming both gloves are on a flat surface for the first 2 seconds 
        // Make sure quaternion is valid before applying rotation, below get the Quaternion norm
        float norm = Mathf.Sqrt(wristQuaternion.x * wristQuaternion.x + wristQuaternion.y * wristQuaternion.y + wristQuaternion.z * wristQuaternion.z + wristQuaternion.w * wristQuaternion.w);
        if (Mathf.Abs(norm - 1.0f) <= (1e-2 + 1e-4))
        {
            //Debug.Log("Wrist quat: " + wristQuaternion.ToString());
            wristQuaternion.Normalize();

            if(AllowRotation)
                transform.rotation = wristQuaternion;
        }
        else
        {
            Debug.Log("Quaterion not valid... \n");
        }
    }
    
    // Used functions 

    public void GetSensorOffsets()
    {
        // Get initial position and orientation offsets from the position sensor on the gloves   
        if (kay < 10)
        {
            int i = 0;

            for (i = 0; i < 100; i++)
            {
                initWristSum += new Vector3(-hand[1], hand[2], hand[0]);

                initOrtX += hand[13];
                initOrtY += hand[14];
                initOrtZ += hand[15];
                initOrtW += hand[16];
            }

            // Average position offset 
            initWristPos = initWristSum / i;

            // Quaternion norm 
            float init_norm = Mathf.Sqrt(initOrtX * initOrtX + initOrtY * initOrtY + initOrtZ * initOrtZ + initOrtW * initOrtW);
            // Average orientation offset using the "Von Mises–Fisher distribution" 
            initOrient = new Quaternion(initOrtX / init_norm, initOrtY / init_norm, initOrtZ / init_norm, initOrtW / init_norm);

            Debug.Log("ay is of length: " + i);

            kay++;
        }
    }

    //public float AverageGet(float[] arr)
    //{
    //    for (int i = 0; i < arr.length; i++)
    //    {
    //        sum += arr[i];
    //    }
    //   return  average = sum / arr.length;
    //}




    /* Unused functions ********************************************************
    below are some unused functions 
    **************************************************************************** */
    public static Quaternion getRotation(Matrix4x4 matrix)
    {

        Vector3 forward;
        forward.x = matrix.m02;
        forward.y = matrix.m12;
        forward.z = matrix.m22;

        Vector3 up;
        up.x = matrix.m01;
        up.y = matrix.m11;
        up.z = matrix.m21;

        return Quaternion.LookRotation(forward, up);
    }

    public static Vector3 getPosition(Matrix4x4 matrix)
    {

        Vector3 position;

        position.x = matrix.m03;
        position.y = matrix.m13;
        position.z = matrix.m23;

        return position;

    }

    public static Transform multiplyTransforms(Transform t1, Transform t2)
    {
        Transform result = Transform.Instantiate(t1);
        result.position = new Vector3(0.0f, 0.0f, 0.0f);
        result.rotation = new Quaternion(0, 0, 0, 1);

        Matrix4x4 m1, m2, r;

        m1 = t1.localToWorldMatrix;
        m2 = t2.localToWorldMatrix;

        r = m1 * m2;

        result.position = getPosition(r);
        result.rotation = getRotation(r);

        return result;
    }

}
