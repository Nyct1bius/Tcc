using UnityEngine;
using UnityEngine.UI;
using FMODUnity;
using static UnityEngine.Rendering.DebugUI;

public class AudioSettings : MonoBehaviour
{

    FMOD.Studio.Bus Master;
    FMOD.Studio.Bus Music;
    FMOD.Studio.Bus SFX;

    //public float MasterVolume;

    [SerializeField] private Slider MasterSlider;
    [SerializeField] private Slider MusicSlider;
    [SerializeField] private Slider SFXSlider;

    private void Awake()
    {
        Master = FMODUnity.RuntimeManager.GetBus("bus:/");
        Music = FMODUnity.RuntimeManager.GetBus("bus:/Music");
        SFX = FMODUnity.RuntimeManager.GetBus("bus:/SFX");
    }

    private void Start()
    {
        Master.setVolume(MasterSlider.value);
    }

    public void MusicVolumeLevel()
    {
        Music.setVolume(MusicSlider.value);
    }
        
    public void MasterVolumeLevel()
    {

        Master.setVolume(MasterSlider.value);
    }

    public void SFXVolumeLevel()
    {
        SFX.setVolume(SFXSlider.value);
    }

}
