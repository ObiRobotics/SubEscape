using UnityEngine;
using UnityEngine.UI;

public class TrackingDifficulty : MonoBehaviour
{
    public ExperimentState expState;
    public bool useSlider = false;
    public Slider sliderTrackingDifficulty;

    private Vector3 originalScale;
    private Transform scaledObj;

    private void Start()
    {
        scaledObj = transform;
        originalScale = transform.localScale;
        //scaledObj.localScale = originalScale;
    }

    private void Update()
    {
        if (useSlider)
        {
            scaledObj.localScale = new Vector3(originalScale.x, originalScale.y, originalScale.z * sliderTrackingDifficulty.value);
            transform.SetParent(scaledObj);
        }
        else
        {
            scaledObj.localScale = new Vector3(originalScale.x, originalScale.y, originalScale.z * expState.trackDifficulty);
            transform.SetParent(scaledObj);
        }
    }
}