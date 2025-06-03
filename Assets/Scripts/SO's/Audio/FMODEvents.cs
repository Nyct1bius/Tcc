using FMODUnity;
using UnityEngine;

[CreateAssetMenu(menuName ="FMODEvents/NewPlayerEvents")]
public class FMODEvents : ScriptableObject
{
    [field:Header("Combat")]
    [field: SerializeField] public EventReference _attack {  get; private set; }
    [field: SerializeField] public EventReference _hit { get; private set; }

    [field: Header("Movement")]
    [field: SerializeField] public EventReference _walk{ get; private set; }
    [field: SerializeField] public EventReference _dash { get; private set; }
    [field: SerializeField] public EventReference _jump { get; private set; }
}
