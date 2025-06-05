using UnityEngine;
using FMODUnity;
using FMOD.Studio;
using System.Collections.Generic;

[CreateAssetMenu(menuName = "Audio Manager/New Audio Manager")]
public class AudioManager: ScriptableObject
{
    public List<EventInstance> eventInstances = new List<EventInstance>();
    public void PlayOneShot(EventReference sound, Vector3 worldPos)
    {
        RuntimeManager.PlayOneShot(sound, worldPos);    
    }

    public EventInstance CreateInstance(EventReference eventReference)
    {
        EventInstance instance = RuntimeManager.CreateInstance(eventReference);
        eventInstances.Add(instance);
        return instance;
    }

    public void CleanUp()
    {
        foreach (EventInstance instance in eventInstances)
        {
            instance.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
            instance.release();
            eventInstances.Remove(instance);
        }
    }
    private void OnDestroy()
    {
        CleanUp();
    }
}
