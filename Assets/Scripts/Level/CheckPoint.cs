using UnityEngine;

public class CheckPoint : MonoBehaviour
{
    [SerializeField] private Transform checkPoint;
    [SerializeField] private int checkpointIndex;

    public void SetupCheckpoint()
    {
        if (checkPoint != null)
        {
            GameManager.instance.SetCheckpoint(checkPoint, checkpointIndex);
        }
        else
        {
            GameManager.instance.SetCheckpoint(transform, checkpointIndex);
        }

        Debug.Log("Checkpoint set");
    }
}
