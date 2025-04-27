using UnityEngine;

public class CheckPoint : MonoBehaviour
{
    [SerializeField] private Transform checkPoint;

    public void SetupCheckpoint()
    {
        if (checkPoint != null)
        {
            GameManager.instance.SetCheckpoint(checkPoint);
        }
        else
        {
            GameManager.instance.SetCheckpoint(transform);
        }

        Debug.Log("Checkpoint set");
    }
}
