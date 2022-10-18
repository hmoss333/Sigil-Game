using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    [SerializeField] GameObject MainMenuParent, AboutMenu;

    private void Start()
    {
        MainMenuParent.SetActive(true);
        AboutMenu.SetActive(false);
    }

    public void AboutButton()
    {
        MainMenuParent.SetActive(!MainMenuParent.activeSelf);
        AboutMenu.SetActive(!AboutMenu.activeSelf);
    }

    public void CreateButton()
    {
        SceneManager.LoadSceneAsync("LoadingScreen", LoadSceneMode.Additive);
        SceneManager.LoadSceneAsync("Test_New");
    }

    public void ViewButton()
    {
        //TODO open the local image viewer application and redirect to the Sigil folder
        ///Research the best way to manually save all generated sigils into a viewable format on the local device
    }

    public void ExitButton()
    {
        Application.Quit();
    }
}
