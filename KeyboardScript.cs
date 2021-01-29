using UnityEngine;
using UnityEngine.UI;

public class KeyboardScript : MonoBehaviour
{
    public float scenedelay = 3f;
    public GameObject[] panels;
    public GameObject[] indicators;
    public InputField[] inputfieldings;
    public static int doneWithForm = 0;
    private string TypedInput = "";

    private int panelIndex = 0;
    private int questionIndex = 0;
    private string st = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ";
    private string newPlayer;

    private void Start()
    {
        panelIndex = 0;

        foreach (GameObject panl in panels)
        {
            panl.SetActive(false);
        }
        panels[0].SetActive(true);

        foreach (GameObject indi in indicators)
        {
            indi.SetActive(false);
        }
        indicators[0].SetActive(true);

        for (int i = 4; i < inputfieldings.Length; i++)
        {
            inputfieldings[i].text = string.Empty;
        }

        // Automatically generate ID number
        char c1 = st[UnityEngine.Random.Range(0, st.Length)];
        char c2 = st[UnityEngine.Random.Range(0, st.Length)];
        char c3 = st[UnityEngine.Random.Range(0, st.Length)];
        char c4 = st[UnityEngine.Random.Range(0, st.Length)];
        string d1 = UnityEngine.Random.Range(0, 1000).ToString();
        string d2 = UnityEngine.Random.Range(0, 1000).ToString();
        string d3 = UnityEngine.Random.Range(0, 1000).ToString();
        string d4 = UnityEngine.Random.Range(0, 1000).ToString();

        // Check whether player already exists before assinging a new player ID to the inputfield
        newPlayer = c1.ToString() + c2.ToString() + d1 + d2 + d3 + c3.ToString() + c4.ToString() + d4;
        if (PlayerPrefs.HasKey("userID"))
        {
            string player = PlayerPrefs.GetString("userID"); // Previous player
            newPlayer = player; // Override new player ID to previous player ID
        }

        //inputfieldings[0].text = SystemInfo.deviceUniqueIdentifier.Substring(34);
        inputfieldings[0].text = newPlayer;

        //Debug.LogError("Today's: " + System.DateTime.Now.ToString());
    }

    private void Update()
    {
        NavigateForm();

        if (panelIndex >= panels.Length - 1 & questionIndex >= inputfieldings.Length - 1)
        {
            CheckForm();
        }
        else
        {
            KeyboardTyping(inputfieldings[questionIndex]);
        }
    }

    private void NavigateForm()
    {
        panelIndex = Mathf.Clamp(panelIndex, 0, panels.Length - 1);
        if (panelIndex < 0 & panelIndex > panels.Length)
            panelIndex = panels.Length - 1;
        questionIndex = Mathf.Clamp(questionIndex, 0, inputfieldings.Length - 1);

        try
        {
            if (TypingScript.Typing & TypingScript.Letter.Contains("Continue"))
            {
                // Next / next page
                foreach (GameObject panl in panels)
                {
                    panl.SetActive(false);
                }

                panelIndex++;
                panelIndex = Mathf.Clamp(panelIndex, 0, panels.Length - 1);
                if (panelIndex < 0 & panelIndex > panels.Length)
                    panelIndex = panels.Length - 1;
                //PreserveIndex();
                panels[panelIndex].SetActive(true);
                NextQuestion();
                TypingScript.Typing = false;
            }
            if (TypingScript.Typing & TypingScript.Letter.Contains("Prev"))
            {
                //Debug.Log("In condition");
                // Back / previous page
                foreach (GameObject panl in panels)
                {
                    panl.SetActive(false);
                }

                panelIndex--;
                panelIndex = Mathf.Clamp(panelIndex, 0, panels.Length - 1);
                //if (panelIndex < 0 & panelIndex > panels.Length)
                //    panelIndex = panels.Length - 1;
                //    panels[panelIndex].SetActive(true);

                PreviousQuestion();

                TypingScript.Typing = false;
            }
        }
        catch
        {
            Debug.Log("Keyboard Error!!!");
        }
    }

    private void PreviousQuestion()
    {
        if (panelIndex < panels.Length - 1)
        { questionIndex = -1; }

        foreach (GameObject indi in indicators)
        {
            indi.SetActive(false);
        }

        TypedInput = "";
        questionIndex--;
        questionIndex = Mathf.Clamp(questionIndex, 0, inputfieldings.Length - 1);
        if (questionIndex < 0 | questionIndex > inputfieldings.Length)
        {
            questionIndex = 0;
        }
        indicators[questionIndex].SetActive(true);

        TypingScript.Typing = false;
    }

    private void NextQuestion()
    {
        if (panelIndex < panels.Length - 1)
        {
            questionIndex = -1;
            inputfieldings[0].text = newPlayer;
        }

        foreach (GameObject indi in indicators)
        {
            indi.SetActive(false);
        }

        TypedInput = "";
        questionIndex++;
        questionIndex = Mathf.Clamp(questionIndex, 0, inputfieldings.Length - 1);

        if (questionIndex < 0 | questionIndex > inputfieldings.Length)
        {
            questionIndex = 0;
        }
        indicators[questionIndex].SetActive(true);

        TypingScript.Typing = false;
    }

    private void CheckForm()
    {
        for (int i = 0; i < inputfieldings.Length; i++)
        {
            if (string.IsNullOrEmpty(inputfieldings[i].text))
            {
                KeyboardTyping(inputfieldings[questionIndex]);
            }
            else
            {
                //Only proceed if all answers to questionnair were appropriate
                if (!string.IsNullOrEmpty(inputfieldings[4].text) & inputfieldings[4].text == "Y" &
                    !string.IsNullOrEmpty(inputfieldings[5].text) & inputfieldings[5].text == "Y" &
                    !string.IsNullOrEmpty(inputfieldings[6].text) & inputfieldings[6].text == "N")
                {
                    PlayerPrefs.SetString("userID", inputfieldings[0].text); //+ System.DateTime.Today.ToString());
                    PlayerPrefs.SetString("age", inputfieldings[1].text);
                    PlayerPrefs.SetString("gender", inputfieldings[2].text);
                    PlayerPrefs.SetString("hand", inputfieldings[3].text);
                    PlayerPrefs.Save();

                    doneWithForm = 1;
                }
                else if
                    (!string.IsNullOrEmpty(inputfieldings[4].text) & inputfieldings[4].text != "Y" &
                     !string.IsNullOrEmpty(inputfieldings[5].text) & inputfieldings[5].text != "Y" &
                     !string.IsNullOrEmpty(inputfieldings[6].text) & inputfieldings[6].text != "N")
                {
                    doneWithForm = 2;
                }
            }
        }
    }

    private void KeyboardTyping(InputField fieldings)
    {
        if (TypingScript.Typing &
            !TypingScript.Letter.Contains("Enter") &
            !TypingScript.Letter.Contains("Space") &
            !TypingScript.Letter.Contains("Continue") &
            !TypingScript.Letter.Contains("Backspace") &
            !TypingScript.Letter.Contains("Up") &
            !TypingScript.Letter.Contains("Down") &
            !TypingScript.Letter.Contains("Del") &
            !TypingScript.Letter.Contains("Clear") &
            !TypingScript.Letter.Contains("CapsLock") &
            !TypingScript.Letter.Contains("Prev"))
        {
            fieldings.text = "";
            TypedInput += TypingScript.Letter;
            fieldings.text = TypedInput;
            TypingScript.Typing = false;
        }

        if (TypingScript.Typing & TypingScript.Letter.Contains("Space"))
        {
            TypedInput += " ";
            fieldings.text = TypedInput;
            TypingScript.Typing = false;
        }
        if (TypingScript.Typing & TypingScript.Letter.Contains("Del"))
        {
            fieldings.text = fieldings.text.Remove(fieldings.text.Length - 1, 1);
            TypedInput = fieldings.text;
            TypingScript.Typing = false;
        }
        if (TypingScript.Typing & TypingScript.Letter.Contains("Clear"))
        {
            ClearAllFields();
        }
    }

    private void ClearAllFields()
    {
        questionIndex = 1;
        indicators[questionIndex].SetActive(true);

        for (int i = 1; i < inputfieldings.Length; i++)
        {
            inputfieldings[i].text = "";
            TypedInput = inputfieldings[i].text;
            TypingScript.Typing = false;
        }
    }
}

//if (TypingScript.Typing & TypingScript.Letter.Contains("Enters"))
//{
//    foreach (GameObject indi in indicators)
//    {
//        indi.SetActive(false);
//    }

//    TypedInput = "";
//    questionIndex++;
//    questionIndex = Mathf.Clamp(questionIndex, 0, inputfieldings.Length - 1);
//    if (questionIndex < 0 & questionIndex > inputfieldings.Length)
//        questionIndex = 0;
//    indicators[questionIndex].SetActive(true);

//    //if (questionIndex > inputfieldings.Length)
//    //{
//    //    questionIndex = 0;
//    //    indicators[questionIndex].SetActive(true);
//    //}
//    //else
//    //{
//    //    indicators[questionIndex].SetActive(true);
//    //}

//    TypingScript.Typing = false;
//}

//panelIndex = Mathf.Clamp(panelIndex, 0, panels.Length - 1);
//if (panelIndex < 0 & panelIndex > panels.Length)
//    panelIndex = panels.Length - 1;
//questionIndex = Mathf.Clamp(questionIndex, 0, inputfieldings.Length-1);
//if (questionIndex < 0 & questionIndex > inputfieldings.Length)
//    questionIndex = 0;

//if (TypingScript.Typing & TypingScript.Letter.Contains("Continue"))
//{
//    // Next / next page
//    foreach (GameObject panl in panels)
//    {
//        panl.SetActive(false);
//    }

//    panelIndex++;
//    panelIndex = Mathf.Clamp(panelIndex, 0, panels.Length - 1);
//    if (panelIndex < 0 & panelIndex > panels.Length)
//        panelIndex = panels.Length - 1;
//    //PreserveIndex();
//    panels[panelIndex].SetActive(true);
//    TypingScript.Typing = false;
//}
//if (TypingScript.Typing & TypingScript.Letter.Contains("Prev"))
//{
//    //Debug.Log("In condition");
//    // Back / previous page
//    foreach (GameObject panl in panels)
//    {
//        panl.SetActive(false);
//    }

//    panelIndex--;
//    panelIndex = Mathf.Clamp(panelIndex, 0, panels.Length - 1);
//    if (panelIndex < 0 & panelIndex > panels.Length)
//        panelIndex = panels.Length - 1; panels[panelIndex].SetActive(true);
//    TypingScript.Typing = false;
//}

//if (TypingScript.Typing & TypingScript.Letter.Contains("Up"))
//{
//    foreach (GameObject indi in indicators)
//    {
//        indi.SetActive(false);
//    }

//    TypedInput = "";
//    questionIndex--;
//    questionIndex = Mathf.Clamp(questionIndex, 0, inputfieldings.Length - 1);
//    if (questionIndex < 0 & questionIndex > inputfieldings.Length)
//        questionIndex = 0;
//    indicators[questionIndex].SetActive(true);

//    //if (questionIndex <= 0) // inputfieldings.Length)
//    //{
//    //    questionIndex = 0;
//    //    indicators[questionIndex].SetActive(true);
//    //}
//    //else
//    //{
//    //    indicators[questionIndex].SetActive(true);
//    //}
//    TypingScript.Typing = false;
//}
//if (TypingScript.Typing & TypingScript.Letter.Contains("Enters"))
//{
//    foreach (GameObject indi in indicators)
//    {
//        indi.SetActive(false);
//    }

//    TypedInput = "";
//    questionIndex++;
//    questionIndex = Mathf.Clamp(questionIndex, 0, inputfieldings.Length - 1);
//    if (questionIndex < 0 & questionIndex > inputfieldings.Length)
//        questionIndex = 0;
//    indicators[questionIndex].SetActive(true);

//    //if (questionIndex > inputfieldings.Length)
//    //{
//    //    questionIndex = 0;
//    //    indicators[questionIndex].SetActive(true);
//    //}
//    //else
//    //{
//    //    indicators[questionIndex].SetActive(true);
//    //}

//    TypingScript.Typing = false;
//}