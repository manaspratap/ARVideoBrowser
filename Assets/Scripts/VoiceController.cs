using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;
using UnityEngine.Networking;
using System;
using System.Threading;
using System.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TextSpeech;

// to get a video for the voice query
public class VoiceController : MonoBehaviour
{
    const string LANG_CODE = "en-US";

    [SerializeField]
    Text uiText;

    void Start()
    {
        Setup(LANG_CODE);

        SpeechToText.instance.onResultCallback = OnFinalSpeechResult;
    }

    public void StartListening()
    {
        SpeechToText.instance.StartRecording();
    }

    public void StopListening()
    {
        SpeechToText.instance.StopRecording();
    }

    async void OnFinalSpeechResult(string result)
    {
        uiText.text = result;

        var videoPlayer = GetComponent<VideoPlayer>();
        await videoPlayer.PlayVideoAsync(result);
    }

    void Setup(string code)
    {
        SpeechToText.instance.Setting(code);
    }
}

public static class VideoPlayerExtensions
{
    public static async Task PrepareAsync(this VideoPlayer videoPlayer, CancellationToken cancellationToken = default)
    {
        if (videoPlayer.isPrepared) return;

        var tcs = new TaskCompletionSource<bool>();

        void OnPrepare(VideoPlayer source)
        {
            videoPlayer.prepareCompleted -= OnPrepare;
            if (cancellationToken.IsCancellationRequested)
            {
                tcs.TrySetCanceled(cancellationToken);
            }
            else
            {
                tcs.TrySetResult(true);
            }
        }

        cancellationToken.Register(obj => OnPrepare((VideoPlayer)obj), videoPlayer);

        videoPlayer.prepareCompleted += OnPrepare;
        videoPlayer.Prepare();

        await tcs.Task;
    }

    public static async Task PlayVideoAsync(this VideoPlayer videoPlayer, string voiceQuery, CancellationToken cancellationToken = default)
    {
        string searchWord = voiceQuery.Replace(" ", "+");
        // get video link from server
        var rawUrl = await VideoBrowserServer.GetVideoMetaDataAsync<string>("https://ar-video-browser-scripts.herokuapp.com/video?name=" + searchWord);

        videoPlayer.source = VideoSource.Url;
        videoPlayer.url = $"{rawUrl}";

        // prepare video
        await videoPlayer.PrepareAsync(cancellationToken);

        // play video
        videoPlayer.Play();
    }
}

public class VideoBrowserServer
{
    public static async Task<string> GetVideoMetaDataAsync<String>(string requestUrl)
    {
        var request = UnityWebRequest.Get(requestUrl);
        var tcs = new TaskCompletionSource<string>();
        request.SendWebRequest().completed += operation =>
        {
            if (request.isNetworkError)
            {
                tcs.TrySetException(new Exception(request.error));
                return;
            }

            var text = request.downloadHandler.text;

            if (request.isHttpError)
            {
                tcs.TrySetException(new Exception(request.error + "\nResponseError:" + text));
                return;
            }

            tcs.TrySetResult(text);
        };

        return await tcs.Task;
    }
}
