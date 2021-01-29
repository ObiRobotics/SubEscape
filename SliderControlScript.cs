using UnityEngine;
using UnityEngine.UI;

public class SliderControlScript : MonoBehaviour
{
    public ExperimentState expState;
    public static bool debugMode = false;
    public Slider sliderHandGain;
    public Slider sliderTargetRange;
    public Slider sliderTargetSpeed;
    //public Slider sliderTrackingDifficulty;

    private void Update()
    {
        if (debugMode)
        {
            expState.handGain = sliderHandGain.value;
            expState.targetRange[0] = 0f;
            expState.targetRange[1] = sliderTargetRange.value;
            expState.angularSpeed = sliderTargetSpeed.value;
            //expState.trackDifficulty = sliderTrackingDifficulty.value;
        }
    }
}