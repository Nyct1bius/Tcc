using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System.Collections;
using System.Linq;

public class HorizontalMovePlatform : MonoBehaviour
{
    public List<Transform> points;
    [SerializeField] float movementSpeed = 5f;
    private int currentIndex = 1;
    private Vector3 lastPosition;
    public Vector3 velocity { get; private set; }

    public bool loop = false;
    public bool isGoingBack = false;

    public bool needsToReturn = false;
    public int checkpointIndexChecker = 0;



    private void Start()
    {
        if (points.Count < 2) Debug.LogError("Need at least 2 waypoints!");
        transform.position = points[0].position;
        lastPosition = transform.position;

    }
    private void FixedUpdate()
    {
        //MovePlatform();
    }

    private void MovePlatform()
    {
        print("MovePlatformCalled");
        if (points.Count == 0) return;

        Vector3 target = points[currentIndex].position;
        print(points.Count);
        //transform.position = Vector3.MoveTowards(transform.position, target, movementSpeed * Time.deltaTime);

        transform.parent.DOMove(target, 4f).SetEase(Ease.InOutSine).SetUpdate(UpdateType.Fixed).OnComplete(() => transform.parent.DOShakePosition(0.5f, 0.15f).OnComplete(() =>
                transform.parent.position = target).OnComplete(() => SetNextIndex()));

        //if (Vector3.Distance(transform.position, target) < 0.01f)
        //{
            
        //}
    }

    public void SetNextIndex()
    {
        if (!isGoingBack)
        {
            if (currentIndex < points.Count - 1)
            {
                currentIndex++;
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
        }
    }

    private void OnTriggerExit(Collider other)
    {
        other.transform.parent = null;
        if(needsToReturn)
        {
            print("HasLeftTrigger");
            StartCoroutine(DelayReturn());
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
}
