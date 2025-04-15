using UnityEngine;

[CreateAssetMenu(menuName = "Weapons/ New Weapon")]
public class WeaponSO : ScriptableObject
{

    public float attackRange;
    public float weaponDamage;
    public GameObject weaponVisual;
    private Transform _posToAttack;
    public void OnAttack(Transform posToAttack, LayerMask damageableLayer)
    {
        _posToAttack = posToAttack;
        foreach (Collider enemy in GetAllNearbyColliders(damageableLayer))
        {
         
            Vector3 vectorToCollider = enemy.transform.position - posToAttack.position;
            vectorToCollider = vectorToCollider.normalized;

            if (Vector3.Dot(vectorToCollider, posToAttack.forward) > 0.5f)
            {
             
                if (!HasWallInfront(enemy.transform))
                {
                    Debug.Log("Damage");
                    OnDamage(enemy);
                }
            }
        }
    }

    private Collider[] GetAllNearbyColliders(LayerMask damageableLayer)
    {
        return Physics.OverlapSphere(_posToAttack.position, attackRange, damageableLayer);
    }

    private bool HasWallInfront(Transform enemyTransform)
    {
        Vector3 direction = (enemyTransform.position - _posToAttack.position).normalized;
        Ray ray = new Ray(_posToAttack.position, direction);
        Debug.Log(enemyTransform);
        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            Debug.DrawLine(_posToAttack.position, hit.transform.position, Color.red, 1f);
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
