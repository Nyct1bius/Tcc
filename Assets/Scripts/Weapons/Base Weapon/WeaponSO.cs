using System;
using UnityEngine;

[CreateAssetMenu(menuName = "Weapons/New Weapon")]
public class WeaponSO : ScriptableObject
{
    public AttackData[] attacks;
    public float weaponDamage;
    public GameObject weaponVisual;
    public LayerMask damageableLayer;

    public enum WeaponType
    {
        Sword,
        Spear,
    }
    public WeaponType Type;

    public void OnAttack(Transform posToAttack, LayerMask damageableLayer, int performedAttack)
    {
        foreach (Collider enemy in GetAllNearbyColliders(posToAttack, damageableLayer, performedAttack))
        {
            Vector3 vectorToCollide = (enemy.transform.position - posToAttack.position).normalized;
            float angleToTarget = Vector3.Angle(posToAttack.forward, vectorToCollide);
            if (angleToTarget <= attacks[performedAttack].attackArcAngle / 2f)
            {
                if (!HasWallInfront(posToAttack, enemy.transform, attacks[performedAttack].attackRange))
                {
                    OnDamage(enemy, posToAttack, performedAttack);
                }
            }
        }
    }

    private Collider[] GetAllNearbyColliders(Transform posToAttack, LayerMask damageableLayer, int performedAttack)
    {
        return Physics.OverlapSphere(
            posToAttack.position,
            attacks[performedAttack].attackRange,
            damageableLayer
        );
    }

    private bool HasWallInfront(Transform posToAttack, Transform enemyTransform, float attackRange)
    {
        Vector3 direction = (enemyTransform.position - posToAttack.position).normalized;
        if (Physics.Raycast(posToAttack.position, direction, out RaycastHit hit, attackRange))
        {
            Debug.DrawLine(posToAttack.position, hit.point, Color.red, 1f);
            if (hit.transform != enemyTransform) return true;
        }
        return false;
    }

    private void OnDamage(Collider enemy, Transform posToAttack, int performedAttack)
    {
        IHealth health = enemy.GetComponent<IHealth>();
        if (health != null)
        {
            PlayerEvents.OnHitEnemy(enemy.transform.position);

            if (attacks[performedAttack].vfxHit != null)
                Instantiate(attacks[performedAttack].vfxHit, enemy.transform.position, Quaternion.identity);

            health.Damage(weaponDamage, posToAttack.position);
        }
    }

    public void Equip() { }
    public void Unequipped() { }
}

[Serializable]
public class AttackData
{
    public float attackRange;
    [Range(45f, 360f)] public float attackArcAngle;

    [Header("VFX")]
    public GameObject vfxAttacks;
    public GameObject vfxHit;

    [Header("Animation")]
    public AnimationClip attackAnimationClip;
}
