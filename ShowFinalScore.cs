using UnityEngine;

public class ShowFinalScore : MonoBehaviour
{
    public ExperimentState expState;
    public TMPro.TMP_Text goodbyeText;

    private void Start()
    {
        if (expState.group == 0)
        {
            goodbyeText.text =
            "The End \n" +
            "You have saved the submarine from sinking. \n" +
            "\n" +
            "Many thanks for taking part in this experiment. \n" +
            "\n" +
            "You may quit the app now. ";
        }
        else
        {
            goodbyeText.text =
            "The End \n" +
            "You have saved the submarine from sinking. \n" +
            "\n" +
            "Your total score is: " + expState.score.ToString() + "\n" +
            "\n" +
            "Many thanks for taking part in this experiment. \n" +
            "\n" +
            "You may quit the app now. ";
        }
    }
}