using UnityEngine;

public class CheckPoint : MonoBehaviour
{
    [SerializeField] private Transform checkPoint;
    [SerializeField] private int checkpointIndex;

    public void SetupCheckpoint()
    {
        if (checkPoint == null)
        {
            GameManager.instance.SetCheckpoint(transform, checkpointIndex);
            return;
        }

        GameManager.instance.SetCheckpoint(checkPoint, checkpointIndex);

        Debug.Log("Checkpoint set");
    }
}
