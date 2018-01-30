using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwipeTrail : MonoBehaviour {

    //Built using these guides:
    //https://www.youtube.com/watch?v=cHVZ0SYIHkI
    //https://www.youtube.com/watch?v=xlwuGKTyJBs&feature=youtu.be

    public GameObject trailPrefab;
    GameObject thisTrail;
    Vector3 startPos;
    Plane objPlane;

    TrailRenderer existingLines;
    public List<TrailRenderer> lines;

    SigilManager sm;

    // Use this for initialization
    void Start () {
        objPlane = new Plane(Camera.main.transform.forward *= 1, this.transform.position);

        sm = GameObject.FindObjectOfType<SigilManager>();
	}
	
	// Update is called once per frame
	void Update () {
		if (((Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began) ||
            Input.GetMouseButtonDown(0)))
        {
            DestroyLines();

            thisTrail = (GameObject)Instantiate(trailPrefab, 
                                                    this.transform.position, 
                                                    Quaternion.identity);
            Ray mRay = Camera.main.ScreenPointToRay(Input.mousePosition);
            float rayDistance;
            if (objPlane.Raycast(mRay, out rayDistance))
                startPos = mRay.GetPoint(rayDistance);
        }
        else if (((Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Moved) || 
            Input.GetMouseButton(0)))
        {
            Ray mRay = Camera.main.ScreenPointToRay(Input.mousePosition);
            float rayDistance;
            if (objPlane.Raycast(mRay, out rayDistance))
                thisTrail.transform.position = mRay.GetPoint(rayDistance);
        }
        else if ((Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Ended) || 
            Input.GetMouseButtonUp(0))
        {
            if (Vector3.Distance(thisTrail.transform.position, startPos) < 0.1f)
                Destroy(thisTrail);
        }
	}

    void DestroyLines ()
    {
        sm.ClearString();
        lines.Clear();

        existingLines = GameObject.FindObjectOfType<TrailRenderer>();
        lines.Add(existingLines);

        foreach (TrailRenderer line in lines)
        {
            Destroy(line);
        }
    }
}
