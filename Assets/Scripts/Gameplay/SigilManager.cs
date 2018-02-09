using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.IO;
using System.Linq;

public class SigilManager : MonoBehaviour {

    public string currentString;
    public string checkString;
    public static string screenShotName;
    public Text sigilName;

    public TriggerTest[] triggerList;
    public List<TriggerTest> correctTriggerList;
    Dictionary<string, Texture> storedSigils;
    public Dropdown sigilList;

    public RawImage storedImage;

    HiResScreenShots hrss;

    // Use this for initialization
    void Start () {
        hrss = GameObject.FindObjectOfType<HiResScreenShots>();
        triggerList = GameObject.FindObjectsOfType<TriggerTest>();

        screenShotName = "";
        storedImage.gameObject.SetActive(false);

        storedSigils = new Dictionary<string, Texture>();
        StartCoroutine(GetFiles());
    }
	
	// Update is called once per frame
	void Update () {
		
	}


    //==Generate Trigger Order==//
    public void ClearString ()
    {
        currentString = "";
        storedImage.gameObject.SetActive(false);
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

    string ConvertToInt(string testString)
    {
        string numberOrder = "";

        for (int i = 0; i < testString.Length; i++)
        {
            if ("AHOV".Contains(testString[i]))
                numberOrder += "1";
            if ("BIPW".Contains(testString[i]))
                numberOrder += "2";
            if ("CJQX".Contains(testString[i]))
                numberOrder += "3";
            if ("DKRY".Contains(testString[i]))
                numberOrder += "4";
            if ("ELSZ".Contains(testString[i]))
                numberOrder += "5";
            if ("FMT".Contains(testString[i]))
                numberOrder += "6";
            if ("GNU".Contains(testString[i]))
                numberOrder += "7";

            if (i < testString.Length - 1)
                numberOrder += ", ";
        }

        return numberOrder;
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
                        //trigger.orderNum.text += (j + 1) + ", ";
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
                    break;
                }
                if (hasMatched)
                    break;
            }
            if (hasMatched)
                break;
        }

        if (currentString == checkString)
        {
            //StartCoroutine(ShowLoadingScreen());
            SceneManager.LoadSceneAsync("LoadingScreen", LoadSceneMode.Additive);

            sigilName.text = "Correct";
            //hrss.takeHiResShot = true;
            StartCoroutine(TakeScreenshot());
            StartCoroutine(GetFiles());
            checkString = "";

            foreach (TriggerTest trigger in correctTriggerList)
            {
                trigger.image.color = Color.white;
                //trigger.orderNum.text = "";
                trigger.isCorrect = false;
            }

            StartCoroutine(CloseLoadingScreen());
        }
    }

    IEnumerator TakeScreenshot ()
    {
        yield return new WaitForSeconds(0.1f);
        hrss.takeHiResShot = true;
    }

    IEnumerator CloseLoadingScreen()
    {
        yield return new WaitForSeconds(0.45f);
        if (SceneManager.GetSceneByName("LoadingScreen").isLoaded)
            SceneManager.UnloadSceneAsync("LoadingScreen");

        Debug.Log("Unloaded scene");
    }


    //==Image Storage/Recall==//
    IEnumerator GetFiles()
    {
        //loadingScreen.gameObject.SetActive(true);
        //if (SceneManager.GetActiveScene().name != "LoadingScreen")
        //    SceneManager.LoadSceneAsync("LoadingScreen", LoadSceneMode.Additive);

        yield return new WaitForSeconds(0.15f);
        storedSigils.Clear();
        sigilList.ClearOptions();

        List<Dropdown.OptionData> tempOptionData = new List<Dropdown.OptionData>();

        string info = Application.persistentDataPath + "/Sigils/";
        string[] fileInfo = Directory.GetFiles(info, "*.png");
        for (int i = 0; i < fileInfo.Length; i++)
        {
            string url = "file://" + fileInfo[i];

            WWW www = new WWW(url);

            // Wait for download to complete
            yield return www;

            string fileName = Path.GetFileNameWithoutExtension(fileInfo[i]);

            if (!storedSigils.ContainsKey(fileName))
                storedSigils.Add(fileName, www.texture);

            Dropdown.OptionData newData = new Dropdown.OptionData();
            newData.text = fileName;
            //newData.image = Sprite.Create(www.texture, new Rect(0,0, www.texture.width, www.texture.height), new Vector2(0,0)); //not needed. storedSigils already has this reference
            tempOptionData.Add(newData);
        }

        foreach (Dropdown.OptionData optionData in tempOptionData)
        {
            sigilList.options.Add(optionData);
        }

        sigilList.value = 0;
        sigilList.RefreshShownValue();
        //loadingScreen.gameObject.SetActive(false);

        //if (SceneManager.GetSceneByName("LoadingScreen").isLoaded)
        //    SceneManager.UnloadSceneAsync("LoadingScreen");
    }

    public void LoadSavedImage ()
    {
        int tempValue = sigilList.value;
        storedImage.texture = storedSigils[sigilList.options[tempValue].text];
        storedImage.gameObject.SetActive(true);
    }

    public void RemoveSavedImage ()
    {
        int tempValue = sigilList.value;
        File.Delete(Application.persistentDataPath + "/Sigils/" + sigilList.options[tempValue].text + ".png");
        storedSigils.Remove(sigilList.options[tempValue].text);
        sigilList.options.RemoveAt(tempValue);
        sigilList.Hide();
        sigilList.RefreshShownValue();
        ClearString();
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
        sigilName.text = ConvertToInt(checkString);
        CheckTriggers();
        storedImage.gameObject.SetActive(false);

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
        screenShotName = "";
        ClearString();

        sigilName.text = checkString;
        foreach (TriggerTest trigger in correctTriggerList)
        {
            trigger.image.color = Color.white;
            //trigger.orderNum.text = "";
            trigger.isCorrect = false;
        }
        CheckTriggers();
    }
}
