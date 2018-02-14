using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExitButton : MonoBehaviour {

    public GameObject exitPanel;

    bool menuOpen;
    
    // Use this for initialization
	void Start () {
        exitPanel.SetActive(false);
        menuOpen = false;
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    public void Exit ()
    {
        if (!menuOpen)
        {
            exitPanel.SetActive(true);
            menuOpen = true;
        }
    }

    public void Yes ()
    {
        Application.Quit();
    }

    public void No ()
    {
        exitPanel.SetActive(false);
        menuOpen = false;
    }
}
