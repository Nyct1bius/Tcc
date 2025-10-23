using UnityEngine;
using DG.Tweening;

public class PlatformRaiser : MonoBehaviour
{
    public GameObject platformsParent;
    public Transform target;
    public float movementSpeed;



    public void RaisePlatforms()
    {
        platformsParent.transform.DOMoveY(target.position.y, movementSpeed).SetEase(Ease.InOutSine).SetUpdate(UpdateType.Fixed).OnPlay(() => transform.parent.DOShakePosition(0.5f, 0.15f));
    }
}
