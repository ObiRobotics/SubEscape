using System.Collections;
using UnityEngine;

public class TutorialAnim : MonoBehaviour
{
    public ExperimentState expState;
    public Animation[] animations;
    public AudioSource[] instructions;
    public Collider TargetCollider;
    public GameObject[] TutorialObjects; // Active during tutorial
    public MoveDial[] moveDialObjects; // Inactive during tutorial
    //public GameObject TimerObject; // Inactive during tutorial

    public AnimTestScript animTestScript; // Switch this off during the tutorial
    public StimulusHand stimulusHandScript; // Switch this on during the tutorial
    public Animator stimulusHandAnim;

    public bool skipTutorial;

    private void Start()
    {
        //animTestScript.enabled = false;
        //stimulusHandScript.enabled = true;
        //stimulusHandAnim.enabled = false;

        // Deactivate tutorial objects
        foreach (GameObject tutorObjects in TutorialObjects)
        {
            tutorObjects.SetActive(false);
        }
        foreach (MoveDial moveDialObject in moveDialObjects)
        {
            moveDialObject.enabled = false;
        }

        TargetCollider.enabled = false;
        expState.startExperiment = 0;
        for (int i = 0; i < expState.startTrial.Length; i++)
        {
            expState.startTrial[i] = 0;
        }

        expState.block = 0;

        Invoke("StartTutorial", 3f);
    }

    private void StartTutorial()
    {
        foreach (GameObject tutorObjects in TutorialObjects)
        {
            tutorObjects.SetActive(true);
        }

        if (skipTutorial)
        {
            StartCoroutine(SkippedTutorial());
        }
        else
        {
            StartCoroutine(TutorialSequence());
        }
    }

    private IEnumerator TutorialSequence()
    {
        instructions[0].Play(); // Welcome
        while (instructions[0].isPlaying)
            yield return null;
        yield return new WaitForSecondsRealtime(1f);

        instructions[12].Play(); // Lighting and Left Hand 
        while (instructions[12].isPlaying)
            yield return null;
        yield return new WaitForSecondsRealtime(1f);

        instructions[1].Play(); // Pressure meter
        animations[3].Play(); // Spot light animation
        while (instructions[1].isPlaying)
            yield return null;
        animations[3].Stop();
        yield return new WaitForSecondsRealtime(1f);

        instructions[2].Play(); // Target indicator
        animations[2].Play();
        while (instructions[2].isPlaying)
            yield return null;
        animations[2].Stop();
        yield return new WaitForSecondsRealtime(1f);

        instructions[3].Play(); // Move dial
        animations[1].Play();
        while (instructions[3].isPlaying)
            yield return null;
        animations[1].Stop();
        yield return new WaitForSecondsRealtime(1f);

        instructions[4].Play(); // Open-Close hand
        animations[0].Play();
        while (instructions[4].isPlaying)
            yield return null;
        animations[0].Stop();
        yield return new WaitForSecondsRealtime(1f);

        if (expState.group != 0) // Reward trials
        {
            instructions[5].Play(); // Reward coins
            TargetCollider.enabled = true;
            animations[0].Play();
            animations[1].Play();
            animations[2].Play();
            while (instructions[5].isPlaying)
                yield return null;
            yield return new WaitForSecondsRealtime(1f);

            instructions[6].Play(); // Reward medallions
            while (instructions[6].isPlaying)
                yield return null;
            yield return new WaitForSecondsRealtime(2f);
        }
        else // No reward trials
        {
            //TargetCollider.enabled = true;
            animations[0].Play();
            animations[1].Play();
            animations[2].Play();
            yield return new WaitForSecondsRealtime(3f);
        }

        // Why is this animation repeated?
        // Because if it is a no-reward group then you get the animation once here.
        // Otherwise, for a reward group, you get this animation below twice.
        animations[0].Stop();
        animations[1].Stop();
        animations[2].Stop();
        TargetCollider.enabled = false;

        instructions[7].Play(); // Beaware of weird hand!!!
        while (instructions[7].isPlaying)
            yield return null;
        yield return new WaitForSecondsRealtime(2f);

        // End of experiment
        instructions[8].Play();
        while (instructions[8].isPlaying)
            yield return null;
        instructions[9].Play();
        while (instructions[9].isPlaying)
            yield return null;

        // Deactivate tutorial objects
        foreach (GameObject tutorObjects in TutorialObjects)
        {
            tutorObjects.SetActive(false);
        }
        foreach (MoveDial moveDialObject in moveDialObjects)
        {
            moveDialObject.enabled = true;
        }
        expState.score = 0;

        //stimulusHandAnim.enabled = true;
        //animTestScript.enabled = true;
        //stimulusHandScript.enabled = false;

        //instructions[11].Play(); // Open and close hand 5 times (calibration purposes)
        //while (instructions[11].isPlaying)
        //    yield return null;

        instructions[10].Play(); // Open-Close hand to move dial 
        yield return new WaitForSecondsRealtime(1f);

        //TimerObject.SetActive(true);
        expState.startExperiment = 1; // Start the experiment -> see "TimeVisualizer.cs" script
    }

    private IEnumerator SkippedTutorial()
    {
        // Deactivate tutorial objects
        foreach (GameObject tutorObjects in TutorialObjects)
        {
            tutorObjects.SetActive(false);
        }
        foreach (MoveDial moveDialObject in moveDialObjects)
        {
            moveDialObject.enabled = true;
        }
        expState.score = 0;

        instructions[10].Play();
        while (instructions[10].isPlaying)
            yield return null;

        //stimulusHandAnim.enabled = true;
        //animTestScript.enabled = true;
        //stimulusHandScript.enabled = false;

        expState.startExperiment = 1; // Start the experiment -> see "TimeVisualizer.cs" script
    }
}

//private void Update()
//{
//    if(Input.GetKeyDown(KeyCode.Z))
//    {
//        foreach (GameObject tutorObjects in TutorialObjects)
//        {
//            tutorObjects.SetActive(true);
//        }
//        StartCoroutine(TutorialSequence());
//    }
//}