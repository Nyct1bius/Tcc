using UnityEngine;
using DG.Tweening;

public class PlatformRotator : MonoBehaviour
{
    public float rotationSpeed;
    public bool clockwise;
    void Start()
    {
        float duration = 360f / rotationSpeed;
        transform.DORotate(
            clockwise ? new Vector3(0, 360, 0) : new Vector3(0, -360, 0),
            duration, RotateMode.FastBeyond360)
            .SetEase(Ease.Linear)
            .SetLoops(-1, LoopType.Restart);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Player") 
        {
            collision.transform.parent = transform;
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        collision.transform.parent = null;
    }
    private void OnTriggerEnter(Collider other)
    {
        print("OnTriggerCalled");
        
    }

    private void OnTriggerExit(Collider other)
    {
        other.transform.parent = null;
        
    }


}
