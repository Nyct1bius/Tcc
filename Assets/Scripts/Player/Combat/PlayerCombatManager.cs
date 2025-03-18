using UnityEngine;

public class PlayerCombatManager : MonoBehaviour
{
    private bool hasSword;
    private int attackCounts;
    private void OnEnable()
    {
        PlayerEvents.Attack += PerformAttack;
        PlayerEvents.SwordPickUp += CheckIfHasSword;
    }
    private void OnDisable()
    {
        PlayerEvents.Attack -= PerformAttack;
        PlayerEvents.SwordPickUp -= CheckIfHasSword;
    }

    private void CheckIfHasSword()
    {
        hasSword = true;
    }

    private void PerformAttack()
    {
        if(hasSword)
        {
            Debug.Log("Attack");
        }
    }
}
