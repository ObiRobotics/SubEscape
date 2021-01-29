using UnityEngine;

public class OnSessionSave : MonoBehaviour
{
    public ExperimentState expState;

    private void Start()
    {
        if (!PlayerPrefs.HasKey("bend_state_0"))
        {
            PlayerPrefs.SetFloat("bend_state_0", expState.bendState[0]);
            PlayerPrefs.SetFloat("bend_state_1", expState.bendState[1]);
            PlayerPrefs.SetInt("score", expState.score);
        }
        else
        {
            expState.bendState[0] = PlayerPrefs.GetFloat("bend_state_0");
            expState.bendState[1] = PlayerPrefs.GetFloat("bend_state_1");
            expState.score = PlayerPrefs.GetInt("score");
        }
    }
}