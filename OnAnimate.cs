using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Animation))]
public class OnAnimate : MonoBehaviour
{
    public ExperimentState expState;
    public Animation scaleAnimation;
    public Animation textAnimation;
    public TMPro.TMP_Text scoreText;
    public AudioSource[] scoreAccumulationSound;
    public Renderer indicatorRend;
    public float waitTime = 4f;
    public int bonusPoints = 4;
    public GameObject TreasureChest;
    public GameObject[] RewardRings;

    private int accumulatedReward;
    private bool onTargetStay = false;
    private Coroutine soundRoutine;

    private float startTime = 0f;
    private float elapsedTime = 0f;
    private bool bonusScore = false;

    private Material _mat;
    private Transform scaledObj;
    private Vector3 originalScale;

    [Range(0f, 0.5f)]
    public float rewardDelay1 = 0.2f;

    [Range(0f, 0.5f)]
    public float rewardDelay2 = 0.2f;

    private void Start()
    {
        if (expState.group == 0)
        {
            scoreText.text = "";
        }

        foreach (GameObject rewardRing in RewardRings)
        {
            rewardRing.SetActive(false);
        }

        scaledObj = transform;
        originalScale = transform.localScale;

        TreasureChest.SetActive(false);

        _mat = indicatorRend.GetComponent<Renderer>().material;
        //scaleAnimation = GetComponent<Animation>();
    }

    private void Update()
    {
        if (onTargetStay)
            elapsedTime = Time.time - startTime;
        else
            elapsedTime = 0f;

        //Debug.Log("T: " + elapsedTime);
        if (expState.group != 0) // Only if you are in group one should there be a bonus round
        {
            // TernaryOperator approach to if statement
            bonusScore = elapsedTime >= waitTime ? true : false;
        }
    }

    private void OnTriggerEnter()
    {
        if (expState.group != 0) // Only if you are in group one should there be a bonus round
        {
            //scaleAnimation.Play();
            textAnimation.Play();
            //scoreAccumulationSound.Play();
            startTime = Time.time;
            StartCoroutine(PlaySound());
        }
    }

    private void OnTriggerStay()
    {
        if (expState.group != 0) // Only if you are in group one should there be a bonus round
        {
            //scaleAnimation.Play();
            textAnimation.Play();
            if (!onTargetStay)
            {
                //StartCoroutine(ScaleAnimation());
                StartCoroutine(PlaySound());
                onTargetStay = true;
            }
        }
    }

    private void OnTriggerExit()
    {
        if (expState.group != 0) // Only if you are in group one should there be a bonus round
        {
            onTargetStay = false;
        }
    }

    private IEnumerator PlaySound()
    {
        if (bonusScore)
        {
            _mat.SetColor("_EmissionColor", Color.red * 6.0f);
            expState.addtoScore = bonusPoints;
            expState.score += expState.addtoScore;
            accumulatedReward += bonusPoints;
            scoreText.text = "+" + bonusPoints.ToString();
            scoreAccumulationSound[1].Play();
            yield return new WaitForSecondsRealtime(rewardDelay1);

            if (accumulatedReward == 12)
            {
                RewardRings[0].SetActive(true);
            }
            if (accumulatedReward == 20)
            {
                RewardRings[1].SetActive(true);
            }
            if (accumulatedReward == 32)
            {
                accumulatedReward = 0;
                StartCoroutine(RewardChest());
                elapsedTime = 0f;
                bonusScore = false;
            }
        }
        else
        {
            // Reset accumulated reward
            accumulatedReward = 0;
            foreach (GameObject rewardRing in RewardRings)
            {
                rewardRing.SetActive(false);
            }
            _mat.SetColor("_EmissionColor", Color.black * 1f);
            expState.addtoScore = 1;
            expState.score += expState.addtoScore;
            scoreText.text = "+1";
            scoreAccumulationSound[0].Play();
            yield return new WaitForSecondsRealtime(rewardDelay2);
        }

        onTargetStay = false;

        //yield return null;
    }

    private IEnumerator RewardChest()
    {
        RewardRings[2].SetActive(true);
        TreasureChest.SetActive(true);
        expState.score += 100;
        scoreText.text = "+100";
        yield return new WaitForSecondsRealtime(1.5f);

        TreasureChest.SetActive(false);
        foreach (GameObject rewardRing in RewardRings)
        {
            rewardRing.SetActive(false);
        }

        bonusScore = false;
        elapsedTime = 0f;
    }

    private IEnumerator ScaleAnimation()
    {
        while (!onTargetStay)
            yield return null;

        float scaleto = 1.25f;
        float scaleVal = originalScale.z;

        float scaleDiff = originalScale.z - scaleto;

        if (scaleDiff >= 0f)
        {
            while (transform.localScale.z < scaleto)
            {
                scaleVal += 0.01f;
                scaledObj.localScale = new Vector3(originalScale.x,
                                                   originalScale.y,
                                                   originalScale.z * Mathf.Sin(scaleVal));
                transform.SetParent(scaledObj);
                yield return null;
            }
        }
        //else //if(dialDiff > 0)
        //{
        //    while (yRot > TargetAngle)
        //    {
        //        yRot -= expState.angularSpeed;

        //        Quaternion toRot = Quaternion.Euler(0f, yRot, 0f);
        //        transform.localRotation = toRot;
        //        yield return null;
        //    }
        //}

        //yield return null;
    }
}