using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.UI;

public class SigilManager : MonoBehaviour {

    public string currentString;
    public string checkString;
    public Text sigilName;

    public TriggerTest[] triggerList;
    public List<TriggerTest> correctTriggerList;

    public Button[] keys;

    // Use this for initialization
    void Start () {
        triggerList = GameObject.FindObjectsOfType<TriggerTest>();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    //public void AddChar(char character)
    //{
    //    if (currentString.Length < 10)
    //    {
    //        currentString = currentString + character;
    //        sigilName.text = currentString;
    //    }
    //}

    public void ClearString ()
    {
        currentString = "";
        //sigilName.text = currentString;
    }

    string RemoveDuplicateLetters(string testString)
    {
        string tempString = "";

        foreach (char character in testString)
        {
            if (!tempString.Contains(character.ToString()))
                tempString += character;
        }

        sigilName.text = tempString;
        return tempString;
    }

    public void CheckTriggers ()
    {
        correctTriggerList.Clear();
        char[] tempCheckChars = checkString.ToCharArray();

        foreach (TriggerTest trigger in triggerList)
        {
            char[] tempTriggerChars = trigger.letters.ToCharArray();

            for (int i = 0; i < tempTriggerChars.Length; i++)
            {
                for (int j = 0; j < tempCheckChars.Length; j++)
                {
                    if (tempCheckChars[j] == tempTriggerChars[i])
                    {
                        correctTriggerList.Add(trigger);
                        trigger.isCorrect = true;
                        trigger.orderNum.text += (j + 1) + ", ";
                    }
                }
            }
        }
    }

    public void AddTriggerValues (string values)
    {
        //currentString = currentString + values;
        bool hasMatched = false;
        char[] tempCurrentChars = values.ToCharArray();
        //char[] tempCheckChars = checkString.ToCharArray();

        string tempTestString = currentString;

        foreach (char character in checkString)
        {
            for (int j = 0; j < tempCurrentChars.Length; j++)
            {
                if (character == tempCurrentChars[j] && !hasMatched && !tempTestString.Contains(tempCurrentChars[j].ToString()))
                {
                    tempTestString = tempTestString + tempCurrentChars[j];
                    Debug.Log(tempTestString);
                    currentString = tempTestString;
                    hasMatched = true;

                    if (currentString == checkString)
                    {
                        sigilName.text = "Correct";
                        checkString = "";
                        foreach (TriggerTest trigger in correctTriggerList)
                        {
                            trigger.image.color = Color.white;
                            trigger.orderNum.text = "";
                            trigger.isCorrect = false;
                        }
                    }
                    break;
                }
                if (hasMatched)
                    break;
            }
        }
    }

    //==Button Inputs==//

    public void LetterButton (string character)
    {
        checkString = checkString + character;

        sigilName.text = checkString;
    }

    public void GenerateButton ()
    {
        checkString = RemoveDuplicateLetters(checkString);
        CheckTriggers();

        if (correctTriggerList.Count != 0)
        {
            foreach (TriggerTest trigger in correctTriggerList)
            {
                trigger.GetComponent<Image>().color = Color.green;
            }
        }
        else
        {
            Debug.Log("Something went wrong");
        }
    }
}
