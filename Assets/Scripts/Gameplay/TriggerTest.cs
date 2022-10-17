using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TriggerTest : MonoBehaviour {

    //Renderer renderer;
    //[System.NonSerialized] public Image image;
    SigilManager sm;
    public Text orderNum;

    public string letters;
    public bool isCorrect = false;
    public bool triggered = false;
    

    // Use this for initialization
	void Start () {
        sm = GameObject.FindObjectOfType<SigilManager>();

        //renderer = GetComponent<Renderer>();
        //renderer.material.color = Color.white;

        //image = GetComponent<Image>();
        //image.color = Color.white;
        //orderNum.text = "";
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Line")
        {
            //image.color = Color.red;
            if (isCorrect && !triggered)
            {
                sm.TestTriggerValues(letters);
                triggered = true;
            }
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.tag == "Line")
        {
            triggered = false;
        }
    }
}
