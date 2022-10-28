using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.MixedReality.Toolkit;
using Microsoft.MixedReality.Toolkit.Input;
using UnityEngine;
using Debug = UnityEngine.Debug;

namespace MRAT
{
    public class MratGlobalInputHandler : MonoBehaviour, IMixedRealityPointerHandler, IMixedRealitySourceStateHandler
    {
	    public bool LogClickEvents = true;
	    public bool LogSourceEvents = true;
	    public bool LogFocusEvents = true;
	    public bool CaptureScreenshotOnClick = true;
		public bool CapturePhotoToFileOnClick = true;
        public bool SavePhotosToServer = true;

	    private MratCommunicationManager _commMng;
	    private MratPhotoRecorder _recorder;

	    private readonly List<MratInputSource> _mratInputSources = new List<MratInputSource>(2);

		public void Start()
        {
	        _commMng = MratHelpers.GetMratCommunicationManager();
	        _recorder = MratHelpers.GetMratPhotoRecorder();

            if (!CoreServices.InputSystem.EventListeners.Contains(gameObject))
            {
               CoreServices.InputSystem.EventListeners.Add(gameObject);
            }
        }

        private GameObject _objectInFocus = null;

        public void Update()
            {
            if(LogFocusEvents)
            { 
                var currentfocusobject = CoreServices.InputSystem.FocusProvider.GetFocusedObject(CoreServices.InputSystem.GazeProvider.GazePointer);
            if (currentfocusobject != null)
            {

                if (_objectInFocus == null)
                {
//                    Debug.Log("EnterFocus" + currentfocusobject);
                    OnFocusEnter(currentfocusobject);
                }
                else if (_objectInFocus.GetInstanceID() != currentfocusobject.GetInstanceID())
                {
//                    Debug.Log("ExitFocus" + _objectInFocus);

                    OnFocusExit(_objectInFocus);
                    _objectInFocus = null;
                }
            }
            else if(_objectInFocus!=null)
            {
//                Debug.Log("ExitFocus" + _objectInFocus);
                OnFocusExit(_objectInFocus);
                _objectInFocus = null;
            }
            }
        }
        
		public void PhotoReceiver(MratEventPhotoCaptured photoEvent)
		{
			Debug.Log(Time.realtimeSinceStartup + " PhotoReceiver - filename: " + photoEvent.ImageFileName);
			_commMng.LogMratEvent(photoEvent);

            if (SavePhotosToServer) _commMng.LogMratImageEvent(photoEvent);
		}

		public void OnFocusEnter(GameObject go)
		{
			if (!LogFocusEvents) return;

			var focusEvent = new MratEventStatusUpdate("", MratEventTypes.FocusStart);
			focusEvent.CollectDataFromUnity();

			_commMng.LogMratEvent(focusEvent);
		}

		public void OnFocusExit(GameObject go)
		{
			if (!LogFocusEvents) return;

			var focusEvent = new MratEventStatusUpdate("", MratEventTypes.FocusStop);
			focusEvent.CollectDataFromUnity();

			_commMng.LogMratEvent(focusEvent);
		}

		void IMixedRealitySourceStateHandler.OnSourceDetected(SourceStateEventData eventData)
		{
            if (!LogSourceEvents) return;
            var inputSource = new MratInputSource(eventData.InputSource, eventData.SourceId);

			if (_mratInputSources.All(src => src.SourceId != inputSource.SourceId))
			{
				_mratInputSources.Add(inputSource);
			}

			var inputEvent = new MratEventInput(inputSource, MratEventTypes.InputSourceDetected);
			inputEvent.CollectDataFromUnity();

			_commMng.LogMratEvent(inputEvent);
		}

		void IMixedRealitySourceStateHandler.OnSourceLost(SourceStateEventData eventData)
		{
			if (!LogSourceEvents) return;
            
			var timestamp = MratHelpers.GetTimestamp();

			MratInputSource inputSource = null;

			for (var i = 0; i < _mratInputSources.Count; i++)
			{
				if (_mratInputSources[i].SourceId != eventData.SourceId) continue;

				inputSource = _mratInputSources[i];

				_mratInputSources.RemoveAt(i);
				
				break;
			}

			if (inputSource == null)
			{
				inputSource = new MratInputSource(eventData.InputSource, eventData.SourceId);
			}
			else if (inputSource.InputClickCount <= 0 && _mratInputSources.All(s => s.InputClickCount <= 0))
			{
				var noClickEvent = new MratEventInput(inputSource, MratEventTypes.InputNoClick);
				noClickEvent.CollectDataFromUnity();
				_commMng.LogMratEvent(noClickEvent);
			}

			inputSource.SourceLost(timestamp);

			var inputEvent = new MratEventInput(inputSource, MratEventTypes.InputSourceLost);
			inputEvent.CollectDataFromUnity();

			_commMng.LogMratEvent(inputEvent);
		}

        public void OnPointerDown(MixedRealityPointerEventData eventData)
        {
        }

        public void OnPointerDragged(MixedRealityPointerEventData eventData)
        {
          }

        public void OnPointerUp(MixedRealityPointerEventData eventData)
        {
        }

        public void OnPointerClicked(MixedRealityPointerEventData eventData)
        {
  //          if(eventData.selectedObject!=null)
  //          Debug.Log("Event:" + eventData.selectedObject);
            if (!LogSourceEvents && !LogClickEvents) return;
            
            var inputSource = _mratInputSources.FirstOrDefault() ?? new MratInputSource(eventData.InputSource, eventData.SourceId);

            foreach (var source in _mratInputSources)
            {
                source.InputClickCount += 1;
            }

            if (inputSource.InputClickCount <= 0)
            {
                inputSource.InputClickCount += 1;
            }

            if (LogClickEvents)
            {
                var clickType = eventData.selectedObject != null
                    ? MratEventTypes.InputClickWithTarget
                    : MratEventTypes.InputClickWithoutTarget;

                var inputEvent = new MratEventInput(inputSource, clickType);

                inputEvent.CollectDataFromUnity();

                _commMng.LogMratEvent(inputEvent);
            }
            #if !UNITY_ANDROID

            if (CapturePhotoToFileOnClick) _recorder.TakePictureToFile();
#endif
            if (CaptureScreenshotOnClick)
            {
                
                var photoEvent = new MratEventPhotoCaptured("Screenshot", MratEventTypes.Screenshot);
                photoEvent.CollectDataFromUnity();

                var screenshotName = Path.Combine(Application.persistentDataPath, photoEvent.MakeMratFilename());
//                Debug.Log("Testing Capture Screen: "+screenshotName);
                ScreenCapture.CaptureScreenshot(screenshotName);

                _commMng.LogMratEvent(photoEvent);

                if (SavePhotosToServer) _commMng.LogMratImageEvent(photoEvent);
            }
        }
    }
}
