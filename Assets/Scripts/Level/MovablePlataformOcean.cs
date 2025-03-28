using System.Collections;
using UnityEngine;

public class MovablePlataformOcean : MonoBehaviour
{
     private Vector3 upPositionPosition;
    [SerializeField] private float movePlataformTime = 2f;
    [SerializeField] private Vector3 downPosition;
    private float t;
    private void Start()
    {
        downPosition -= transform.position;
        upPositionPosition = transform.position;
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
        StartCoroutine(MovePlataform(downPosition));
    }

    private void MoveUp()
    {
        StartCoroutine(MovePlataform(upPositionPosition));
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
    }
}
