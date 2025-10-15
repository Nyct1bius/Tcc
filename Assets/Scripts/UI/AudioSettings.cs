using UnityEngine;
using UnityEngine.UI;
using FMODUnity;
using static UnityEngine.Rendering.DebugUI;

public class AudioSettings : MonoBehaviour
{

    FMOD.Studio.Bus Master;

    public float MasterVolume;

    [SerializeField] private Slider MasterSlider;

    private void Awake()
    {
        Master = FMODUnity.RuntimeManager.GetBus("bus:/");
    }

    private void Start()
    {
        Master.setVolume(MasterSlider.value);
    }

    public void MasterVolumeLevel()
    {

        Master.setVolume(MasterSlider.value);
    }

}
