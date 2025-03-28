using UnityEngine;

public class SteppingTrigger : MonoBehaviour
{
    [SerializeField] private Material pressedMat;
    [SerializeField] private Button[] ButtonsToActivate;
    private MeshRenderer triggerMeshRenderer;

    private void Awake()
    {
        triggerMeshRenderer = GetComponent<MeshRenderer>();
    }

    private void OnTriggerEnter(Collider other)
    {
        PlayerMovement player = other.GetComponent<PlayerMovement>();

        if(player != null )
        {
            ActivateTrigger();
        }
    }

    private void ActivateTrigger()
    {
        triggerMeshRenderer.material = pressedMat;
        for( int i = 0; i < ButtonsToActivate.Length; i++ )
        {
            ButtonsToActivate[i].ActivatedButton();   
        }
        
    }
}
