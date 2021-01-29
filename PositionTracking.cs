using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PositionTracking : MonoBehaviour
{

    public bool ObiPos;
    public bool LeapPos;
    public bool QuestPos;

    public bool ObiRot;
    public bool LeapRot;
    public bool QuestRot;

    public Transform LeapHand;
    //public Transform QuestHand; 

    void Start()
    {
        
    }

    void Update()
    {
        if(ObiPos)
        {
            //transform.position = ObiTracker.position;
        }
        if(LeapPos & LeapRot)
        {
            transform.position = LeapHand.position;
            transform.rotation = LeapHand.rotation;
        }
        if(QuestPos)
        {
            //transform.position = QuestHand.position;  
        }
    }
}
