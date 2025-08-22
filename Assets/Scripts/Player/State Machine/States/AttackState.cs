using PlayerState;
using System;
using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.EventSystems;
using UnityEngine.Playables;

public class AttackState : State
{
    public AttackState(PlayerStateMachine contex, PlayerStateFactory playerStateFactory)
   : base(contex, playerStateFactory)
    {
        isRootState = true;
    }
    public override void Enter()
    {
        PerformAttack();
        PlayerEvents.StartAttackDetection += StartAttackCollisionDetection;
    }

    public override void Do()
    {
        CheckSwitchState();
        _timeInState += Time.deltaTime;
    }

    public override void FixedDo() { }

    public override void Exit()
    {
        PlayerEvents.StartAttackDetection -= StartAttackCollisionDetection;
        _ctx.Combat.AttackCount++;
        if (_ctx.Combat.AttackCount >= _ctx.Combat.CurrentWeaponData.attacks.Length)
        {
            _ctx.Combat.AttackCount = 0;
        }
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

    public override void InitializeSubState() { }

    private void PerformAttack()
    {
        if (!_ctx.Combat.AttackIncooldown)
        {
            _ctx.Combat.AttackIncooldown = true;
            LockToClosestEnemy();
        }
    }

    public void SelectAttack()
    {
        PlayAttackAnimation();
    }

    private void StartAttackCollisionDetection()
    {
        _ctx.Combat.CurrentWeaponData.OnAttack(_ctx.Movement.PlayerTransform, _ctx.Combat.DamageableLayer, _ctx.Combat.AttackCount);
    }

    private void LockToClosestEnemy()
    {
        _ctx.Combat.GetClosestTargets();

        if (_ctx.Combat.DetectedEnemys.Count > 0)
        {
            float range = _ctx.Combat.CurrentWeaponData.attacks[_ctx.Combat.AttackCount].attackRange;
            Transform targetPos = _ctx.Combat.DetectedEnemys[0].transform;

            Vector3 dirEnemyToPlayer = (_ctx.Movement.PlayerTransform.position - targetPos.position).normalized;
            Vector3 posToMove = targetPos.position + dirEnemyToPlayer * (range * 0.4f);
            posToMove.y = _ctx.Movement.PlayerTransform.position.y;

            _ctx.StartCoroutine(SmoothDash(posToMove, targetPos, 0.15f)); // duração do dash suave
        }

        SelectAttack();
    }

    private System.Collections.IEnumerator SmoothDash(Vector3 targetPos, Transform enemy, float duration)
    {
        Transform player = _ctx.Movement.PlayerTransform;
        Vector3 startPos = player.position;
        Quaternion startRot = player.rotation;

        Vector3 lookDir = (enemy.position - player.position).normalized;
        Quaternion targetRot = Quaternion.LookRotation(new Vector3(lookDir.x, 0, lookDir.z));

        float time = 0f;

        while (time < duration)
        {
            time += Time.deltaTime;
            float t = time / duration;

            // easing exponencial (mais rápido no começo, suave no final)
            float easedT = 1f - Mathf.Pow(2f, -10f * t);

            player.position = Vector3.Lerp(startPos, targetPos, easedT);
            player.rotation = Quaternion.Slerp(startRot, targetRot, easedT);

            yield return null;
        }

        player.position = targetPos;
        player.rotation = targetRot;
    }

    private void PlayAttackAnimation()
    {
        _ctx.AnimationSystem.PlayAttack(_ctx.Combat.CurrentWeaponData.attacks[_ctx.Combat.AttackCount].attackAnimationClip);
    }
}
