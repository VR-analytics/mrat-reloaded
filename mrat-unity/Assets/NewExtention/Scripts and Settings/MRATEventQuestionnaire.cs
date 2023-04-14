using System;
using System.Collections.Generic;
using UnityEngine;

namespace MRAT
{
    [Serializable]
    public class MRATEventQuestionnaire : MratEventSimple
    {
        public string QuestionnairName;
        public List<string> QuestionnairAnswers = new List<string>(80);

        public MRATEventQuestionnaire(string message = "", MratEventTypes eventType = MratEventTypes.QuestionnaireSubmission) : base(message, eventType)
        {
        }
        
        public override void CollectDataFromUnity()
        {
            base.CollectDataFromUnity();
            
        }
        
        public void setEventValues(string questionnairName, List<string> questionnairAnswers)
        {
            QuestionnairName = questionnairName.Trim();
            QuestionnairAnswers.AddRange(questionnairAnswers);
        }
    }
}