using UnityEngine;
using PlayerState;
public class AttackState : State
{
    public AttackState(PlayerStateMachine contex, PlayerStateFactory playerStateFactory)
   : base(contex, playerStateFactory) 
    {
        isRootState = true;
    }

    public override void Enter()
    {
        _ctx.Combat.VfxAttack.SetActive(true);
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
        //_ctx.Combat.VfxAttack.SetActive(false);
        _ctx.Animator.SetBool("IsAttacking", false);
        _ctx.Combat.AttackCount++;
    }

    public override void CheckSwitchState()
    {
        if (!_ctx.Combat.AttackIncooldown)
        {
            SwitchStates(_factory.Grounded());
        }

        if (_ctx.Health.IsDamaged)
        {
            SwitchStates(_factory.Damaged());
        }
    }

    public override void InitializeSubState()
    {
        
    }

    private void PerformAttack()
    {
        if (!_ctx.Combat.AttackIncooldown)
        {
            _ctx.Combat.AttackIncooldown = true;
            SelectAttack();
        }
    }

    public void SelectAttack()
    {
        _ctx.Combat.CurrentWeaponData.OnAttack(_ctx.Movement.PlayerTransform, _ctx.Combat.DamageableLayer);
        _ctx.Animator.SetFloat("AttackCount", _ctx.Combat.AttackCount);

        if (_ctx.Combat.AttackCount >= 3)
        {
            _ctx.Body.AddForce(_ctx.transform.forward.normalized * 20f, ForceMode.Impulse);
            _ctx.Combat.AttackCount = 0;
        }


    }
}
