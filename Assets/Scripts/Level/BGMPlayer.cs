using UnityEngine;
using FMODUnity;
using System.Collections;

public class BGMPlayer : MonoBehaviour
{
    [SerializeField] EventReference BGMReference;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        StartCoroutine(DelayStart());
    }

    private IEnumerator DelayStart()
    {
        RuntimeManager.PauseAllEvents(true);
        yield return new WaitForSeconds(2f);



        BGMManager.Instance.SetBGMToPlay(BGMReference);
    }
}
