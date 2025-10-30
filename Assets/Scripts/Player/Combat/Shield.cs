using UnityEngine;

public class Shield : MonoBehaviour
{
    [SerializeField] private PlayerStateMachine _machine;
    [SerializeField] private GameObject shieldVisual;
    [SerializeField] private float defenseAngle;
    private Animator _shieldAnimator;
    private void Awake()
    {
        _shieldAnimator = shieldVisual.GetComponent<Animator>();    
    }
    public void ToggleShield(bool isBlocking)
    {
        if (!_machine.Movement.IsGrounded) return;

        _machine.IsBlocking = isBlocking;
       if(_machine.IsBlocking)
        {
            shieldVisual.SetActive(true);
            _shieldAnimator.Play("Shield_Open");
        }
        else if(shieldVisual.activeInHierarchy)
        {

            _shieldAnimator.Play("Shield_Close");
        }
    }
    public void ResetShield()
    {
        _shieldAnimator.Play("Shield_Close");
        shieldVisual.SetActive(false);
        _machine.IsBlocking = false;
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
