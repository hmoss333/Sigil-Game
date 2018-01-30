using System.Collections;
using System.Collections.Generic;
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
        sigilName.text = currentString;
    }

    public void CheckTriggers ()
    {
        correctTriggerList.Clear();
        int orderNum = 0;
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
                        orderNum++;
                        correctTriggerList.Add(trigger);
                        trigger.isCorrect = true;
                        trigger.orderNum.text = orderNum.ToString();
                    }
                }
            }
        }
    }

    public void AddTriggerValues (string values)
    {
        currentString = currentString + values;

        char[] tempCurrentChars = currentString.ToCharArray();
        char[] tempCheckChars = checkString.ToCharArray();

        string tempTestString = "";

        for (int i = 0; i < tempCurrentChars.Length; i++)
        {
            for (int j = 0; j < tempCheckChars.Length; j++)
            {
                if (tempCheckChars[j] == tempCurrentChars[i] && !tempTestString.Contains(tempCurrentChars[i].ToString()))
                {
                    tempTestString = tempTestString + tempCurrentChars[i];
                    Debug.Log(tempTestString);
                }
            }
        }

        currentString = tempTestString;
        int testCount = 0; //this seems bad/lazy
        for (int i = 0; i < tempCheckChars.Length; i++)
        {
            if (tempTestString.Contains(tempCheckChars[i].ToString())) 
            {
                testCount++;
                if (testCount >= tempCheckChars.Length)
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
                    
            }
        }

        //if (tempTestString == checkString)
        //{
        //    sigilName.text = "Correct";
        //    checkString = "";
        //    foreach (TriggerTest trigger in correctTriggerList)
        //    {
        //        trigger.image.color = Color.white;
        //        trigger.isCorrect = false;
        //    }
        //}
    }

    void CheckSigil ()
    {
        //char[] tempCheckChars = checkString.ToCharArray();
        //char[] tempCurrentChars = currentString.ToCharArray();


        if (currentString.Contains(checkString))
        {
            sigilName.text = "Correct";
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
