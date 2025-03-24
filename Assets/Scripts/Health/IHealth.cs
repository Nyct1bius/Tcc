using UnityEngine;

public interface IHealth  
{

     void TakeDamage(float damage);

     void HealHealth(float healing);

    void Death();
}
