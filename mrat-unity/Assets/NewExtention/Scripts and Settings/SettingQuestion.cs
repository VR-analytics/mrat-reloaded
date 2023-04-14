using Microsoft.MixedReality.Toolkit.UI;
using TMPro;
using UnityEngine;

public class SettingQuestion : MonoBehaviour
{
    public string QuestionText="Question?";
    public string guidelineText="";
    public string sliderValue = "0";
    public bool ShowGuidlines = true;
    public bool ShowAnswerValue=true;
    public bool ShowSlider = true;
    public bool ShowBackButton = true;
    public bool ShowNextButton = true;
    public bool ShowSubmitButton = false;
    public TextMeshPro QustionTextObj;
    public TextMeshPro QuestionGuidelineObj;
    public TextMeshPro AnswerValueObj;
    public GameObject SliderObj;
    public GameObject BackButton;
    public GameObject NextButton;
    public GameObject SubmitButton;
    

    void Start()
    {
        QustionTextObj.SetText(QuestionText);
        QuestionGuidelineObj.SetText(guidelineText);
        QuestionGuidelineObj.enabled = ShowGuidlines;
        AnswerValueObj.SetText(sliderValue);
        AnswerValueObj.enabled = ShowAnswerValue;
        SliderObj.SetActive(ShowSlider);
        BackButton.SetActive(ShowBackButton);
        NextButton.SetActive(ShowNextButton);
        SubmitButton.SetActive(ShowSubmitButton);
        BackButton.GetComponent<Interactable>().OnClick.AddListener(() => { transform.parent.GetComponent< QuestionnaireController>().showPreviousQuestion(); });
        NextButton.GetComponent<Interactable>().OnClick.AddListener(() => { transform.parent.GetComponent<QuestionnaireController>().showNextQuestion(); });
        SubmitButton.GetComponent<Interactable>().OnClick.AddListener(() => { transform.parent.GetComponent<QuestionnaireController>().submitAnswers(); });
    }

    
}
