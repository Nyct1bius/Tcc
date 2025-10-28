using UnityEngine;
using System.Collections;
using DG.Tweening;


public class HealingBattery : MonoBehaviour, IPooledObject
{
    public float despawnTimer;
    public CapsuleCollider capColl;

    [Header("Float Settings")]
    public float floatAmplitude = 0.5f;
    public float floatDuration = 2f;

    [Header("Rotation Settings")]
    public float rotationAngle = 10f;
    public float rotationDuration = 3f;

    [Header("Scale Settings")]
    public float scaleDuration = 1f;

    public void OnObjectSpawn()
    {
        transform.localScale = Vector3.zero;

        Sequence seq = DOTween.Sequence();

        seq.Append(transform.DOScale(new Vector3(0.5f, 0.5f, 0.5f), scaleDuration)
            .SetEase(Ease.Linear)).OnComplete(() => StartCoroutine(EnableCollider()));

        seq.AppendCallback(() =>
        {
            float startY = transform.position.y;

            transform.DOMoveY(startY + floatAmplitude, floatDuration)
                .SetEase(Ease.InOutSine)
                .SetLoops(-1, LoopType.Yoyo);

            transform.DORotate(new Vector3(0, 0, rotationAngle), rotationDuration, RotateMode.LocalAxisAdd)
                .SetEase(Ease.InOutSine)
                .SetLoops(-1, LoopType.Yoyo);
        });



        StartCoroutine(DeactivateFromTime());
    }

    private void OnTriggerEnter(Collider other)
    {
        if (GameManager.instance.PlayerInstance == other.gameObject)
        {
            other.GetComponent<PlayerHealthManager>().HealHealth(10f);

            StartCoroutine(DeactivateAfterHit());
        }
    }

    IEnumerator DeactivateFromTime()
    {
        yield return new WaitForSeconds(despawnTimer);
        gameObject.SetActive(false);
    }
    IEnumerator DeactivateAfterHit()
    {
        StopCoroutine(DeactivateFromTime());
        yield return new WaitForSeconds(0.05f);
        gameObject.SetActive(false);
    }

    IEnumerator EnableCollider()
    {
        yield return new WaitForSeconds(1f);
        capColl.enabled = true;
    }

}
