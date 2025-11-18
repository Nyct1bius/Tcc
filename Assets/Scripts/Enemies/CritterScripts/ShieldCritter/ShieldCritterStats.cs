using UnityEngine;

public class ShieldCritterStats : MonoBehaviour, IHealth
{
    public ShieldCritter critter;

    [SerializeField] private float maxHealth;
    public float CurrentHealth;
    public float MovementSpeed;
    public float RotationSpeed;

    void Start()
    {
        CurrentHealth = maxHealth;
        critter.Agent.speed = MovementSpeed;
    }

    public void Damage(float damage, Vector3 DamageSourcePos)
    {
        CurrentHealth -= damage;
    }

    public void HealHealth(float health)
    {

    }

    public void Death()
    {

    }
}
