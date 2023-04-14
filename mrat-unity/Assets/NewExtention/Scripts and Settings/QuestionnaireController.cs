using MRAT;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using TMPro;
using UnityEngine;


public class QuestionnaireController : MonoBehaviour
{
    [Tooltip("No. of Questions and their objects")]
    public GameObject[] Questions;
    [Tooltip("Delay to avoid double-clicking actions")]
    public int DelayInSeconds = 3;
    public bool LocalLogging = true;
    public bool ExportToFile = true;
    public bool ExportViaMRATEventSystem = false;

    [Tooltip("Enable the user to auto transfer into the next Scenario after pressing the Submit button")]
    public bool AutoScenarioTransfer = true;

    private GameObject ScenarioControllerObj;
    private int currentQuestionIndex = -1;
    private bool DelayStatus = false;
    private float DelayTimeStamp;
    private static string AppTimeStamp = System.DateTime.Now.ToString("yyyy_MM_dd_HH_mm_ss");
    //private MratCommunicationManager _commManager;

    private void Awake()
    {
        foreach (GameObject q in Questions)
            q.SetActive(false);
        if (AutoScenarioTransfer)
            ScenarioControllerObj = GameObject.FindGameObjectWithTag("ScenarioController");

    }

    void Start()
    {
        if(ExportViaMRATEventSystem)
        {

       //     _commManager = MratHelpers.GetMratCommunicationManager();
        }
        foreach (GameObject q in Questions)
            q.SetActive(false);
        if (AutoScenarioTransfer)
            ScenarioControllerObj = GameObject.FindGameObjectWithTag("ScenarioController");

        if (Questions.Length > 0)
        {
            Questions[0].SetActive(true);
            currentQuestionIndex = 0;
        }
    }

    public void showNextQuestion()
    {
        if (Questions != null)
        {

            if (currentQuestionIndex + 1 >= Questions.Length)
            {//Do nothing
            }
            else
            {
                if (Time.time >= DelayTimeStamp)
                    DelayStatus = false;

                if (!DelayStatus)
                {
                    DelayStatus = true;
                    DelayTimeStamp = Time.time + DelayInSeconds;
                    Questions[currentQuestionIndex].SetActive(false);
                    currentQuestionIndex++;
                    Questions[currentQuestionIndex].SetActive(true);
                }

            }

        }
    }

    public void showPreviousQuestion()
    {
        if (Questions != null)
            if (currentQuestionIndex <= 0)
            {//Do nothing
            }
            else
            {
                if (Time.time >= DelayTimeStamp)
                    DelayStatus = false;
                if (!DelayStatus)
                {
                    DelayStatus = true;
                    DelayTimeStamp = Time.time + DelayInSeconds;
                    Questions[currentQuestionIndex].SetActive(false);
                    currentQuestionIndex--;
                    Questions[currentQuestionIndex].SetActive(true);
                }
            }
    }

    public void submitAnswers()
    {
        string Answers = "";
        if (Questions != null)
        {
            if (Time.time >= DelayTimeStamp)
                DelayStatus = false;
            if (!DelayStatus)
            {
                for (int i = 0; i < Questions.Length; i++)
                {
                    Answers += Questions[i].transform.Find("AnswerValue").GetComponent<TextMeshPro>().text.ToString();
                    if (i != Questions.Length - 1)
                        Answers += ",";
                }
                ExportAnswers(Answers);
                Questions[currentQuestionIndex].SetActive(false);
                
                if (AutoScenarioTransfer && ScenarioControllerObj != null)
                {
                    ScenarioControllerObj.GetComponent<ScenarioController>().showNextScenario();
                }
            }
        }
    }

    public void ExportAnswers(string Answers)
    {
        string questionnaireName = this.name;
        if (AutoScenarioTransfer && ScenarioControllerObj != null)
        {
            questionnaireName = ScenarioControllerObj.GetComponent<ScenarioController>().GetCurrentScenarioName() + "-" + questionnaireName;
        }

        if (LocalLogging)
        {
            Debug.Log("localLogging: " + questionnaireName + ", answers:" + Answers);
        }

        if (ExportToFile)
        {
            string path = Path.Combine(Application.persistentDataPath, ("Questionnaires_Answers" + AppTimeStamp + ".csv"));
            StreamWriter writer = new StreamWriter(path, true);
            writer.WriteLine(questionnaireName + "," + Answers);
            writer.Flush();
            writer.Close();
        }

        if (ExportViaMRATEventSystem)
        {
            var e = new MRATEventQuestionnaire("QuestionnaireSubmitted");
            e.CollectDataFromUnity();
            e.setEventValues(questionnaireName, Answers.Split(',').ToList());
            MratCommunicationManager.Instance.LogMratEvent(e);
        }
    }

}
