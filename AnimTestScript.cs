using UnityEngine;

public class AnimTestScript : MonoBehaviour
{
    public ExperimentState expState;
    private Animator handOpenClose = new Animator();
    //AnimationState animState = new AnimationState();

    private float animTime = 1;

    [Range(0f, 10f)]
    public float animScale = 1f;

    private void Start()
    {
        handOpenClose = GetComponent<Animator>();
    }

    private void Update()
    {
        if (expState.block == 1)
        {
            float handOpenState = expState.virtDialAngle.Normalize(180f);
            handOpenClose.SetFloat("handAnimTime", animTime - (animScale * handOpenState));
        }
        else
        {
            float handOpenState = expState.virtDialAngle.Normalize(180f);
            handOpenClose.SetFloat("handAnimTime", animTime - handOpenState);
        }
    }
}