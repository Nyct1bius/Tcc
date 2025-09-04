using System.Collections;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Splines;

public class MeleeEnemyStats : MonoBehaviour, IHealth
{
    public MeleeEnemy enemy;
    
    [SerializeField] private float maxHealth;
    public float CurrentHealth;
    public float MovementSpeed;
    public float TimeBetweenAttacks;
    public float MeleeAttackDistance;
    public float KnockbackForce;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        CurrentHealth = maxHealth;

        enemy.Agent.speed = MovementSpeed;
    }

    // Update is called once per frame
    void Update()
    {
        
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
        Debug.Log("This shouldn't happen!");
    }

    public void Death()
    {
        Debug.Log("Enemy died!");
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
        enemy.Agent.enabled = true;
    }
}
