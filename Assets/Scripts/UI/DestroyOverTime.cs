using System.Collections;
using UnityEngine;

public class DestroyOverTime : MonoBehaviour
{
    public bool callOnStart;
    void Start()
    {
        if(callOnStart == true)
        {
            DestroyIn(5);
        }
    }

    public void DestroyIn(float timeToDestroy)
    {
        StartCoroutine(CountDown(timeToDestroy));
    }

    IEnumerator CountDown(float value)
    {
        float normalizedTime = 0;
        while(normalizedTime <= 1)
        {
            normalizedTime += Time.deltaTime / value;
            yield return null;
        }
        Destroy(gameObject);
    }
}
