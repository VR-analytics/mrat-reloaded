using Microsoft.MixedReality.Toolkit.Utilities;
using System.Collections.Generic;

using UnityEngine;

namespace MRAT
{
    public class MratStatusTracker : MonoBehaviour
    {
        /// <summary>
        /// Recording interval as a floating point, measured in seconds.
        /// </summary>
        public float IntervalSeconds = 1.0f;
	    public int FpsWarningLevel = 50;
	    public bool LogPerformanceWarnings = true;
        public bool ReportingFPSReadings = true;
        public bool ReportingMemoryReadings = true;
        [HideInInspector]
        public float TotalUserDistanceTravelled;

        private Vector3 _lastUserPosition = Vector3.zero;

	    private MratCommunicationManager _commManager;

	    private readonly List<float> _fpsReadings = new List<float>(80);
	    private readonly List<long> _memoryReadings = new List<long>(80);

        //New Extension QQ - begin
        public bool ReportingScenarioName = false;
        public bool ReportingDetectedTaggedObjects = false;
        [Tooltip("Searchable Detection Tag")]
        public string SearchableTag = "DetectableObject";
        [Tooltip("Detection Radius")]
        public float DetectionRadius = 10;

        private static GameObject PlayerObject;
        private static ScenarioController ScenarioControllerComponent;
        private List<string> _detectedTaggedObjects = new List<string>(80);
        private string _currentScenario="";
        void Awake()
        {
            PlayerObject = GameObject.FindGameObjectWithTag("MainCamera");
            GameObject ScenarioControllerObj = GameObject.FindGameObjectWithTag("ScenarioController");
            if (ScenarioControllerObj != null)
                ScenarioControllerComponent = ScenarioControllerObj.GetComponent<ScenarioController>();
        }

        private List<string> detectTaggedObjects()
        {
            List<string> _detectedTaggedObjects = new List<string>(80);
            Collider[] hitColliders = Physics.OverlapSphere(PlayerObject.transform.position, DetectionRadius);
            int i = 0;
            while (i < hitColliders.Length)
            {
                if (hitColliders[i].tag == SearchableTag)
                {
                    if (_detectedTaggedObjects.Count < 80)
                        _detectedTaggedObjects.Add(hitColliders[i].gameObject.name);
                }
                i++;
            }
            return _detectedTaggedObjects;
        }
        //New Extension QQ - end

        private void Start()
	    {
		    _commManager = MratHelpers.GetMratCommunicationManager();

            if(ReportingDetectedTaggedObjects)
            {
                GameObject []taggedobjects= GameObject.FindGameObjectsWithTag(SearchableTag);
                if(taggedobjects!=null && taggedobjects.Length>0)
                {
                    for(int i=0;i<taggedobjects.Length;i++)
                    {
                        if(!taggedobjects[i].TryGetComponent<Collider>(out Collider x))
                        {
                            taggedobjects[i].AddComponent<MeshCollider>();
                        }
                    }
                }
            }

            InvokeRepeating(nameof(ReportStatusUpdate), 1.0f, IntervalSeconds);


        }

        private void Update()
        {
	        var dist = Vector3.Distance(CameraCache.Main.transform.position, _lastUserPosition);
	        TotalUserDistanceTravelled += dist;

			_lastUserPosition = CameraCache.Main.transform.position;

	        _fpsReadings.Add(1.0f / Time.deltaTime);
	        _memoryReadings.Add(System.GC.GetTotalMemory(false));

            if (ScenarioControllerComponent != null)
                _currentScenario = ScenarioControllerComponent.GetCurrentScenarioName();

        }

        public void ReportStatusUpdate()
		{
			var e = new MratEventStatusUpdate();

			e.CollectDataFromUnity();
            if(ReportingFPSReadings)
    	        e.AddFpsReadings(_fpsReadings);
            if(ReportingMemoryReadings)
    	        e.AddMemoryReadings(_memoryReadings);

            //New Extension QQ - begin
            if(ReportingScenarioName)
            {
                    e.CurrentScenario = _currentScenario;
            }
            if (ReportingDetectedTaggedObjects)
            {
                
                e.AddDetectedTaggedObjects(detectTaggedObjects());
            }
            
            //New Extension QQ - end
            
            _fpsReadings.Clear();
			_memoryReadings.Clear();
			_commManager.LogMratEvent(e);
			if (LogPerformanceWarnings && e.FpsWarningFlag) _commManager.LogMratEvent(e.CreatePerformanceWarningEvent());
		}

    }
}
