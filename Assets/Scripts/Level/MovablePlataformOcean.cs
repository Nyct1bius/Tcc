using DG.Tweening;
using System.Collections;
using UnityEngine;

public class MovablePlataformOcean : MonoBehaviour
{
    [SerializeField] private float movePlataformTime = 2f;
    [SerializeField] private float amountToLower = 2f;
    [SerializeField] private float timeBetweenStates = 2f;
    [SerializeField] private int shakesBeforeGoingDown = 3;
    [SerializeField] private float shakeForce = 0.3f;
    [SerializeField] private float shakeMultiply = 1.5f;

    private bool isDown;
    private Vector3 startPos;

    private void Start()
    {
        startPos = transform.position;
        StartCoroutine(ShakePlataform());
    }

    private IEnumerator ShakePlataform()
    {
        yield return new WaitForSeconds(timeBetweenStates);

        float currentShake = shakeForce;
        float shakeDuration = 0.3f;

        for (int i = 0; i < shakesBeforeGoingDown; i++)
        {
            yield return transform.DOShakePosition(
                shakeDuration,
                currentShake,
                10,
                90,
                false,
                true
            ).SetEase(Ease.OutBack).WaitForCompletion();

            currentShake *= shakeMultiply;
            shakeDuration += 0.1f;
            yield return new WaitForSeconds(1.5f);
        }

        StartCoroutine(ChangeState());
    }

    private void MoveDown()
    {
        transform.DOMoveY(startPos.y - amountToLower, 1f).SetEase(Ease.InCubic)
            .OnComplete(() => StartCoroutine(ChangeState(timeBetweenStates)));
    }

    private void MoveUp()
    {
        transform.DOMoveY(startPos.y, movePlataformTime).SetEase(Ease.InOutSine)
            .OnComplete(() => StartCoroutine(ShakePlataform()));
    }

    private IEnumerator ChangeState(float time = 0)
    {
        yield return new WaitForSeconds(time);
        isDown = !isDown;

        if (isDown)
            MoveDown();
        else
            MoveUp();
    }
}
