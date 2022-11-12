using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.IO;
using System.Linq;
using TMPro;

public class SigilManager : MonoBehaviour {

    public string currentString;
    public string checkString;
    public static string screenShotName;

    public TriggerTest[] triggerList;
    public List<TriggerTest> correctTriggerList;
    Dictionary<string, Texture> storedSigils;
    public RawImage storedImage;

    //public AudioSource audioSource;
    //public AudioClip keyboardSound;
    //public AudioClip generateSound;
    //public AudioClip clearSound;
    //public AudioClip selectImageSound;
    //public AudioClip deleteSound;
    //public AudioClip screenshotSound;

    public TMP_InputField inputField;
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


    //==Generate Trigger Order==//
    public void ClearString ()
    {
        currentString = "";
        storedImage.gameObject.SetActive(false);
    }

    string RemoveDuplicateLetters(string testString)
    {
        string tempString = "";

        foreach (char character in testString)
        {
            if (!tempString.Contains(character.ToString()))
                tempString += character;
        }

        return tempString;
    }

    string RemoveVowels(string testString)
    {
        string tempString = "";
        string vowels = "AEIOU";

        foreach (char character in testString)
        {
            if (!vowels.Contains(character.ToString()))
                tempString += character;
        }

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
                    }
                }
            }
        }
    }

    public void TestTriggerValues (string values)
    {
        bool hasMatched = false;
        char[] tempCurrentChars = values.ToCharArray();

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
            SceneManager.LoadSceneAsync("LoadingScreen", LoadSceneMode.Additive);

            StartCoroutine(TakeScreenshot());
            StartCoroutine(GetFiles());
            checkString = "";

            foreach (TriggerTest trigger in correctTriggerList)
            {
                trigger.isCorrect = false;
            }

            StartCoroutine(CloseLoadingScreen());
        }
    }

    IEnumerator TakeScreenshot ()
    {
        yield return new WaitForSeconds(0.1f);
        hrss.TakeHiResShot();
    }

    IEnumerator CloseLoadingScreen()
    {
        yield return new WaitForSeconds(0.45f);
        if (SceneManager.GetSceneByName("LoadingScreen").isLoaded)
            SceneManager.UnloadSceneAsync("LoadingScreen");
    }


    //==Image Storage/Recall==//
    IEnumerator GetFiles()
    {
        yield return new WaitForSeconds(0.15f);
        storedSigils.Clear();
        //sigilList.ClearOptions();

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
    }


    //==Button Inputs==//
    public void LetterButton (string character)
    {
        //PlayAudio(keyboardSound);

        checkString = checkString + character;
        screenShotName = checkString;
    }

    public void GenerateButton ()
    {
        //PlayAudio(generateSound);

        checkString = inputField.text.ToUpper();
        screenShotName = checkString;

        //Dog Easter Egg
        if (checkString == "POOP")
        {
            storedImage.texture = (Texture)Resources.Load("DogHead");
            storedImage.gameObject.SetActive(true);
            checkString = "";
            currentString = "";
            inputField.text = "";
        }
        //Convert text to numbers
        else
        {
            checkString = RemoveDuplicateLetters(checkString).ToUpper();
            checkString = RemoveVowels(checkString).ToUpper();
            checkString = checkString.Replace(" ", "");
            string tempCheckString = "";
            foreach (char c in checkString)
            {
                if (!char.IsPunctuation(c) && !char.IsDigit(c))
                    tempCheckString += c;
            }
            checkString = tempCheckString;
            inputField.text = ConvertToInt(checkString);
            CheckTriggers();
            storedImage.gameObject.SetActive(false);
        }
    }

    public void ClearButton ()
    {
        //PlayAudio(clearSound);

        checkString = "";
        screenShotName = "";
        ClearString();

        inputField.text = "";
        foreach (TriggerTest trigger in correctTriggerList)
        {
            //trigger.image.color = Color.white;
            //trigger.orderNum.text = "";
            trigger.isCorrect = false;
        }
        CheckTriggers();
    }

    //void PlayAudio(AudioClip audioClip)
    //{
    //    audioSource.Stop();
    //    audioSource.clip = audioClip;
    //    audioSource.Play();
    //}
}
