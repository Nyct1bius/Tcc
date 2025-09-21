using System.Collections;
using UnityEngine;
using DG.Tweening;


public class Door : MonoBehaviour
{
    [SerializeField] private int buttonsActivatedToOpenDoor;
    private int currentButtonsActivated;
    [SerializeField] private Vector3 targetPosition;
    [SerializeField] private float openDoorTime = 2f;
    private Vector3 closedPosition;
    private float t;
    public bool isClosed = true;
    [SerializeField] private GameObject[] gateDoors;
    [SerializeField] private float[] targetAxisRotationsOpen;
    [SerializeField] private float[] targetAxisRotationsClosed;

    void Start()
    {
        closedPosition = transform.position;
        targetPosition += transform.position;

        if(!isClosed)
        {
            isClosed = true;
            OpenDoor();
        }
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
        //StartCoroutine(MoveDoor(targetPosition));
        RotateTheDoor(1);
    }

    [ContextMenu("Close door")]
    public void CloseDoor()
    {
        //StartCoroutine(MoveDoor(closedPosition));
        RotateTheDoor(0);
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
        if (gameObject.GetComponent<BoxCollider>())
            gameObject.GetComponent<BoxCollider>().enabled = false;
    }

    private void RotateTheDoor(int command)
    {
        if(command == 1 && isClosed)
        {
            gateDoors[0].transform.DORotate(new Vector3(0, targetAxisRotationsOpen[0], 0), openDoorTime);
            gateDoors[1].transform.DORotate(new Vector3(0, targetAxisRotationsOpen[1], 0), openDoorTime);

            if (gameObject.GetComponent<BoxCollider>())
                gameObject.GetComponent<BoxCollider>().enabled = false;

            isClosed = false;
        }
        else if (command == 0 && !isClosed)
        {
            gateDoors[0].transform.DORotate(new Vector3(0, targetAxisRotationsClosed[0], 0), openDoorTime);
            gateDoors[1].transform.DORotate(new Vector3(0, targetAxisRotationsClosed[1], 0), openDoorTime);

            if (gameObject.GetComponent<BoxCollider>())
                gameObject.GetComponent<BoxCollider>().enabled = true;

            isClosed = true;
        }
        
    }
}
