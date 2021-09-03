using UnityEngine;
using UnityEngine.Video;

// change the video scale
// while changing between landscape and portrait mode
[RequireComponent(typeof(VideoPlayer))]
public class VideoScaler : MonoBehaviour
{

    Vector3 startScale;

    void Awake()
    {
        startScale = transform.localScale;

        VideoPlayer videoPlayer = GetComponent<VideoPlayer>();
        float width = videoPlayer.clip.width;
        float height = videoPlayer.clip.height;

        ScaleVideo(width, height);
    }

    public void ScaleVideo(float width, float height)
    {
        Vector3 newScale = startScale;

        if (width > height)
        {
            // landscape
            float AspectRatio = height / width;
            newScale.y = startScale.x * AspectRatio;
        }
        else
        {
            // portrait
            float AspectRatio = width / height;
            newScale.x = startScale.y * AspectRatio;
        }

        transform.localScale = newScale;
    }
}
