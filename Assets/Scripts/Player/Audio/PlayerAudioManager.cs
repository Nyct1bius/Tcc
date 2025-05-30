using System;
using UnityEngine;

public class PlayerAudioManager : MonoBehaviour
{
    [SerializeField] FMODEvents _playerFmodEvents;

    private void OnEnable()
    {
        PlayerEvents.AttackSFX += PlayAttackAudio;
        PlayerEvents.WalkSFX += PlayWalkAudio;
        PlayerEvents.DashSFX += PlayDashAudio;

    }

    private void OnDisable()
    {
        PlayerEvents.AttackSFX -= PlayAttackAudio;
        PlayerEvents.WalkSFX -= PlayWalkAudio;
        PlayerEvents.DashSFX -= PlayDashAudio;
    }

    private void PlayAttackAudio()
    {
        AudioManager.PlayOneShot(_playerFmodEvents._attack,transform.parent.position);
    }

    private void PlayWalkAudio()
    {
        AudioManager.PlayOneShot(_playerFmodEvents._walk, transform.position);
    }
    private void PlayDashAudio()
    {
        AudioManager.PlayOneShot(_playerFmodEvents._dash, transform.position);
    }
}
