using Unity.Cinemachine;
using UnityEngine;

public static class CameraShakeManager
{

    public static void CameraShake(CinemachineImpulseSource source)
    {
        source.GenerateImpulseWithForce(1f);
    }

    public static void CameraShakeFromProfile(ScreenShakeProfileSO profile, CinemachineImpulseSource source)
    {
        source.GenerateImpulseWithForce(profile.impactForce);

        SetupScreenShake(profile, source);
    }

    private static void SetupScreenShake(ScreenShakeProfileSO profile, CinemachineImpulseSource source)
    {
        CinemachineImpulseDefinition impulseDefinition = source.ImpulseDefinition;
        impulseDefinition.ImpulseDuration = profile.impactTime;
        source.DefaultVelocity = profile.defaultVelocity;   
        impulseDefinition.ImpulseShape = profile.impulseCurve;

        CinemachineImpulseListener impulseListener = GameManager.instance._cnCameraRef.GetComponent<CinemachineImpulseListener>();
        impulseListener.ReactionSettings.AmplitudeGain = profile.ListenerAmplitude;
        impulseListener.ReactionSettings.FrequencyGain = profile.ListenerFrequency;
        impulseListener.ReactionSettings.Duration = profile.ListenerDuration;

    }
}
