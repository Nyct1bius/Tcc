using UnityEngine;
using FMODUnity;

public static class AudioManager 
{
    public static void PlayOneShot(EventReference sound, Vector3 worldPos)
    {
        RuntimeManager.PlayOneShot(sound, worldPos);    
    }
}
