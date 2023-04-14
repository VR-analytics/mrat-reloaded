using Microsoft.MixedReality.Toolkit.Input;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class SurveyController : MonoBehaviour
{
    [Tooltip("No. of Questionnaires and their objects")]
    public GameObject[] Questionnaires;
    [Tooltip("Show the first (or next) questionnaire by pressing a key/button")]
    public bool ActivationByKey = true;
    [Tooltip("Delay to avoid double-clicking actions")]
    public int DelayInSeconds = 2;

    private bool DelayStatus = false;
    private float DelayTimeStamp;
    private int currentQuestionnaireIndex = -1;

    private void Awake()
    {
        currentQuestionnaireIndex = -1;
        foreach (GameObject q in Questionnaires)
            q.SetActive(false);  
    }
    
    void Start()
    {

        foreach (GameObject q in Questionnaires)
            q.SetActive(false);
        if (ActivationByKey)
        {
            InputActionHandler[] keyscontrollers = gameObject.GetComponents<InputActionHandler>();
            foreach (InputActionHandler k in keyscontrollers)
                k.enabled = true;
        }
        else
        {
            InputActionHandler[] keyscontrollers = gameObject.GetComponents<InputActionHandler>();
            foreach (InputActionHandler k in keyscontrollers)
                k.enabled = false;
        }
        currentQuestionnaireIndex = -1;
    }

    public void showNextQuestionnaire()
    {
        if (Questionnaires != null)
        {
            if (Time.time >= DelayTimeStamp)
                DelayStatus = false;

            if (!DelayStatus)
            {
                DelayStatus = true;
                DelayTimeStamp = Time.time + DelayInSeconds;
                if (currentQuestionnaireIndex + 1 > Questionnaires.Length)
                {//Do nothing
                }
                else if (currentQuestionnaireIndex + 1 == Questionnaires.Length)
                {
                    HideCurrentQuestionnaire();
                }
                else
                {
                    if (currentQuestionnaireIndex < 0 || Questionnaires[currentQuestionnaireIndex].activeSelf)
                    {
                        if (currentQuestionnaireIndex >= 0)
                            HideCurrentQuestionnaire();
                        currentQuestionnaireIndex++;
                        ShowCurrentQuestionnaire();
                    }
                    else
                    {
                        ShowCurrentQuestionnaire();
                    }
                }
            }
        }
    }

    public void showPreviousQuestionnaire()
    {
        if (Questionnaires != null)
            if (currentQuestionnaireIndex < 0)
            {
                //Do nothing
            }
            else if (currentQuestionnaireIndex == 0)
            {
                HideCurrentQuestionnaire();
            }
            else
            {
                if (Questionnaires[currentQuestionnaireIndex].activeSelf)
                {
                    if (Time.time >= DelayTimeStamp)
                        DelayStatus = false;
                    if (!DelayStatus)
                    {
                        DelayStatus = true;
                        DelayTimeStamp = Time.time + DelayInSeconds;
                        HideCurrentQuestionnaire();
                        currentQuestionnaireIndex--;
                        ShowCurrentQuestionnaire();
                    }
                }
                else
                {
                    ShowCurrentQuestionnaire();
                }
            }
    }

    public void HideCurrentQuestionnaire()
    {
        if (Questionnaires != null)
            if (currentQuestionnaireIndex>=0 && currentQuestionnaireIndex < Questionnaires.Length)
                Questionnaires[currentQuestionnaireIndex].SetActive(false);
    }

    public void ShowCurrentQuestionnaire()
    {
        if (Questionnaires != null)
            if (currentQuestionnaireIndex >= 0 && currentQuestionnaireIndex < Questionnaires.Length)
                Questionnaires[currentQuestionnaireIndex].SetActive(true);
    }

}
