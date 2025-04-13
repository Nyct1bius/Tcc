using UnityEngine;

[CreateAssetMenu(menuName = "Weapons/ New Weapon")]
public class WeaponSO : ScriptableObject
{

    public float attackRange;
    public float weaponDamage;
    public GameObject weaponVisual;
    public void OnAttack(Transform posToAttack, LayerMask damageableLayer)
    {
        foreach (Collider enemy in GetAllNearbyColliders(posToAttack, damageableLayer))
        {
            Debug.Log(enemy);
            Vector3 vectorToCollider = enemy.transform.position - posToAttack.position;
            vectorToCollider = vectorToCollider.normalized;

            if (Vector3.Dot(vectorToCollider, posToAttack.forward) > 0.5)
            {
                if (!HasWallInfront(enemy.transform, posToAttack))
                {
                    OnDamage(enemy);
                }
            }
        }
    }

    private Collider[] GetAllNearbyColliders(Transform posToOverlap, LayerMask damageableLayer)
    {
        return Physics.OverlapSphere(posToOverlap.position, attackRange, damageableLayer);
    }

    private bool HasWallInfront(Transform enemyTransform, Transform currentTransform)
    {
        Vector3 direction = (enemyTransform.position - currentTransform.position).normalized;
        Ray ray = new Ray(currentTransform.position, direction);

        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            Debug.DrawLine(currentTransform.position, hit.transform.position, Color.red, 1f);
            if (hit.transform != enemyTransform)
            {
                return true;
            }
        }
        return false;
    }

    private void OnDamage(Collider enemy)
    {
        IHealth health = enemy.GetComponent<IHealth>();
        if (health != null)
        {
            health.Damage(weaponDamage);
        }

    }

    public void Equip() { }
    public void Unequipped() { }
}
