using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using Microsoft.MixedReality.Toolkit.Input;
using Microsoft.MixedReality.Toolkit.Audio;

using JetBrains.Annotations;
using UnityEngine;
using Debug = UnityEngine.Debug;

// ReSharper disable CompareOfFloatsByEqualityOperator

namespace MRAT
{
    public static class MratHelpers
    {
	    public static string GetServerPath(string baseServerPath, string endpointPath)
	    {
		    return new Uri(new Uri(baseServerPath), endpointPath).ToString();
		}

	    private static void LogMratFiles(string path)
	    {
		    var files = Directory.GetFiles(path, "*.png", SearchOption.TopDirectoryOnly)
			    .OrderByDescending(f => f).ToArray();

		    Debug.Log("Mrat files in data path " + path + " \n" + string.Join(", ", files));
	    }

		public static string ToQueryString([CanBeNull] IEnumerable<KeyValuePair<string, object>> parameters)
	    {
		    if (parameters != null)
			    return "";

		    var builder = new StringBuilder("?");

		    var separator = "";
		    // ReSharper disable once AssignNullToNotNullAttribute
		    foreach (var kvp in parameters.Where(kvp => kvp.Value != null))
		    {
			    builder.AppendFormat("{0}{1}={2}", separator, WebUtility.UrlEncode(kvp.Key), WebUtility.UrlEncode(kvp.Value.ToString()));

			    separator = "&";
		    }
            return builder.ToString();
	    }

		public static NameValueCollection ToNameValueCollection<TKey, TValue>(
		    this IDictionary<TKey, TValue> dict)
	    {
		    var nameValueCollection = new NameValueCollection();

		    foreach (var kvp in dict)
		    {
			    string value = null;
			    if (kvp.Value != null)
				    value = kvp.Value.ToString();

			    nameValueCollection.Add(kvp.Key.ToString(), value);
		    }

		    return nameValueCollection;
	    }

		public static string DictionaryStringify(Dictionary<string, string> dict)
        {
            return string.Join(";", dict.Select(x => x.Key + "=" + x.Value).ToArray());
        }

        /// <summary>
        /// Returns Unix timestamp in milliseconds
        /// </summary>
        /// <returns></returns>
        public static long GetTimestamp() {
            return DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
            //return (DateTime.Now.Ticks / TimeSpan.TicksPerMillisecond);
        }

        public static MratCommunicationManager GetMratCommunicationManager()
        {
            return MratCommunicationManager.Instance;
        }

        public static MratVoiceCommands GetMratVoiceCommands()
        {
            if (!Application.isPlaying) return null;
            var mng = GameObject.Find("MratManager");
            if (mng == null) return null;
            var component = mng.GetComponent<MratVoiceCommands>();
            return component;
        }

        public static MratStatusTracker GetMratStatusTracker()
	    {
            if (!Application.isPlaying) return null;
            var mng = GameObject.Find("MratManager");
            if (mng == null) return null;
            var component = mng.GetComponent<MratStatusTracker>();

		    return component;
	    }

		public static TextToSpeech GetSpeaker()
	    {
            if (!Application.isPlaying) return null;
            var mng = GameObject.Find("MratManager");
            if (mng == null) return null;
            var component = mng.GetComponent<TextToSpeech>();

		    return component;
	    }

		public static MratPhotoRecorder GetMratPhotoRecorder()
        {
            if (!Application.isPlaying) return null;
            var mng = GameObject.Find("MratManager");
            if (mng == null) return null;
            var recorder = mng.GetComponent<MratPhotoRecorder>();

            return recorder;
        }

	    public static MratDaemon GetMratDaemon()
	    {
            if (!Application.isPlaying) return null;
            var mng = GameObject.Find("MratManager");
            if (mng == null) return null;
            var component = mng.GetComponent<MratDaemon>();

		    return component;
	    }

		public static AnimatedCursor GetAnimatedCursor()
        {
            bool cursorfound = false;
            if (!Application.isPlaying) return null;
            var cursor = GameObject.Find("DefaultCursor");
            if (cursor != null)
                cursorfound = true;
            else
            {
                
                cursorfound = true;
                cursor = GameObject.Find("DefaultCursor(Clone)");
                if (cursor == null)
                {
                    cursor = GameObject.Find("DefaultGazeCursor(Clone)");
                    if (cursor == null)
                        cursorfound = false;
                }
            }

            if (cursorfound && cursor!=null)
            {
                AnimatedCursor component=null;
                if (cursor.TryGetComponent<AnimatedCursor>(out component))
                    //var component = cursor.GetComponent<AnimatedCursor>();

                    return component;
                else
                    return null;
            }
            else
                return null;
        }

		#region AspectRatio Calculator functions
		// Original code for this region from wiki.unity3d.com/index.php/Get_Aspect_Ratio

		public static Vector2 GetAspectRatio(int x, int y)
	    {
		    var f = x / (float)y;
		    var i = 0;
		    while (true)
		    {
			    i++;
			    if (Math.Round(f * i, 2) == Mathf.RoundToInt(f * i))
				    break;
		    }
		    return new Vector2((float)Math.Round(f * i, 2), i);
	    }
	    public static Vector2 GetAspectRatio(Vector2 xy)
	    {
		    var f = xy.x / xy.y;
		    var i = 0;
		    while (true)
		    {
			    i++;
			    if (Math.Round(f * i, 2) == Mathf.RoundToInt(f * i))
				    break;
		    }
		    return new Vector2((float)Math.Round(f * i, 2), i);
	    }
	    public static Vector2 GetAspectRatio(int x, int y, bool debug)
	    {
		    var f = x / (float)y;
		    var i = 0;
		    while (true)
		    {
			    i++;
			    if (Math.Round(f * i, 2) == Mathf.RoundToInt(f * i))
				    break;
		    }
		    if (debug)
			    Debug.Log("Aspect ratio is " + f * i + ":" + i + " (Resolution: " + x + "x" + y + ")");
		    return new Vector2((float)Math.Round(f * i, 2), i);
	    }
	    public static Vector2 GetAspectRatio(Vector2 xy, bool debug)
	    {
		    var f = xy.x / xy.y;
		    var i = 0;
		    while (true)
		    {
			    i++;
			    if (Math.Round(f * i, 2) == Mathf.RoundToInt(f * i))
				    break;
		    }
		    if (debug)
			    Debug.Log("Aspect ratio is " + f * i + ":" + i + " (Resolution: " + xy.x + "x" + xy.y + ")");
		    return new Vector2((float)Math.Round(f * i, 2), i);
	    }

	    #endregion

	}
}
