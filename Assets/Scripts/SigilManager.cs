using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SigilManager : MonoBehaviour {

    public string currentString;

    public Text sigilName;

    // Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void AddChar(char character)
    {
        if (currentString.Length < 10)
        {
            currentString = currentString + character;
            sigilName.text = currentString;
        }
    }

    public void ClearString ()
    {
        currentString = "";
        sigilName.text = currentString;
    }
}
