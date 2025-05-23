using System;
using UnityEngine;

[CreateAssetMenu(menuName = "Weapons/ New Weapon")]
public class WeaponSO : ScriptableObject
{

    public float attackRange;
    public float weaponDamage;
    public GameObject weaponVisual;
    [Range(45f, 180f)]
    public float attackArcAngle;
    [Header("VFX")]
    public GameObject[] vfxAttacks;
    public GameObject vfxHit;
    public enum WeaponType
    {
        Sword,
        Spear,
    }
    public WeaponType Type;
    private Transform _posToAttack;
    public void OnAttack(Transform posToAttack, LayerMask damageableLayer)
    {
        _posToAttack = posToAttack;
        foreach (Collider enemy in GetAllNearbyColliders(damageableLayer))
        {
         
            Vector3 vectorToCollide = (enemy.transform.position - posToAttack.position).normalized;
            float angleToTarget = Vector3.Angle(posToAttack.forward, vectorToCollide);

            if (angleToTarget <= attackArcAngle / 2f)
            {
                OnDamage(enemy);

                //if (!HasWallInfront(enemy.transform))
                //{
                //}
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
            PlayerEvents.OnHitEnemy();
            Instantiate(vfxHit, enemy.transform.position, Quaternion.identity);
            health.Damage(weaponDamage, _posToAttack.position);
        }

    }

    public void Equip() { }
    public void Unequipped() { }
}
