using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExitButton : MonoBehaviour {

    public GameObject exitPanel;

    bool menuOpen;

    public AudioSource audioSource;
    public AudioClip exitSound;
    public AudioClip returnSound;
    
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
            PlayAudio(exitSound);
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
        PlayAudio(returnSound);
        exitPanel.SetActive(false);
        menuOpen = false;
    }

    void PlayAudio(AudioClip audioClip)
    {
        audioSource.Stop();
        audioSource.clip = audioClip;
        audioSource.Play();
    }
}
