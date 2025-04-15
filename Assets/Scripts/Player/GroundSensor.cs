using UnityEngine;

public class GroundSensor : MonoBehaviour
{
    [Header("Ground Check")]
    [SerializeField] private Transform groundCheckTransform;
    [SerializeField] private float groundCheckRadius;
    [SerializeField] private LayerMask groundMask;

    public bool IsGrounded()
    {
       
        return Physics.CheckSphere(groundCheckTransform.position, groundCheckRadius, groundMask);
    }


    #region Debug

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;

        Gizmos.DrawWireSphere(groundCheckTransform.position, groundCheckRadius);
    }
    #endregion
}

