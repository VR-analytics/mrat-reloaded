using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Microsoft.MixedReality.Toolkit.UI;
using TMPro;
using Microsoft.MixedReality.Toolkit.Experimental.UI;
using Microsoft.MixedReality.Toolkit.Utilities.Solvers;

namespace Microsoft.MixedReality.Toolkit.Examples.Demos
{
    [AddComponentMenu("Scripts/MRTK/Examples/CustomShowSliderValue")]
    public class CustomShowSliderValue : MonoBehaviour
    {
        [SerializeField]
        private TextMeshPro textMesh = null;

        public GameObject[] MenuList;
        public bool AccessabilityStatusUpdate = false;
        
        public void OnSliderUpdated(SliderEventData eventData)
        {
            if (textMesh == null)
            {
                textMesh = GetComponent<TextMeshPro>();
            }

            if (textMesh != null)
            {
                int divisions=transform.GetComponent<StepSlider>().SliderStepDivisions;
                textMesh.text = $"{eventData.NewValue*divisions+1}";
            }
        }

        public void OnSliderUpdatedAccessabilityMenu(SliderEventData eventData)
        {
            if (textMesh == null)
            {
                textMesh = GetComponent<TextMeshPro>();
            }

            if (textMesh != null)
            {
                int divisions = transform.GetComponent<StepSlider>().SliderStepDivisions;
                textMesh.text = (0.3+(eventData.NewValue * divisions*2)/10).ToString("F1");

                if(AccessabilityStatusUpdate)
                {
                    foreach( GameObject x in MenuList)
                    {
                        RadialView comp = x.transform.GetComponent<RadialView>();
                        float dist = float.Parse(textMesh.text);
                        comp.MinDistance = dist;
                        comp.MaxDistance = dist;
                        Debug.Log(dist);
                        Debug.Log(comp.MinDistance);
                    }
                }
            }
        }
    }
}

