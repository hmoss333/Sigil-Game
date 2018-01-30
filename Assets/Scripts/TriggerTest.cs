using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TriggerTest : MonoBehaviour {

    //Renderer renderer;
    Image image;
    SigilManager sm;

    public char letter;
    bool triggered = false;
    

    // Use this for initialization
	void Start () {
        sm = GameObject.FindObjectOfType<SigilManager>();

        //renderer = GetComponent<Renderer>();
        //renderer.material.color = Color.white;

        image = GetComponent<Image>();
        image.color = Color.white;
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Line")
        {
            if (!triggered)
                sm.AddChar(letter);

            image.color = Color.red;
            triggered = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.tag == "Line")
        {
            image.color = Color.white;
        }
    }
}
