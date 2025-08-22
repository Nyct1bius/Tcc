using UnityEngine;

public class GroundSensor : MonoBehaviour
{
    [Header("Ground Check")]
    [SerializeField] private Transform groundCheckTransform;
    [SerializeField] private float groundCheckRadius;
    [SerializeField] private LayerMask groundMask;

    private Collider[] hitColliders = new Collider[3];

    public bool IsGrounded()
    {
        int hits = Physics.OverlapSphereNonAlloc(
            groundCheckTransform.position,
            groundCheckRadius,
            hitColliders,
            groundMask
        );

        return hits > 0;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(groundCheckTransform.position, groundCheckRadius);
    }
}
