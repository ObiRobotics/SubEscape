using UnityEngine;

public class OnButton : MonoBehaviour
{
    public ExperimentState expState;
    public GameObject leftHandPump;
    public GameObject handobj;
    public GameObject dialobj;

    public AudioSource buttonClickSound;
    public bool calibrationMode;
    public bool dialVisibleMode;
    public bool handVisibleMode;
    public bool pauseResume;

    public static int calibrationStep = -1;
    public static bool calibrate;
    public TMPro.TMP_Text debugText;
    public TMPro.TMP_Text pauseText;

    public Collider myCollider;
    private MeshRenderer dialMesh;
    private SkinnedMeshRenderer handMesh;

    private void Start()
    {
        pauseText.text = "Pause";
        dialMesh = dialobj.GetComponent<MeshRenderer>();
        handMesh = handobj.GetComponent<SkinnedMeshRenderer>();

        myCollider = GetComponent<Collider>();
    }

    //private void Update()
    //{
    //if (calibrationStep == -1)
    //    debugText.text = "Open hand!";
    //else if (calibrationStep == 0)
    //    debugText.text = "Close hand!";
    //else if (calibrationStep == 1)
    //    debugText.text = "Calibration done!";
    //else if (calibrationStep == 2)
    //    debugText.text = "Repeat calibration?";
    //}

    private void OnTriggerEnter()
    {
        if (!calibrationMode)
        {
            if (!dialVisibleMode)
            {
                if (!handVisibleMode)
                {
                    //expState.startTrial[0] = 1;
                    //expState.startTrial[1] = 1;
                    buttonClickSound.Play();
                }
            }
        }

        if (pauseResume)
        {
            if (expState.pause) // Start again
            {
                //expState.startTrial[2] = 1;
                expState.startExperiment = 1;
                expState.pause = false;
                pauseText.text = "Pause";
            }
            else // Pause
            {
                expState.pause = true;
                pauseText.text = "Resume";
            }
        }

        if (calibrationMode)
        {
            buttonClickSound.Play();

            calibrate = true;
            calibrationStep++;
            //Debug.Log("Calib step: " + calibrationStep.ToString());

            if (calibrationStep > 2)
            {
                GameController_Dial.calibrated = false;
                calibrationStep = -1;
            }

            myCollider.enabled = false;
            Invoke("ReactivateCollider", 2f);
        }

        if (dialVisibleMode)
        {
            buttonClickSound.Play();
            if (!dialMesh.enabled)
            {
                dialMesh.enabled = true;
            }
            else if (dialMesh.enabled)
            {
                dialMesh.enabled = false;
            }
        }

        if (handVisibleMode)
        {
            buttonClickSound.Play();
            if (!handMesh.enabled)
            {
                handMesh.enabled = true;
            }
            else if (handMesh.enabled)
            {
                handMesh.enabled = false;
            }
        }

        // ActivatePump();
    }

    private void ReactivateCollider()
    {
        myCollider.enabled = true;
    }

    private void ActivatePump()
    {
        if (!leftHandPump.activeInHierarchy)
        {
            leftHandPump.SetActive(true);
        }
        else if (leftHandPump.activeInHierarchy)
        {
            leftHandPump.SetActive(false);
        }
    }
}