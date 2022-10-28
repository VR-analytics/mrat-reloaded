using System;
using UnityEngine;
using Microsoft.MixedReality.Toolkit.Input;
using Microsoft.MixedReality.Toolkit;

namespace MRAT
{
	[Serializable]
	public class MratInputSource
	{
		public uint SourceId;

		[NonSerialized]
		public IMixedRealityInputSource Source;

		public InputSourceType SourceKind;
		public Vector3 PreviousSourcePosition;
		public Vector3 CurrentSourcePosition;
		public string PositionSide;
		public bool IsTrackable;

		public long SourceDetectedTimestamp;
		public long SourceLostTimestamp;
		public long SourceTrackedDuration;

		public int InputClickCount;

		public MratInputSource()
		{
		}
		
		
        public MratInputSource(IMixedRealityInputSource source, uint sourceId)
        {
            if (source == null) return;

            SourceDetectedTimestamp = MratHelpers.GetTimestamp();
            
            Source = source;
            SourceId = sourceId;

           // if (!Source.TryGetSourceKind(SourceId, out SourceKind)) return;
           
            if (!TryGetSourcePosition(out CurrentSourcePosition)) return;

            IsTrackable = true;
            PreviousSourcePosition = CurrentSourcePosition;

            PositionSide = GetSideFromPosition(CurrentSourcePosition);
        }

        public bool TryUpdateSourcePosition()
		{
			Vector3 newPosition;

			if (!TryGetSourcePosition(out newPosition)) return false;

			PreviousSourcePosition = CurrentSourcePosition;
			CurrentSourcePosition = newPosition;

			PositionSide = GetSideFromPosition(CurrentSourcePosition);

			return true;
		}

		public bool TryGetSourcePosition(out Vector3 inputPosition)
		{
			bool result=false;
            inputPosition = Vector3.zero;

            switch (SourceKind)
			{
				case InputSourceType.Hand:
                    if (Source.Pointers.Length > 0)
                    {

                        foreach (IMixedRealityPointer x in Source.Pointers)
                            if (x.PointerId == SourceId)
                            {
                                inputPosition = x.Position;
                                result = true;
                            }
                    }
                                break;
				case InputSourceType.Controller:
                    if (Source.Pointers.Length > 0)
                    {

                        foreach (IMixedRealityPointer x in Source.Pointers)
                            if (x.PointerId == SourceId)
                            {
                                inputPosition = x.Position;
                                result = true;
                            }
                    }
                    break;
				case InputSourceType.Other:
                    if (Source.Pointers.Length > 0)
                    {

                        foreach (IMixedRealityPointer x in Source.Pointers)
                            if (x.PointerId == SourceId)
                            {
                                inputPosition = x.Position;
                                result = true;
                            }
                            else
                            {
                                inputPosition = Vector3.zero;
                                result = false;
                                
                            }
                    }
                    break;
				default:
					inputPosition = Vector3.zero;
					result = false;
					break;
			}

			return result;
		}

		public static string GetSideFromPosition(Vector3 inputPosition)
		{
            // Hand position code adapted from: https://forums.hololens.com/discussion/comment/6264/#Comment_6264
            // Qudamah Quboa - Might need more work
            //var heading = inputPosition - GazeManager.Instance.GazeOrigin;
            //var perp = Vector3.Cross(GazeManager.Instance.GazeTransform.forward, heading);
            //var dot = Vector3.Dot(perp, GazeManager.Instance.GazeTransform.up);
            var heading = inputPosition - CoreServices.InputSystem.GazeProvider.GazeOrigin;
			var perp = Vector3.Cross(CoreServices.InputSystem.GazeProvider.GazeDirection, heading);
			var dot = Vector3.Dot(perp, CoreServices.InputSystem.GazeProvider.GameObjectReference.transform.up);// it might need an update

			return dot <= 0 ? "left" : "right";
		}

		/// <summary>
		/// Update object with source lost info, including tracking duration.
		/// </summary>
		public void SourceLost(long timestamp = 0)
		{
			if (timestamp <= 0)
			{
				timestamp = MratHelpers.GetTimestamp();
			}

			SourceLostTimestamp = timestamp;

			if (SourceDetectedTimestamp <= 0) return;

			SourceTrackedDuration = SourceLostTimestamp - SourceDetectedTimestamp;
		}
	}
}
