using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using TMPro; 

[System.Serializable]
public struct Gesture
{
    public string name;
    public List<Vector3> fingerData;
    public UnityEvent onRecognized; 
}

public class GestureDetector : MonoBehaviour
{
    public static string gestureState;
    public static bool saveGesture;
    public static float threshold = 0.05f;

    public TMP_Text dispaly; 
    public OVRSkeleton skeleton; // # Make whether Left or Right hand is chosen dependent on the PlayerPrefs (hand) parameter
    public List<Gesture> gestures; 
    public List<OVRBone> fingerBones;
    private Gesture previousGesture; 

    void Start()
    {
        fingerBones = new List<OVRBone>(skeleton.Bones); // Reference to bones of the hand/fingers
        previousGesture = new Gesture(); 
    }

    void Update()
    {
        fingerBones = new List<OVRBone>(skeleton.Bones);

        if (Input.GetKeyDown(KeyCode.G) | saveGesture) // This boolean is set in the GameController scripts 
        {
            saveGesture = false;
            Save(GameController.gestureName); 
        }

        Gesture currentGesture = Recognize();
        bool hasRecognized = !currentGesture.Equals(new Gesture());
        // Checkif new gesture 
        if(hasRecognized && !currentGesture.Equals(previousGesture))
        {
            // New Gesture !!
            // Debug.Log("New gesture fond: " + currentGesture.name);
            previousGesture = currentGesture;
            //currentGesture.onRecognized.Invoke();

            gestureState = currentGesture.name;
            //dispaly.text = gestureState; 
        }
    }

    void Save(string gestureName)
    {
        Gesture g = new Gesture();

        g.name = gestureName; //"OpenGesture";


        List<Vector3> data = new List<Vector3>();
        foreach(var bone in fingerBones)
        {
            // finger position relative to root of hand
            data.Add(skeleton.transform.InverseTransformPoint(bone.Transform.position));
            Debug.Log(bone.Transform.name); 
        }

        g.fingerData = data;
        gestures.Add(g);
    }

    Gesture Recognize()
    {
        Gesture currentgesture = new Gesture();
        float curretnMin = Mathf.Infinity; 

        foreach(var gesture in gestures)
        {
            float sumDistance = 0f; // the similarity of the hand to the predefined/saved gestures
            bool isDiscarded = false; // to tell us whether the current pose is discarded or not

            for(int i = 0; i<fingerBones.Count; i++)
            {
                Vector3 currentData = skeleton.transform.InverseTransformPoint(fingerBones[i].Transform.position);
                float distance = Vector3.Distance(currentData, gesture.fingerData[i]);
                
                
                if(distance>threshold)
                {
                    isDiscarded = true;
                    break; 
                }
                sumDistance += distance;     
            }

            if(!isDiscarded && sumDistance < curretnMin)
            {
                curretnMin = sumDistance;
                currentgesture = gesture; 
            }
        }

        return currentgesture;
    }

}
