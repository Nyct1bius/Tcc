using System.Collections;
using UnityEngine;

public class CritterStats : MonoBehaviour, IHealth
{
    public Critter critter;

    [SerializeField] private float maxHealth;
    public float CurrentHealth;
    public float MovementSpeed;
    public float KnockbackForce;

    void Start()
    {
        CurrentHealth = maxHealth;

        critter.Agent.speed = MovementSpeed;
    }

    public void Damage(float damage, Vector3 DamageSourcePos)
    {
        CurrentHealth -= damage;

        Vector3 knockbackDir = (transform.position - DamageSourcePos).normalized;
        StartCoroutine(Knockback(knockbackDir, .4f));
    }

    public void HealHealth(float health)
    {

    }

    public void Death()
    {

    }

    private IEnumerator Knockback(Vector3 direction, float duration)
    {
        critter.Agent.enabled = false;

        yield return new WaitForSeconds(0.1f);

        float timer = 0f;
        while (timer < duration)
        {
            transform.position += direction.normalized * KnockbackForce * Time.deltaTime;
            timer += Time.deltaTime;
            yield return null;
        }

        critter.Agent.enabled = true;
    }
}
