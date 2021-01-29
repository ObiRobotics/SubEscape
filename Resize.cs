using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Resize : MonoBehaviour
{
    public ExperimentState expState;
    public Vector3 scaleAxis = new Vector3(1f,1f,1f);
    public Vector3 scaleFactor = new Vector3(1f, 1f, 1f); 

    Vector3 originalScale;
    float[] selfMinMax = new float[2]{0f,180f};
    float[] otherMinMax = new float[2]{0f,1f};

    private void Start()
    {
        originalScale = transform.localScale; 
    }

    void Update()
    {
        expState.scaleFactor.x = expState.Remap(expState.dialAngle, selfMinMax, otherMinMax) * scaleFactor.x;
        expState.scaleFactor.y = expState.Remap(expState.dialAngle, selfMinMax, otherMinMax) * scaleFactor.y;
        expState.scaleFactor.z = expState.Remap(expState.dialAngle, selfMinMax, otherMinMax) * scaleFactor.z;

        if (scaleAxis.x > 0)
        {
            transform.localScale = new Vector3(originalScale.x * expState.scaleFactor.x,
                                               originalScale.y,
                                               originalScale.z);
        }
        if(scaleAxis.y > 0)
        {
            transform.localScale = new Vector3(originalScale.x,
                                               originalScale.y * expState.scaleFactor.y,
                                               originalScale.z);
        }
        if (scaleAxis.z > 0)
        {
            transform.localScale = new Vector3(originalScale.x,
                                               originalScale.y,
                                               originalScale.z * expState.scaleFactor.z);
        }
    }
}
