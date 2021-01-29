using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro; 

public class SliderScript : MonoBehaviour
{
    public TMP_Text gainDisplay;
    public Slider _slider;
    public static float gainSliderValue = 1f;
    public float incrementval = 0.01f; 

    void Start()
    {
        gainDisplay.text = _slider.value.ToString("F3"); 
    }

    void OnTriggerEnter(Collider other)
    {
        gainSliderValue += incrementval;
        _slider.value = gainSliderValue;
        gainDisplay.text = _slider.value.ToString("F3");
    }
}
