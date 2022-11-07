using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class MainMenu : MonoBehaviour
{
    [SerializeField] GameObject MainMenuParent, AboutMenu;
    [SerializeField] TMP_Text versionNum;

    private void Start()
    {
        versionNum.text = $"v{Application.version}";

        MainMenuParent.SetActive(true);
        AboutMenu.SetActive(false);

        if (PlayerPrefs.GetInt("firstLaunch", 0) == 0)
        {
            AboutButton();
            PlayerPrefs.SetInt("firstLaunch", 1);
        }
    }

    public void AboutButton()
    {
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
        //TODO open the local image viewer application and redirect to the Sigil folder
        ///Research the best way to manually save all generated sigils into a viewable format on the local device
    }
}
