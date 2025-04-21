using UnityEngine;

public class CheckPoint : MonoBehaviour
{
    [SerializeField] private Transform checkPoint;

    public void SetupCheckpoint()
    {
        if (checkPoint != null)
        {
            GameManager.instance.SetCheckpoint(checkPoint.position);
        }
        else
        {
            GameManager.instance.SetCheckpoint(transform.position);
        }

        Debug.Log("Checkpoint set");
    }
}
