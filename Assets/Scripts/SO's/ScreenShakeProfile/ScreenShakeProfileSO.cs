using UnityEngine;
using static Unity.Cinemachine.CinemachineImpulseDefinition;

[CreateAssetMenu(menuName ="ScreenShake / new Profile")]
public class ScreenShakeProfileSO : ScriptableObject
{
    [Header("Impulse Source Setting")]
    public float impactTime = 0.2f;
    [Range(0.1f, 20f)]
    public float impactForce = 1f;
    public Vector3 defaultVelocity = new Vector3(0,-1f,0);
    public ImpulseShapes impulseCurve;

    [Header("Impulse Listener Settings")]
    public float ListenerAmplitude = 1f;
    public float ListenerFrequency = 1f;
    public float ListenerDuration = 1f;
}
