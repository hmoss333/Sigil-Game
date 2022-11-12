using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PageSwiper : MonoBehaviour, IDragHandler, IEndDragHandler
{
    [Header("Sliding Values")]
    public float percentThreshold = 0.2f;
    public float easing = 0.5f;
    int currentChild;
    public bool showSlide;
    private Vector3 panelLocation;
    private Vector3 initPos;


    [Header("Auto Cycle Values")]
    public bool autoCycle;
    public float tileTimer = 5.0f;
    Coroutine ac;


    //[Header("Resources Values")]
    //public string dotFolderPath;
    //public Image sliderSpots;


    // Start is called before the first frame update
    void OnEnable()
    {
        currentChild = 0;

        if (initPos != Vector3.zero && transform.position != initPos)
            transform.position = initPos;

        panelLocation = transform.position;
        initPos = panelLocation;

        easing = showSlide ? easing : 0f;

        if (autoCycle)
            ac = StartCoroutine(AutoCycle(tileTimer));

        //UpdateSpotSlider(currentChild);
    }

    public void OnDrag(PointerEventData data)
    {
        float difference = data.pressPosition.x - data.position.x;

        if (showSlide)
            transform.position = panelLocation - new Vector3(difference, 0, 0);

        if (autoCycle)
            StopCoroutine(ac); //Stop the current autocycle timer
    }

    public void OnEndDrag(PointerEventData data)
    {
        float percentage = (data.pressPosition.x - data.position.x) / Screen.width;
        if (Mathf.Abs(percentage) >= percentThreshold)
        {
            Vector3 newLocation = panelLocation;
            if (percentage > 0 && currentChild < transform.childCount - 1)
            {
                currentChild++;
                newLocation += new Vector3(-Screen.width, 0, 0);
            }
            else if (percentage < 0 && currentChild > 0)
            {
                currentChild--;
                newLocation += new Vector3(Screen.width, 0, 0);
            }
            //transform.position = newLocation;
            StartCoroutine(SmoothMove(transform.position, newLocation, easing));
            panelLocation = newLocation;
        }
        else
        {
            //transform.position = panelLocation;
            StartCoroutine(SmoothMove(transform.position, panelLocation, easing));
        }

        //UpdateSpotSlider(currentChild);

        if (autoCycle)
            ac = StartCoroutine(AutoCycle(tileTimer)); //Start a new autocycle timer; this avoids the tiles changing right away if you swipe late
    }

    IEnumerator SmoothMove(Vector3 startpos, Vector3 endpos, float seconds)
    {
        float t = 0.0f;
        while (t <= 1.0f)
        {
            t += Time.fixedDeltaTime / seconds;
            transform.position = Vector3.Lerp(startpos, endpos, Mathf.SmoothStep(0f, 1f, t));
            yield return null;
        }
    }

    IEnumerator AutoCycle(float seconds)
    {
        while (autoCycle)
        {
            yield return new WaitForSeconds(seconds);
           
            if (currentChild < transform.childCount - 1)
            {
                Vector3 newLocation = panelLocation;
                currentChild++;
                newLocation += new Vector3(-Screen.width, 0, 0);
                StartCoroutine(SmoothMove(transform.position, newLocation, easing));
                panelLocation = newLocation;
            }
            else
            {
                currentChild = 0;
                StartCoroutine(SmoothMove(transform.position, initPos, easing));
                panelLocation = initPos;
            }

            //UpdateSpotSlider(currentChild);
        }
    }

    //void UpdateSpotSlider(int panelNum)
    //{
    //    //string path = "Main/Feed/Dots/" + panelNum;
    //    sliderSpots.sprite = Resources.Load<Sprite>(dotFolderPath + panelNum.ToString());
    //}
}
