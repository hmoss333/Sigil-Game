using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.UI;
using System.IO;

public class SigilManager : MonoBehaviour {

    public string currentString;
    public string checkString;
    public static string screenShotName;
    public Text sigilName;

    public TriggerTest[] triggerList;
    public List<TriggerTest> correctTriggerList;
    Dictionary<string, Texture> storedSigils;

    public Button[] keys;

    HiResScreenShots hrss;

    // Use this for initialization
    void Start () {
        hrss = GameObject.FindObjectOfType<HiResScreenShots>();
        triggerList = GameObject.FindObjectsOfType<TriggerTest>();

        screenShotName = "";

        storedSigils = new Dictionary<string, Texture>();
        StartCoroutine(GetFiles());
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
                        hrss.takeHiResShot = true;
                        StartCoroutine(GetFiles());
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

    IEnumerator GetFiles()
    {
        yield return new WaitForSeconds(0.25f);
        storedSigils.Clear();

        string info = Application.persistentDataPath + "/Sigils/";
        string[] fileInfo = Directory.GetFiles(info, "*.png");
        //System.IO.Path.GetFileName(fullPath)
        for (int i = 0; i < fileInfo.Length; i++)
        {
            string url = "file://" + fileInfo[i];

            WWW www = new WWW(url);

            // Wait for download to complete
            yield return www;

            storedSigils.Add(fileInfo[i], www.texture);
            Debug.Log(fileInfo[i].ToString());
        }
    }

    //==Button Inputs==//

    public void LetterButton (string character)
    {
        checkString = checkString + character;
        screenShotName = checkString;

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

    public void ClearButton ()
    {
        checkString = "";
        currentString = "";
        screenShotName = "";

        sigilName.text = checkString;
        foreach (TriggerTest trigger in correctTriggerList)
        {
            trigger.image.color = Color.white;
            trigger.orderNum.text = "";
            trigger.isCorrect = false;
        }
        CheckTriggers();
    }
}
