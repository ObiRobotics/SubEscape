using TMPro;
using UnityEngine;

public class OnScore : MonoBehaviour
{
    public ExperimentState expState;
    private TMP_Text scoretxt;
    private Color origColor;

    private void Start()
    {
        scoretxt = GetComponent<TMP_Text>();
        origColor = scoretxt.faceColor;
    }

    private void Update()
    {
        scoretxt.text = "[" + expState.score.ToString() + "]";
        //Debug.Log(expState.score);

        if (expState.addtoScore == 1)
            scoretxt.faceColor = origColor;
        else if (expState.addtoScore > 1)
            scoretxt.faceColor = new Color(0.65f, 0.5f, 0f, 1f);
    }
}