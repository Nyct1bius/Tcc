using UnityEngine;
using FMODUnity;
using FMOD.Studio;
using System.Collections;

public class BGMManager : MonoBehaviour
{
    public static BGMManager Instance;
    private EventInstance bgmPlay;
    public EventReference songToPlay;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        
    }

    void PlayBGM()
    {
        if (songToPlay.IsNull)
        {
            Debug.LogWarning("BGMManager: No event set to play!");
            return;
        }

        // Stop any existing instance
        if (bgmPlay.isValid())
        {
            bgmPlay.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
            bgmPlay.release();
        }

        bgmPlay = RuntimeManager.CreateInstance(songToPlay);
        bgmPlay.start();

        Debug.Log($"BGMManager: Playing {songToPlay.Path}");
    }

    public void StopBGM()
    {
        if (bgmPlay.isValid())
            bgmPlay.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
    }

    public void StopAndClearBGM()
    {
        if (bgmPlay.isValid())
        {
            bgmPlay.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
            bgmPlay.release();
        }
    }

    public void SetBGMToPlay(EventReference nextSongToPlay)
    {
        songToPlay = nextSongToPlay;
        StartCoroutine(PlayAfterDelay());
    }


    IEnumerator PlayAfterDelay()
    {
        yield return new WaitForSeconds(0.5f);
        Debug.Log("Banks loaded? " + RuntimeManager.HaveAllBanksLoaded);
        FMOD.Studio.Bus masterBus = FMODUnity.RuntimeManager.GetBus("bus:/");
        masterBus.getVolume(out float volume);
        Debug.Log("Master bus volume: " + volume);
        PlayBGM();
    }
}
