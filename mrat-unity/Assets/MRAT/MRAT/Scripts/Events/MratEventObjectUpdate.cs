using System;
using UnityEngine;

namespace MRAT
{
    /// <summary>
    /// MratEvent for handling object tracking information. 
    /// </summary>
    [Serializable]
    public class MratEventObjectUpdate : MratEventSimple, IVisualizable
    {
        public Vector3 Position;
        public Quaternion ObjectRotation;
        public Vector3 Scale;
		public string ObjectName;
	    public int ObjectInstanceId;

        public Vector3 GetVisualizationPosition()
        {
            return Position;
        }

        public Quaternion GetVisualizationRotation()
        {
            return ObjectRotation;
        }

        public Vector3 GetVisualizationScale()
        {
            return Scale;
        }

        public MratEventObjectUpdate(string message = "", MratEventTypes eventType = MratEventTypes.ObjectUpdate) : base(message, eventType)
	    {
	    }

	    public MratEventObjectUpdate(GameObject go, MratEventTypes eventType = MratEventTypes.ObjectUpdate,
		    string message = "") : base(message, eventType)
	    {
		    Position = go.transform.position;
		    ObjectRotation = go.transform.rotation;
            Scale = go.transform.localScale;
		    ObjectName = go.name;
		    ObjectInstanceId = go.GetInstanceID();
	    }

    }
}
