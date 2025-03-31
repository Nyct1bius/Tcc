using System.Collections;
using UnityEngine;

public class MovablePlataformOcean : MonoBehaviour
{
     private Vector3 startPosition;
    [SerializeField] private float movePlataformTime = 2f;
    [SerializeField] private Transform downPosition;
    [SerializeField] private float timeBetweenStates;
    private float t;


    private enum OceanStates
    {
        down,
        up
    }
    [SerializeField] private OceanStates state;


    private void Start()
    {
        downPosition.position -= transform.position;
        startPosition = transform.position;
        StartCoroutine(ChangeState());
    }
    private void OnEnable()
    {
        OceanTest.OnOceanDown += MoveDown;
        OceanTest.OnOceanUp += MoveUp;
    }

    private void OnDisable()
    {
        OceanTest.OnOceanDown -= MoveDown;
        OceanTest.OnOceanUp -= MoveUp;
    }

    private void MoveDown()
    {
        StartCoroutine(MovePlataform(downPosition.position));
    }

    private void MoveUp()
    {
        StartCoroutine(MovePlataform(startPosition));
    }


    private IEnumerator MovePlataform(Vector3 targetPosition)
    {
        float time = 0f;
        Vector3 startPosition = transform.position;
        while (time < movePlataformTime)
        {
            t = time / movePlataformTime;
            transform.position = Vector3.Lerp(startPosition, targetPosition, t);
            time += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }
        transform.position = targetPosition;
        StartCoroutine(ChangeState());
    }

    IEnumerator ChangeState()
    {
        yield return new WaitForSeconds(timeBetweenStates);
        if (state == OceanStates.down)
        {
            MoveUp();
            state = OceanStates.up;
        }
        else
        {
           MoveDown();
           state = OceanStates.down;
        }
    }
}
