using UnityEngine;

public class CheckPoint : MonoBehaviour
{
    [SerializeField] private Transform checkPoint;

    public void SetupCheckpoint()
    {
        if (checkPoint != null)
        {
            GameManager.instance.checkPoint = checkPoint;
        }
        else
        {
            GameManager.instance.checkPoint = transform;
        }

        Debug.Log("Checkpoint set");
    }
}
