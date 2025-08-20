using UnityEngine;

public class SomTeste : MonoBehaviour
{

    public AudioSource audioSource;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Debug.Log("Unity audio output sample rate: " + AudioSettings.outputSampleRate);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
