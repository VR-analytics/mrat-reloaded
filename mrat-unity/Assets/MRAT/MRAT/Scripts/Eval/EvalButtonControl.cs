using Microsoft.MixedReality.Toolkit.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace MRAT
{
	public class EvalButtonControl : MonoBehaviour, IMixedRealityPointerHandler
    {
		public int ChangeObjectCount = 100;

		private EvalObjectManager _evalManager;

        public void OnPointerClicked(MixedRealityPointerEventData eventData)
        {
            _evalManager.MaxObjects += ChangeObjectCount;
            _evalManager.ResetObjects();
        }

        public void OnPointerDown(MixedRealityPointerEventData eventData)
        {
            throw new NotImplementedException();
        }

        public void OnPointerDragged(MixedRealityPointerEventData eventData)
        {
            throw new NotImplementedException();
        }

        public void OnPointerUp(MixedRealityPointerEventData eventData)
        {
            throw new NotImplementedException();
        }

        private void Awake()
		{
			_evalManager = FindObjectOfType<EvalObjectManager>();
		}

		
	}
}
