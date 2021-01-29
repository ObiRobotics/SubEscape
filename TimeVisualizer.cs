using System.Collections;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Slider))]
public class TimeVisualizer : MonoBehaviour
{
    public ExperimentState expState;
    public Image timerSliderImage;
    public float BetweenTrialTime = 3.5f;

    private AudioSource buttonClickSound;

    private float startTime = 0f;
    private float startTime2 = 0f;
    private float startTime3 = 0f;

    private Slider timerSlider;
    private Color sliderInitColor;

    private Coroutine sliderTimerRoutine;
    private Coroutine nextTrialRoutine;

    private void Start()
    {
        buttonClickSound = GameObject.FindGameObjectWithTag("ButtonClick").GetComponent<AudioSource>();
        timerSlider = GetComponent<Slider>();
        sliderInitColor = timerSliderImage.color;
    }

    private void Update()
    {
        if (expState.pause)
        {
            expState.ResetTrial();
            if (sliderTimerRoutine != null)
            {
                StopCoroutine(sliderTimerRoutine);
            }
            if (nextTrialRoutine != null)
            {
                StopCoroutine(nextTrialRoutine);
            }
        }

        // When tutorial is over then start the init experiment sequence
        if (expState.startExperiment == 1)
        {
            startTime3 = Time.time;
            StartCoroutine(InitExperimentCountDown());
            expState.startExperiment = 0;
        }

        if (expState.startTrial[2] == 1)
        {
            timerSliderImage.color = sliderInitColor;
            startTime = Time.time;
            sliderTimerRoutine = StartCoroutine(SliderCounter());
            expState.startTrial[2] = 0;
        }
    }

    public IEnumerator SliderCounter()
    {
        while (Time.time - startTime < expState.trialDuration)
        {
            //Debug.Log("Time: " + (Time.time - startTime).ToString("F2"));
            float progressTime = Mathf.Clamp01((Time.time - startTime) / expState.trialDuration);
            timerSlider.value = 1f - progressTime;
            timerSliderImage.color = new Color(1f, 1f - progressTime, 0f, 0.15f);

            yield return null;
        }

        expState.inTrial = false;

        startTime2 = Time.time;
        nextTrialRoutine = StartCoroutine(NextTrialCountDown());
    }

    public IEnumerator NextTrialCountDown()
    {
        while (Time.time - startTime2 < BetweenTrialTime) // & expState.inTrial
        {
            //Debug.Log("Time: " + (Time.time - startTime2).ToString("F2"));
            float progressTime = Mathf.Clamp01((Time.time - startTime2) / BetweenTrialTime);
            timerSlider.value = 1f - progressTime;
            timerSliderImage.color = new Color(1f, 1f - progressTime, 0f, 0.15f);

            yield return null;
        }

        //if (!expState.pause)
        //{
        for (int i = 0; i < expState.startTrial.Length; i++)
        {
            expState.startTrial[i] = 1;
        }
        buttonClickSound.Play();
        //}
    }

    public IEnumerator InitExperimentCountDown()
    {
        while (Time.time - startTime3 < expState.initDelay) // & expState.inTrial
        {
            //Debug.Log("Time: " + (Time.time - startTime2).ToString("F2"));
            float progressTime = Mathf.Clamp01((Time.time - startTime3) / expState.initDelay);
            timerSlider.value = 1f - progressTime;
            timerSliderImage.color = new Color(1f, 1f - progressTime, 0f, 0.15f);

            yield return null;
        }

        //if (!expState.pause)
        //{
        for (int i = 0; i < expState.startTrial.Length; i++)
        {
            expState.startTrial[i] = 1;
        }
        buttonClickSound.Play();
        //}
    }
}

//if (expState.startTrial[3] == 1)
//{
//    startTime2 = Time.time;
//    StartCoroutine(NextTrialCountDown());
//    expState.startTrial[3] = 0;
//}