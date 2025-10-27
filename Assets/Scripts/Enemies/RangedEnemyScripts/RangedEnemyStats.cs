using System.Collections;
using UnityEngine;

public class RangedEnemyStats : MonoBehaviour, IHealth
{
    public RangedEnemy enemy;

    public float Health;
    public float MovementSpeed;
    public float TimeBetweenAttacks;
    public float RangedAttackDistance;
    public float KnockbackForce;

    public LayerMask GroundLayer;

    void Start()
    {
        enemy.Agent.speed = MovementSpeed;
        enemy.Agent.stoppingDistance = RangedAttackDistance;
    }

    public void Damage(float damage, Vector3 DamageSourcePos)
    {
        Health -= damage;
        enemy.TookDamage = true;

        Vector3 knockbackDir = (transform.position - DamageSourcePos).normalized;
        StartCoroutine(Knockback(knockbackDir, .2f));
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
        if (Health > 0)
            enemy.Agent.enabled = true;
    }

    public bool IsGrounded()
    {
        return Physics.Raycast(transform.position, Vector3.down, 5f, GroundLayer);
    }
}
