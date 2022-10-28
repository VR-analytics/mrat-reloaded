using Microsoft.MixedReality.Toolkit;
using System.Collections;

using UnityEngine;
using UnityEngine.Networking;

namespace MRAT.Examples
{
	public class LoadTextureFromWeb : MonoBehaviour
	{
		public string TextureUrl = "https://upload.wikimedia.org/wikipedia/commons/d/db/Patern_test.jpg";

		private MratCommunicationManager _mratManager;

		private void Start()
		{
			_mratManager = MratHelpers.GetMratCommunicationManager();

			StartCoroutine(nameof(LoadTexture), TextureUrl);

			
            if (!CoreServices.InputSystem.EventListeners.Contains(gameObject))
            {
                CoreServices.InputSystem.EventListeners.Add(gameObject);
               
            }
        }

		public void LoadTexture(string texturePath)
		{
			StartCoroutine(nameof(GetTexture), texturePath);
		}

		private  IEnumerator GetTexture(string texturePath)
		{
			var www = UnityWebRequestTexture.GetTexture(texturePath);

			yield return www.SendWebRequest();

			if (www.isNetworkError || www.isHttpError)
			{
				Debug.Log(www.error);
			}
			else
			{
				Debug.Log("Loading texture from path: " + texturePath);

				var rend = GetComponent<Renderer>();
				var myTexture = ((DownloadHandlerTexture)www.downloadHandler).texture;
				rend.material.mainTexture = myTexture;

				var mratEvent = new MratEventSimple("Texture loaded from path: " + texturePath);
				mratEvent.CollectDataFromUnity();
				
				_mratManager.LogMratEvent(mratEvent);
			}
		}
	}
}
