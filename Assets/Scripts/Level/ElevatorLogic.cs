using UnityEngine;
using DG.Tweening;
public class ElevatorLogic : MonoBehaviour
{
    public Transform[] transforms;
    //private void OnCollisionEnter(Collision collision)
    //{
    //    if (collision.gameObject.CompareTag("Player"))
    //    {
    //        collision.transform.parent = transform;

    //        if (transform.position.y != transforms[1].position.y)
    //        {
    //            transform.DOMoveY(transforms[1].position.y, 8f).SetEase(Ease.InOutSine).OnComplete(() => transform.DOShakePosition(0.5f, 0.25f).OnComplete(() => 
    //            transform.position = transforms[1].position));
    //        }
    //        else
    //            transform.DOMoveY(transforms[0].position.y, 8f).SetEase(Ease.InOutSine).OnComplete(() => transform.DOShakePosition(0.5f, 0.25f).OnComplete(() =>
    //            transform.position = transforms[0].position));
    //    }
    //}
    //private void OnCollisionExit(Collision collision)
    //{
    //    collision.transform.parent = null;
    //}

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            other.transform.parent = transform.parent;
            MoveElevator();
            
        }
    }
    [ContextMenu("MoveElevator")]
    public void MoveElevator()
    {
        if (transform.parent.position.y != transforms[1].position.y)
        {
            transform.parent.DOMoveY(transforms[1].position.y, 10f).SetEase(Ease.InOutSine).SetUpdate(UpdateType.Fixed).OnComplete(() => transform.parent.DOShakePosition(0.5f, 0.25f).OnComplete(() =>
            transform.parent.position = transforms[1].position));
        }
        else
            transform.parent.DOMoveY(transforms[0].position.y, 10f).SetEase(Ease.InOutSine).SetUpdate(UpdateType.Fixed).OnComplete(() => transform.parent.DOShakePosition(0.5f, 0.25f).OnComplete(() =>
            transform.parent.position = transforms[0].position));
    }
    private void OnTriggerExit(Collider other)
    {
        other.transform.parent = null;
    }

}
