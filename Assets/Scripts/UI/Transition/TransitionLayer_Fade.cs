using System.Collections;
using System.Runtime.CompilerServices;
using UnityEngine;
[RequireComponent(typeof(CanvasGroup))]
public class TransitionLayer_Fade : TransitionLayer
{
    private CanvasGroup cover;
    private Coroutine tween;
    public override void Hide(float time, float delay)
    {
        isDone = false;
        progress = 0.0f;
        tween = StartCoroutine(HideTween(time, delay));
    }

    public override void HideImmediately()
    {
        InterruptTween();
        cover.alpha = 0.0f;
        gameObject.SetActive(false);
    }

    public override void Show(float time, float delay)
    {
        gameObject.SetActive(true);
        isDone = false;
        progress = 0.0f;
        tween = StartCoroutine(ShowTween(time, delay));
    }

    public override void ShowImmediately()
    {
        InterruptTween();
        cover.alpha = 1.0f;
        gameObject.SetActive(true);
    }

    private void Awake()
    {
        cover = GetComponent<CanvasGroup>();
    }

    private void InterruptTween()
    {
        if (tween != null)
        {
            StopCoroutine(tween);
            tween = null;
        }
        progress = 1.0f;
        isDone = true;
    }

    private IEnumerator ShowTween(float time, float delay)
    {
        float t = 0.0f;
        cover.alpha = 0.0f;
        yield return new WaitForSeconds(delay);

        while (t < time)
        {
            t += Time.unscaledDeltaTime;
            Debug.Log(t);
            progress = Mathf.Clamp01(t / time);
            cover.alpha = progress;
            yield return null;
        }

        cover.alpha = 1.0f;
        progress = 1.0f;
        isDone = true;
        tween = null;
        InvokeAndClearCallback();
    }

    private IEnumerator HideTween(float time, float delay)
    {
        float t = 0.0f;
        cover.alpha = 1.0f;
        yield return new WaitForSeconds(delay);

        while (t < time)
        {
            t += Time.unscaledDeltaTime;
            progress = Mathf.Clamp01(t / time);
            cover.alpha = 1 - progress;
            yield return null;
        }

        cover.alpha = 0.0f;
        progress = 0.0f;
        isDone = false;
        tween = null;
        gameObject.SetActive(false);
        InvokeAndClearCallback();
    }


}
