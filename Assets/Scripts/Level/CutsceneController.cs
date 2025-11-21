using FMODUnity;
using System.Collections;
using UnityEngine;
using UnityEngine.Video;

public class CutsceneController : MonoBehaviour
{
    public VideoPlayer player;
    public StudioEventEmitter emitter;

    private LoadScene loadscene;


    private void Awake()
    {
        loadscene = GetComponent<LoadScene>();
    }
    private void Start()
    {
        StartCoroutine(DelayedStart());
        player.loopPointReached += OnVideoFinished;

    }

    private void OnDestroy()
    {
        player.loopPointReached -= OnVideoFinished;
    }

    private IEnumerator DelayedStart()
    {
        yield return new WaitForSeconds(0.5f);
        player.Play();
        emitter.Play();
    }


    void OnVideoFinished(VideoPlayer vp)
    {
        Debug.Log("VideoFinished");
        loadscene.StartLoad("1 - First Level");
    }
    
}
