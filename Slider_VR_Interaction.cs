using UnityEngine;
using UnityEngine.UI;

public class Slider_VR_Interaction : MonoBehaviour
{
    public ExperimentState expState;
    private Slider _slider;

    public Transform sliderKnob;
    public float[] knobRange = new float[2];

    private float[] otherMinMax = new float[2];

    private void Start()
    {
        _slider = GetComponent<Slider>();
    }

    private void Update()
    {
        //_slider.value = Remap(sliderKnob.localPosition.x);
        otherMinMax[0] = _slider.minValue;
        otherMinMax[1] = _slider.maxValue;

        _slider.value = expState.Remap(sliderKnob.localPosition.x, knobRange, otherMinMax);
    }

    //public float Remap(float value)
    //{
    //    return _slider.minValue + (value - knobRange[0]) * ((_slider.maxValue - _slider.minValue) / (knobRange[1] - knobRange[0]));
    //}

    //public float Remap(float value, float[] selfMinMax, float[] otherMinMax)
    //{
    //    return otherMinMax[0] + (value - selfMinMax[0]) * ((otherMinMax[1] - otherMinMax[0]) / (selfMinMax[1] - selfMinMax[0]));
    //}
}