using System.Collections;
using UnityEngine;

public class RangedEnemyStats : MonoBehaviour, IHealth
{
    public RangedEnemy enemy;

    [SerializeField] private float maxHealth;
    public float CurrentHealth;
    public float MovementSpeed;
    public float TimeBetweenAttacks;
    public float RangedAttackDistance;
    public float KnockbackForce;

    void Start()
    {
        CurrentHealth = maxHealth;

        enemy.Agent.speed = MovementSpeed;
    }

    public void Damage(float damage, Vector3 DamageSourcePos)
    {
        CurrentHealth -= damage;

        Vector3 knockbackDir = (transform.position - DamageSourcePos).normalized;
        StartCoroutine(Knockback(knockbackDir, .4f));

        enemy.TookDamage = true;
    }
    public void HealHealth(float health)
    {

    }

    public void Death()
    {

    }

    private IEnumerator Knockback(Vector3 direction, float duration)
    {
        enemy.Agent.enabled = false;

        yield return new WaitForSeconds(0.1f);

        float timer = 0f;
        while (timer < duration)
        {
            transform.position += direction.normalized * KnockbackForce * Time.deltaTime;
            timer += Time.deltaTime;
            yield return null;
        }

        enemy.TookDamage = false;

        if (CurrentHealth > 0)
            enemy.Agent.enabled = true;
    }
}
