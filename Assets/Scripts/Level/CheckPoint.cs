using System.Collections;
using UnityEngine;

public class CheckPoint : MonoBehaviour
{
    [SerializeField] private Transform checkPoint;
    [SerializeField] private int checkpointIndex;
    public GameObject standingLamp;
    public Material meshMaterial;

    private void Start()
    {
        if (standingLamp != null)
            meshMaterial = standingLamp.GetComponent<MeshRenderer>().material;
    }

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

    private void OnTriggerEnter(Collider other)
    {
        if (GameManager.instance.PlayerInstance == other.gameObject)
        {
            if(meshMaterial != null)
                meshMaterial.SetFloat("_Has_Emission_Map", true ? 1f : 0f);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (GameManager.instance.PlayerInstance == other.gameObject)
        {
            StartCoroutine(DelayedTurnOff());
        }
    }

    private IEnumerator DelayedTurnOff()
    {
        yield return new WaitForSeconds(25f);
        if (meshMaterial != null)
            meshMaterial.SetFloat("_Has_Emission_Map", true ? 0f : 1f);

    }
}
