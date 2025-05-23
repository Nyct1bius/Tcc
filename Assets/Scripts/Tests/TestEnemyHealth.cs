using System.Collections;
using UnityEngine;

public class TestEnemyHealth : MonoBehaviour, IHealth
{
    [SerializeField] private float maxHealth;
    [SerializeField] private float immortalityTime;
    private bool isImmortal;
    private float currenthealth;

    private void Start()
    {
        currenthealth = maxHealth;
    }

    public void HealHealth(float healing)
    {
        throw new System.NotImplementedException();
    }

    public void Damage(float damage, Vector3 DamageSourcePos)
    {
        if (!isImmortal)
        {
            //currenthealth -= damage;
            isImmortal = true;
            StartCoroutine(Immortal());
            transform.position += Vector3.forward * -1f;
        }
        
    }

    private IEnumerator Immortal()
    {
        yield return new WaitForSeconds(immortalityTime);
        isImmortal = false;
    }


    public void Death()
    {
        currenthealth = maxHealth;
    }

}
