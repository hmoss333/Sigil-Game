using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerTest : MonoBehaviour {

    Renderer renderer;
    

    // Use this for initialization
	void Start () {
        renderer = GetComponent<Renderer>();
        renderer.material.color = Color.white;
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Line")
        {
            renderer.material.color = Color.red;
            Debug.Log("tiggering");
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Line")
        {
            renderer.material.color = Color.white;
            Debug.Log("not triggering");
        }
    }
}
