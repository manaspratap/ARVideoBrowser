using UnityEngine;
using UnityEngine.XR.ARFoundation;

// to toggle the plane and point cloud (top left)
public class TogglePointAndPlane : MonoBehaviour
{

    public ARPlaneManager planeManager;
    public ARPointCloudManager pointCloudManager;

    public void OnValueChanged(bool isOn)
    {
        // isOn false at start
        VisualizePlanes(isOn);
        VisualizePoints(isOn);
    }

    void VisualizePlanes(bool active)
    {
        planeManager.enabled = active;
        foreach (ARPlane plane in planeManager.trackables)
        {
            plane.gameObject.SetActive(active);
        }
    }

    void VisualizePoints(bool active)
    {
        pointCloudManager.enabled = active;
        foreach (ARPointCloud pointCLoud in pointCloudManager.trackables)
        {
            pointCLoud.gameObject.SetActive(active);
        }
    }
}
