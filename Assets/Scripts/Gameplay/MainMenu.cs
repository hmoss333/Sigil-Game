using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEditor;

public class MainMenu : MonoBehaviour
{
    [SerializeField] GameObject MainMenuParent, AboutMenu;
    [SerializeField] TMP_Text versionNum;
    [SerializeField] string buildNum;

    private void Start()
    {
        versionNum.text = $"v{Application.version}.{buildNum}";

        MainMenuParent.SetActive(true);
        AboutMenu.SetActive(false);

        //PlayerPrefs.DeleteAll(); //temp
        if (PlayerPrefs.GetInt("firstLaunch", 0) == 0)
        {
            AboutButton();
        }
    }

    public void AboutButton()
    {
        if (AboutMenu.activeSelf && PlayerPrefs.GetInt("firstLaunch") == 0)
        {
            PlayerPrefs.SetInt("firstLaunch", 1);

            SceneManager.LoadSceneAsync("LoadingScreen");
            SceneManager.LoadScene("Create");
        }

        MainMenuParent.SetActive(!MainMenuParent.activeSelf);
        AboutMenu.SetActive(!AboutMenu.activeSelf);    
    }

    public void CreateButton()
    {
        SceneManager.LoadSceneAsync("LoadingScreen", LoadSceneMode.Additive);
        SceneManager.LoadSceneAsync("Create");
    }

    public void ViewButton()
    {
        SceneManager.LoadSceneAsync("LoadingScreen", LoadSceneMode.Additive);
        SceneManager.LoadSceneAsync("Gallery");
    }
}
