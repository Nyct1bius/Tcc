using UnityEngine;
using FMOD.Studio;

public class PlayerAudioManager : MonoBehaviour
{
    public FMODEvents playerFmodEvents;
    public AudioManager audioManager;
    [SerializeField] private GameObject audioSource;

    private EventInstance chickenSpininstance;
    private void Start()
    {
        chickenSpininstance = audioManager.CreateInstance(playerFmodEvents._chickenSpin);
    }
    private void OnEnable()
    {
        PlayerEvents.WalkSFX += PlayWalkAudio;
        PlayerEvents.DashSFX += PlayDashAudio;
        PlayerEvents.JumpSFX += PlayJumpAudio;
        PlayerEvents.IdleSFX += PlayIdleAudio;
        PlayerEvents.ChickenSFX += PlayChickenSpinAudio;
        PlayerEvents.StopChickenSFX += StopChickenSpinAudio;
        PlayerEvents.LandSFX += PlayLandAudio;
        GameEvents.PauseGame += () => audioSource.SetActive(false);
        GameEvents.ResumeGame += () => audioSource.SetActive(true);

    }

    private void OnDisable()
    {
        PlayerEvents.WalkSFX -= PlayWalkAudio;
        PlayerEvents.DashSFX -= PlayDashAudio;
        PlayerEvents.JumpSFX -= PlayJumpAudio;
        PlayerEvents.IdleSFX -= PlayIdleAudio;
        PlayerEvents.ChickenSFX -= PlayChickenSpinAudio;
        PlayerEvents.StopChickenSFX -= StopChickenSpinAudio;
        PlayerEvents.LandSFX -= PlayLandAudio;
        GameEvents.PauseGame -= () => audioSource.SetActive(false);
        GameEvents.ResumeGame -= () => audioSource.SetActive(true);


    }

    public void PlayAttackAudio()
    {
        audioManager.PlayOneShot(playerFmodEvents._attack,transform.parent.position);
    }

    private void PlayWalkAudio()
    {
        audioManager.PlayOneShot(playerFmodEvents._walk, transform.position);
    }
    private void PlayDashAudio()
    {
        audioManager.PlayOneShot(playerFmodEvents._dash, transform.position);
    }
    private void PlayJumpAudio()
    {
        audioManager.PlayOneShot(playerFmodEvents._jump, transform.position);
    }

    private void PlayLandAudio()
    {
        audioManager.PlayOneShot(playerFmodEvents._land, transform.position);
    }

    private void PlayIdleAudio()
    {
        audioManager.PlayOneShot(playerFmodEvents._idle, transform.position);
    }

    public void PlayChickenSpinAudio()
    {
        PLAYBACK_STATE playbackState;
        chickenSpininstance.getPlaybackState(out playbackState);
        if (playbackState.Equals(PLAYBACK_STATE.STOPPED))
        {
            chickenSpininstance.start();
        }
       
    }

    public void SetChickenVelocity(float speed)
    {
        chickenSpininstance.setParameterByName("ChickenVelocity", speed);
    }

    private void StopChickenSpinAudio()
    {
        chickenSpininstance.stop(STOP_MODE.ALLOWFADEOUT);

    }
}
