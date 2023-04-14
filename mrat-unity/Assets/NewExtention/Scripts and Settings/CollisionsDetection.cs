using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionsDetection : MonoBehaviour
{
    [Tooltip("Searchable Detection Tag")]
    public string SearchableTag = "DetectableObject";
    [Tooltip("Detection Radius")]
    public float DetectionRadius=5;
    [Tooltip("Detection Timer")]
    public float TimerInSeconds = 1;
    private bool TimerStatus=false;
    private float TimerTimeStamp;
    private static string AppTimeStamp = System.DateTime.Now.ToString("yyyy_MM_dd_HH_mm_ss");
    private static GameObject PlayerObject;

    void Awake()
    {
        PlayerObject = GameObject.FindGameObjectWithTag("MainCamera");
    }

    void Update()
    {
        if (Time.time >= TimerTimeStamp)
            TimerStatus = false;
        if (!TimerStatus)
        {
            string detectedobjects="\"DetectedObjects\"=[";
            TimerStatus = true;
            TimerTimeStamp = Time.time + TimerInSeconds;
            Collider[] hitColliders = Physics.OverlapSphere(PlayerObject.transform.position, DetectionRadius);
            int i = 0; bool firsthit=true;
            while (i < hitColliders.Length)
            {
                if (hitColliders[i].tag == SearchableTag)
                {
                    if (firsthit)
                    {
                        detectedobjects += "\"" + hitColliders[i].gameObject.name + "\"";
                        firsthit = false;
                    }
                    else
                        detectedobjects = detectedobjects + ",\"" + hitColliders[i].gameObject.name + "\"";

                }
                i++;
            }
            detectedobjects += "]";
            Debug.Log(detectedobjects);
        }
    }
}