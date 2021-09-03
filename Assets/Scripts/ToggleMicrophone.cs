using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.Events;

// to toggle the microphone (bottom left)
public class ToggleMicrophone : MonoBehaviour
{

    public UnityEvent upEvent;
    public UnityEvent downEvent;

    public void OnValueChanged(bool isOn)
    {
        // isOn false at start
        if (isOn)
        {
            OnMouseDown();
        }
        else
        {
            OnMouseUp();
        }
    }

    void OnMouseDown()
    {
        downEvent?.Invoke();
    }

    void OnMouseUp()
    {
        upEvent?.Invoke();
    }
}
