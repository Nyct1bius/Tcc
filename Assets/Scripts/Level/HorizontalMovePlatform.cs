using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System.Collections;

public class HorizontalMovePlatform : MonoBehaviour
{
    public List<Transform> points;
    [SerializeField] float movementSpeed = 4f;
    public int currentIndex = 1;
    private Vector3 lastPosition;
    public Vector3 velocity { get; private set; }

    public bool loop = false;
    public bool isGoingBack = false;

    public bool needsToReturn = false;
    public int checkpointIndexChecker = 0;

    public bool needsToRotate = false;
    public float origitalRotationY;
    public float rotateAmount = 0f;

    public HorizontalPlatformLever lever;
    public GameObject waybackLever;


    private void Start()
    {
        if (points.Count < 2) Debug.LogError("Need at least 2 waypoints!");
        transform.position = points[0].position;
        lastPosition = transform.position;
        origitalRotationY = transform.parent.rotation.y;
    }

    [ContextMenu("MovePlatform")]
    public void MovePlatform()
    {
        print("MovePlatformCalled");
        if (points.Count == 0) return;

        Vector3 target = points[currentIndex].position;

        transform.parent.DOMove(target, movementSpeed).SetEase(Ease.InOutSine).SetUpdate(UpdateType.Fixed).OnComplete(() => transform.parent.DOShakePosition(0.5f, 0.15f).OnComplete(() =>
                transform.parent.position = target).OnComplete(() => SetNextIndex()));

        RotateElevator();

    }

    public void SetNextIndex()
    {
        if (!isGoingBack)
        {
            if (currentIndex < points.Count - 1)
            {
                currentIndex++;
                if (needsToRotate)
            transform.parent.DORotate( new Vector3(0,rotateAmount,0), movementSpeed);
                MovePlatform();
            }
            else
            {
                isGoingBack = true;
                currentIndex--;
            }
        }
        else
        {
            if (currentIndex > 0)
            {
                currentIndex--;
                MovePlatform();
            }
            else
            {
                isGoingBack = false;
                currentIndex++;
            }
        }
    }

    private void LateUpdate()
    {
        //velocity = (transform.position - lastPosition) / Time.deltaTime;
        //lastPosition = transform.position;     
    }

    private void OnTriggerEnter(Collider other)
    {
        print("OnTriggerCalled");
        if(other.CompareTag("Player"))
        {
            other.transform.parent = transform;
            MovePlatform();
            if(waybackLever != null)
                waybackLever.SetActive(true);

            if(lever != null && !lever.enabled)
            {
                lever.enabled = true;
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        other.transform.parent = null;
        if(needsToReturn)
        {
            print("HasLeftTrigger");
            //StartCoroutine(DelayReturn());
        }
    }

    private IEnumerator DelayReturn()
    {
        yield return new WaitForSeconds(2f);
        print("Has Waited");
        print(currentIndex);
        if(currentIndex  == 0 && GameManager.instance._checkpointIndex < checkpointIndexChecker)
        {
            print("HasAccessedIF");
            MovePlatform();
        }
    }

    void RotateElevator()
    {
        if (currentIndex == 0)
        {
            if (needsToRotate)
                transform.parent.DORotate(new Vector3(0, 0, 0), movementSpeed);
        }
        else
        {
            if (needsToRotate)
                transform.parent.DORotate(new Vector3(0, rotateAmount, 0), movementSpeed);
        }
    }
}
