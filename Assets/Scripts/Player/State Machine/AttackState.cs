using Unity.VisualScripting;
using UnityEngine;

public class AttackState : State
{
    [SerializeField] AnimationClip[] attackAnimations;
    [SerializeField] PlayerCombatManager combatManager;


    public override void Enter()
    {
        animator.Play(attackAnimations[combatManager.attackCount].name);
    }

    public override void Do()
    {

    }

    public override void FixedDo() { }

    public override void Exit() 
    {
    }
}
