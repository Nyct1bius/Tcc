using UnityEngine;
using PlayerState;
public class DamagedState : State
{
    public DamagedState(PlayerStateMachine contex, PlayerStateFactory playerStateFactory)
    : base(contex, playerStateFactory)
    {
        isRootState = true;
    }

    public override void Enter()
    {
        _ctx.Combat.SpawnVFXLightining();
        _ctx.PlayerAnimator.SetTrigger("Hited");
        _ctx.Health.CurrentHealth -=  _ctx.Health.DamageToReceive;
        _ctx.Health.IsInvulnerable = true;
        _ctx.Health.PlayerUIManager.AtualizePlayerHealthUI(_ctx.Health.CurrentHealth);
        _timeInState = 0;
        Knockback();
    }

    public override void Do()
    {
        CheckSwitchState();
    }

    public override void Exit()
    {
        _ctx.Health.IsInvulnerable = false;
        _ctx.Health.IsDamaged = false;
        _ctx.Combat.AttackIncooldown = false;
    }
    public override void FixedDo()
    {
       
    }
    public override void CheckSwitchState()
    {
        _timeInState += Time.deltaTime;
        if (_ctx.Health.CurrentHealth <= 0)
        {
            SwitchStates(_factory.Death());
        }
        if (_timeInState > _ctx.Health.ImmortalityTime)
        {
            SwitchStates(_factory.Grounded());
        }
    }
    public override void InitializeSubState()
    {
      
    }

    private void Knockback()
    {
        _ctx.Body.AddForce(_ctx.Health.KnockBackDirection * _ctx.Health.HorizontalKnockBackForce + (Vector3.up * _ctx.Health.VerticalKnockBackForce), ForceMode.Impulse);
    }
}
