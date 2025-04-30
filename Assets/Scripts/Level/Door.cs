using System.Collections;
using UnityEngine;

public class Door : MonoBehaviour
{
    [SerializeField] private int buttonsActivatedToOpenDoor;
    private int currentButtonsActivated;
    [SerializeField] private Vector3 targetPosition;
    [SerializeField] private float openDoorTime = 2f;
    private Vector3 closedPosition;
    private float t;

    void Start()
    {
        closedPosition = transform.position;
        targetPosition += transform.position;
    }


    public void CheckIfAllButtonsIsActivated()
    {
        currentButtonsActivated++;
        if (currentButtonsActivated == buttonsActivatedToOpenDoor) 
        {
            OpenDoor();
        }
    }

    [ContextMenu("Open door")]
    public void OpenDoor()
    {
        StartCoroutine(MoveDoor(targetPosition));
    }

    [ContextMenu("Close door")]
    public void CloseDoor()
    {
        StartCoroutine(MoveDoor(closedPosition));
    }


    private IEnumerator MoveDoor(Vector3 targetPosition)
    {
        float time = 0f;
        Vector3 startPosition = transform.position;
        while (time < openDoorTime)
        {
            t = time / openDoorTime;
            transform.position = Vector3.Lerp(startPosition, targetPosition, t);
            time += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }
        if(gameObject.GetComponent<BoxCollider>())
            gameObject.GetComponent<BoxCollider>().enabled = false;
    }
}
