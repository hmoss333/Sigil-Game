﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SplashScreen : MonoBehaviour {

    public string sceneToLoad;
    
    // Use this for initialization
	void Start () {
        //This is where you will run any splash screen animation stuff
        //Handheld.PlayFullScreenMovie("ZyroGamesTrailer.mp4", Color.black, FullScreenMovieControlMode.Hidden, FullScreenMovieScalingMode.Fill);

        SceneManager.LoadSceneAsync("LoadingScreen", LoadSceneMode.Additive);
        SceneManager.LoadSceneAsync(sceneToLoad);
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
