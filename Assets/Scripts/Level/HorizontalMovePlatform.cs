using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public class HorizontalMovePlatform : MonoBehaviour
{
    public List<Transform> points;
    [SerializeField] float movementSpeed = 5f;
    private int currentIndex;
    private Vector3 lastPosition;
    public Vector3 velocity { get; private set; }

    public bool loop = false;
    public bool isGoingBack = false;



    private void Start()
    {
        if (points.Count < 2) Debug.LogError("Need at least 2 waypoints!");
        transform.position = points[0].position;
        lastPosition = transform.position;

    }
    private void FixedUpdate()
    {
        MovePlatform();
    }

    private void MovePlatform()
    {
        if (points.Count == 0) return;

        Vector3 target = points[currentIndex].position;
        transform.position = Vector3.MoveTowards(transform.position, target, movementSpeed * Time.deltaTime);

        if (Vector3.Distance(transform.position, target) < 0.01f)
        {
            if (!isGoingBack)
            {
                if (currentIndex < points.Count - 1)
                {
                    currentIndex++;
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
                }
                else
                {
                    isGoingBack = false;
                    currentIndex++;
                }
            }
        }
    }

    private void LateUpdate()
    {
        velocity = (transform.position - lastPosition) / Time.deltaTime;
        lastPosition = transform.position;     
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            other.transform.parent = transform;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        other.transform.parent = null;
    }
}
