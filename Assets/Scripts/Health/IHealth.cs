using UnityEngine;

public interface IHealth  
{

     void Damage(float damage, Vector3 DamageSourcePos);

     void HealHealth(float healing);

    void Death();
}
