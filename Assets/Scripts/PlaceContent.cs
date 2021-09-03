using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARSubsystems;
using UnityEngine.XR.ARFoundation;
using UnityEngine.EventSystems;
using UnityEngine.UI;

// to move the object (video) around
public class PlaceContent : MonoBehaviour
{

    public ARRaycastManager raycastManager;
    public GraphicRaycaster raycaster;

    bool wasDoubleTouch;

    void Update()
    {

        if (Input.GetMouseButtonDown(0))
        {
            wasDoubleTouch = false;
        }

        if (Input.GetMouseButtonDown(1))
        {
            wasDoubleTouch = true;
        }

        if (Input.GetMouseButtonUp(0) && !IsClickOverUI() && !wasDoubleTouch)
        {

            List<ARRaycastHit> hitPoints = new List<ARRaycastHit>();
            raycastManager.Raycast(Input.mousePosition, hitPoints, TrackableType.Planes);

            if (hitPoints.Count > 0)
            {
                Pose pose = hitPoints[0].pose;
                transform.rotation = pose.rotation;
                transform.position = pose.position;
            }
        }
    }

    bool IsClickOverUI()
    {
        PointerEventData data = new PointerEventData(EventSystem.current)
        {
            position = Input.mousePosition
        };
        List<RaycastResult> results = new List<RaycastResult>();
        raycaster.Raycast(data, results);
        return results.Count > 0;
    }
}
