using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Windows;

public class AttackState : State
{
    public AttackState(PlayerStateMachine contex, PlayerStateFactory playerStateFactory)
   : base(contex, playerStateFactory) 
    {
        isRootState = true;
    }

    public override void Enter()
    {
        _ctx.Animator.SetBool("IsAttacking", true);
        PerformAttack();
    }

    public override void Do()
    {
        CheckSwitchState();
    }

    public override void FixedDo() { }

    public override void Exit() 
    {
        _ctx.Animator.SetBool("IsAttacking", false);
    }

    public override void CheckSwitchState()
    {
        if (!_ctx.IsAttacking && !_ctx.AttackIncooldown)
        {
            SwitchStates(_factory.Grounded());
        }
    }

    public override void InitializeSubState()
    {
        
    }

    private void PerformAttack()
    {
        if (!_ctx.AttackIncooldown)
        {
            _ctx.AttackIncooldown = true;
            _ctx.Animator.SetFloat("AttackCount", _ctx.AttackCount);
            Debug.Log("Attack");
            SelectAttack();
        }
    }

    public void SelectAttack()
    {
        _ctx.CurrentWeaponData.OnAttack(_ctx.PlayerTransform, _ctx.DamageableLayer);

        _ctx.AttackCount++;

        if (_ctx.AttackCount >= 3)
        {
            _ctx.AttackCount = 0;
        }

    }
}
