using UnityEngine;

using MRAT;
using Microsoft.MixedReality.Toolkit.Input;
using Microsoft.MixedReality.Toolkit;

public class UngroupOnClick : MonoBehaviour, IMixedRealityPointerHandler {

    public AgglomerativeClusteringVisualizerView parent;
    public AgglomerativeClusteringVisualizerView.ClusterNode node;

    void Start() {
        CoreServices.InputSystem.PushFallbackInputHandler(gameObject);
    }

    public void OnInputClicked(MixedRealityPointerEventData eventData) {
        if (!eventData.used && parent != null && node != null) {
            eventData.Use();
            parent.Split(node);
            var zoom_controller = parent.gameObject.GetComponent<ZoomClusteringController>();
            zoom_controller.enabled = false;
        }        
    }

    public void OnPointerDown(MixedRealityPointerEventData eventData)
    {
        throw new System.NotImplementedException();
    }

    public void OnPointerDragged(MixedRealityPointerEventData eventData)
    {
        throw new System.NotImplementedException();
    }

    public void OnPointerUp(MixedRealityPointerEventData eventData)
    {
        throw new System.NotImplementedException();
    }

    public void OnPointerClicked(MixedRealityPointerEventData eventData)
    {
        if (!eventData.used && parent != null && node != null)
        {
            eventData.Use();
            parent.Split(node);
            var zoom_controller = parent.gameObject.GetComponent<ZoomClusteringController>();
            zoom_controller.enabled = false;
        }
    }
}

/*

public class UngroupOnClick : MonoBehaviour, IInputClickHandler {

    public AgglomerativeClusteringVisualizerView parent;
    public AgglomerativeClusteringVisualizerView.ClusterNode node;

    void Start() {
        InputManager.Instance.PushFallbackInputHandler(gameObject);
    }

    public void OnInputClicked(InputClickedEventData eventData) {
        if (!eventData.used && parent != null && node != null) {
            eventData.Use();
            parent.Split(node);
            var zoom_controller = parent.gameObject.GetComponent<ZoomClusteringController>();
            zoom_controller.enabled = false;
        }        
    }
	
}

*/

