using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class Questions : MonoBehaviour
{
    public List<string> iQuestions = new List<string>();
    public List<List<int>> Answers = new List<List<int>>();
    public int numEnvironments = 3;

    public int currentQuiz = 0;
    // Start is called before the first frame update
    void Start()
    {
        Answers.Add(new List<int>());
        Answers.Add(new List<int>());
        Answers.Add(new List<int>());
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void QuizFinished()
    {
        currentQuiz++;
      //  GameObject.Find("CanvasManager").GetComponent<CanvasManager>().TransitionTo(CanvasManager.Mode.None);

    }

    public void RecordValue()
    {
        Answers[currentQuiz].Add((int)GameObject.Find("Slider").GetComponent<Slider>().value);
        GameObject.Find("Slider").GetComponent<Slider>().value = 4;
    }

    public bool isLastQuiz()
    {
        return currentQuiz == numEnvironments - 1;
    }

    public bool isDone()
    {
        return currentQuiz >= numEnvironments;
    }

    public void WriteOut()
    {
        string path = Path.Combine(Application.persistentDataPath, ("QuestionnaireAnswers" + System.DateTime.Now.ToString("yyyy_MM_dd_HH_mm_ss") + ".csv"));
        StreamWriter writer = new StreamWriter(path);
        foreach (List<int> answers in Answers)
        {
            string line = "";
            foreach (int answer in answers)
            {
                line = line + answer.ToString() + ", ";
            }
            writer.WriteLine(line);
        }
        writer.Flush();
        writer.Close();
    }
}
