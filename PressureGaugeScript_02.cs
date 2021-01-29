using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PressureGaugeScript_02 : MonoBehaviour
{
    public static float pressureGauge = 190f; 

    float i = 0;
    void Update()
    {
        transform.localRotation = Quaternion.Euler(0f, pressureGauge + (GameController.leverCycles * 3) , 0f);
    }

}
