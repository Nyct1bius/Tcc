using System.Collections;
using UnityEngine;

public class MeleeEnemyStats : MonoBehaviour, IHealth
{
    public MeleeEnemy enemy;

    public float Health;
    public float MovementSpeed;
    public float TimeBetweenAttacks;
    public float MeleeAttackDistance;
    public float KnockbackForce;
    public float RotationSpeed;

    public LayerMask GroundLayer;

    void Start()
    {
        enemy.Agent.speed = MovementSpeed;
        enemy.Agent.stoppingDistance = MeleeAttackDistance;
    }

    public void Damage(float damage, Vector3 DamageSourcePos)
    {
        if(enemy.TookDamage == false)
        {
            enemy.TookDamage = true;
            Health -= damage;

            Vector3 knockbackDir = (transform.position - DamageSourcePos).normalized;
            if (!enemy.HasHyperarmor)
                StartCoroutine(Knockback(knockbackDir, 0.2f));
            else
                StartCoroutine(TookDamage());
        }
        
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

        if (enemy.Stats.Health > 0)
            enemy.Animator.SetTrigger("Hit");
        
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

    private IEnumerator TookDamage()
    {
        yield return new WaitForSeconds(0.1f);
        enemy.TookDamage = false;
    }

    public bool IsGrounded()
    {
        return Physics.Raycast(transform.position, Vector3.down, 10f, GroundLayer);
    }
}
