using Microsoft.MixedReality.Toolkit.Input;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class ScenarioController : MonoBehaviour
{
    [Tooltip("No. of Scenarios and their objects")]
    public GameObject[] Scenarios;
    [Tooltip("Enable a Dashboard to display the current Scenario")]
    public bool EnableDashboard = true;
    public GameObject DashboardMenu;
    public TextMeshPro DashboardText;
    [Tooltip("Show the first (or next) questionnaire by pressing a key/button")]
    public bool ActivationByKey = true;
    [Tooltip("Delay to avoid double-clicking actions")]
    public int DelayInSeconds = 2;
    private bool DelayStatus = false;
    private float DelayTimeStamp;
    private int currentScenarioIndex = -1;

    private void Awake()
    {
        currentScenarioIndex = -1;
        foreach (GameObject q in Scenarios)
            q.SetActive(false);
        if (Scenarios.Length > 0)
        {
            currentScenarioIndex = 0;
            ShowCurrentScenario();
            if (EnableDashboard)
                DashboardMenu.SetActive(true);
            else
                DashboardMenu.SetActive(false);
        }
    }

    void Start()
    {
        currentScenarioIndex = -1;
        foreach (GameObject q in Scenarios)
            q.SetActive(false);
        if (Scenarios.Length > 0)
        {
            currentScenarioIndex = 0;
            ShowCurrentScenario();
            if (EnableDashboard)
                DashboardMenu.SetActive(true);
            else
                DashboardMenu.SetActive(false);
        }

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
        
    }

    public void showNextScenario()
    {
        if (Scenarios != null)
        {
            if (Time.time >= DelayTimeStamp)
                DelayStatus = false;

            if (!DelayStatus)
            {
                DelayStatus = true;
                DelayTimeStamp = Time.time + DelayInSeconds;
                if (currentScenarioIndex + 1 > Scenarios.Length)
                {//Do nothing
                }
                else if (currentScenarioIndex + 1 == Scenarios.Length)
                {
                    HideCurrentScenario();
                }
                else
                {
                    if (currentScenarioIndex < 0 || Scenarios[currentScenarioIndex].activeSelf)
                    {
                        if (currentScenarioIndex >= 0)
                            HideCurrentScenario();
                        currentScenarioIndex++;
                        ShowCurrentScenario();
                    }
                    else
                    {
                        ShowCurrentScenario();
                    }
                }
            }
        }
    }

    public void showPreviousScenario()
    {
        if (Scenarios != null)
            if (currentScenarioIndex < 0)
            {
                //Do nothing
            }
            else if (currentScenarioIndex == 0)
            {
                HideCurrentScenario();
            }
            else
            {
                if (Scenarios[currentScenarioIndex].activeSelf)
                {
                    if (Time.time >= DelayTimeStamp)
                        DelayStatus = false;
                    if (!DelayStatus)
                    {
                        DelayStatus = true;
                        DelayTimeStamp = Time.time + DelayInSeconds;
                        HideCurrentScenario();
                        currentScenarioIndex--;
                        ShowCurrentScenario();
                    }
                }
                else
                {
                    ShowCurrentScenario();
                }
            }
    }

    public void HideCurrentScenario()
    {
        if (Scenarios != null)
            if (currentScenarioIndex >= 0 && currentScenarioIndex < Scenarios.Length)
                Scenarios[currentScenarioIndex].SetActive(false);
    }

    public void ShowCurrentScenario()
    {
        if (Scenarios != null)
            if (currentScenarioIndex >= 0 && currentScenarioIndex < Scenarios.Length)
            {
                Scenarios[currentScenarioIndex].SetActive(true);
                if(EnableDashboard)
                {
                    DashboardText.SetText(Scenarios[currentScenarioIndex].name.Trim().ToString());
                }
                else
                {
                    DashboardText.SetText("");
                }
            }
    }

    public string GetCurrentScenarioName()
    {
        if (Scenarios != null)
            if (currentScenarioIndex >= 0 && currentScenarioIndex < Scenarios.Length)
                return Scenarios[currentScenarioIndex].gameObject.name.Trim().ToString();
        return "NotApplicable";
    }

}
