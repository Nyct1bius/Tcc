using System.Collections;
using UnityEngine;

public class BossStats : MonoBehaviour, IHealth
{
    public Boss Boss;
    public float Health, MovementSpeed, TimeBetweenAttacks;
    
    public void Damage(float damage, Vector3 damageSourcePos)
    {
        if (!Boss.TookDamage)
        {
            Boss.TookDamage = true;
            Health -= damage;

            StartCoroutine(TookDamage());
        }
    }

    private IEnumerator TookDamage()
    {
        yield return new WaitForSeconds(0.2f);
        Boss.TookDamage = false;
    }

    public void HealHealth(float health)
    {

    }

    public void Death()
    {

    }
}
