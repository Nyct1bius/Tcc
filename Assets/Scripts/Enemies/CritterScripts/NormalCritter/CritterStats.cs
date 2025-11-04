using System.Collections;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;

public class CritterStats : MonoBehaviour, IHealth
{
    public Critter Critter;

    public float Health;
    public float MovementSpeed;
    public float KnockbackForce;

    private void Awake()
    {
        Critter = GetComponent<Critter>();
    }

    void Start()
    {       
        Critter.Agent.speed = MovementSpeed;
    }

    public void Damage(float damage, Vector3 DamageSourcePos)
    {
        Health -= damage;

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
        Critter.Agent.enabled = false;

        yield return new WaitForSeconds(0.1f);

        float timer = 0f;
        while (timer < duration)
        {
            transform.position += direction.normalized * KnockbackForce * Time.deltaTime;
            timer += Time.deltaTime;
            yield return null;
        }

        Critter.Agent.enabled = true;
    }
}
