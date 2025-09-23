using UnityEngine;

public class Shield : MonoBehaviour
{
    [SerializeField] private PlayerStateMachine _machine;
    [SerializeField] private GameObject shieldVisual;
    [SerializeField] private float defenseAngle;

    public void ToggleShield(bool isBlocking)
    {
        _machine.IsBlocking = isBlocking;
        shieldVisual.SetActive(_machine.IsBlocking);
        Debug.Log(_machine.IsBlocking ? "Shield is raised." : "Shield is lowered.");
    }

    public bool CanBlock(Vector3 enemyPos)
    {
        if (!_machine.IsBlocking) return false;
        Vector3 vectorToCollide = (enemyPos - transform.position).normalized;
        float angleToTarget = Vector3.Angle(transform.forward, vectorToCollide);
        if (angleToTarget <= defenseAngle / 2f)
        {
            return true;
        }
        else
        {
            return false; 
        }
    }


    private void OnDrawGizmos()
    {
        float gizmoRange = 3f;

        Vector3 leftBoundary = Quaternion.Euler(0, -defenseAngle / 2f, 0) * transform.forward * gizmoRange;
        Vector3 rightBoundary = Quaternion.Euler(0, defenseAngle / 2f, 0) * transform.forward * gizmoRange;

        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position, transform.position + leftBoundary);
        Gizmos.DrawLine(transform.position, transform.position + rightBoundary);
    }


}
