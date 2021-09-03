using UnityEngine;

// uses two touches, and scales the object (video)
public class TouchInteraction : MonoBehaviour
{

    Vector2 minScale = Vector3.one * .1f;
    Vector2 maxScale = Vector3.one * 50;

    void Update()
    {
        if (Input.GetMouseButtonDown(1))
        {
            // double touch occurred 
            scaleDelta = 0;
        }

        if (Input.touchCount > 1)
        {
            Vector2 firstPos = Input.GetTouch(0).position;
            Vector2 secondPos = Input.GetTouch(1).position;

            ScaleContent(firstPos, secondPos);
        }
    }

    float scaleDelta;
    Vector2 firstScale;

    void ScaleContent(Vector2 firstPos, Vector2 secondPos)
    {
        float delta = Vector2.Distance(firstPos, secondPos);

        // save last frame of double touch as original state
        if (scaleDelta.Equals(0))
        {
            scaleDelta = delta;
            firstScale = transform.localScale;
        }

        float currDelta = 1 + (delta - scaleDelta) / 200;
        Vector3 newScale = firstScale * currDelta;

        if (newScale.x > minScale.x && newScale.x < maxScale.x)
        {
            // scale the object
            transform.localScale = newScale;
        }
    }
}
