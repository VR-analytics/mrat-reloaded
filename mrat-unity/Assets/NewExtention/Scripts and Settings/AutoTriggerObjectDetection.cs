using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class AutoTriggerObjectDetection : MonoBehaviour
{
    [Tooltip("activate the distance-rule condition")]
    public bool enableDistanceTrigger = true;
    [Tooltip("activate the timer-rule condition")]
    public bool enableTimerTrigger = true;
    [Tooltip("Minimum Distance between the player and the object")]
    public float minDistance=10;
    [Tooltip("Timer value in Seconds (0.000)")]
    public float timerValue = 10;
    [Tooltip("Select the target object and function")]
    public UnityEvent TagetedTriggerFunction;

    private static GameObject PlayerObject;
    private float StartTimeStamp = 0;
    private bool enableTimerCheck = false;
    void Awake()
    {
        PlayerObject = GameObject.FindGameObjectWithTag("MainCamera");
    }

    private void Start()
    {
        enableTimerCheck = false;
        if (!enableDistanceTrigger&&enableTimerTrigger)
        {
            StartTimeStamp = Time.time;
            enableTimerCheck = true;
        }

    }
    
    void Update()
    {
        if (enableDistanceTrigger)
        {
            float currDist = Vector3.Distance(transform.position, PlayerObject.transform.position);
            if (currDist < minDistance)
            {
                if (enableTimerTrigger)
                {
                    enableTimerCheck = true;
                    enableDistanceTrigger = false;
                    StartTimeStamp = Time.time;
                }
                else
                {
                    // call the required function.
                    CallTargetedFunction();
                    // disable the current script
                    this.enabled = false;
                }
            }
            else if (currDist > minDistance)
            {
                // do nothing
            }
        }
        
        
        if(enableTimerCheck)
        {
            if(StartTimeStamp+timerValue<=Time.time)
            {
                // call the required function.
                CallTargetedFunction();
                // disable the current script
                this.enabled = false;
            }
        }

    }

    void CallTargetedFunction()
    {
        TagetedTriggerFunction.Invoke();
    }


}
