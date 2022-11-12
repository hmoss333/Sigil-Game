using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransformPulse : MonoBehaviour
{
    [SerializeField] private Transform pulseTransform;
    private float range;
    [SerializeField] private float rangeMax;
    [SerializeField] private float rangeSpeed;


    // Update is called once per frame
    void Update()
    {    
        range -= rangeSpeed * Time.deltaTime;
        if (range < 0)//> rangeMax)
        {
            //range = 0f;
            range = rangeMax;
        }
        pulseTransform.localScale = new Vector2(range, range);
    }
}
