using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimatePump : MonoBehaviour
{
    //private Vector3 pos1 = new Vector3(-4, 0, 0);
    private Vector3 initPos;
    public Transform targetPos; 
    public float speed = 1.0f;

    void Start()
    {
        initPos = transform.localPosition; 
    }

    void Update()
    {
        transform.localPosition = Vector3.Lerp(initPos, targetPos.localPosition, (Mathf.Sin(speed * Time.time) + 1.0f) / 2.0f);
    }
}
