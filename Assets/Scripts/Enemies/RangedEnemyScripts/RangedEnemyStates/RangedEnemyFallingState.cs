using System.Collections;
using UnityEngine;

public class RangedEnemyFallingState : RangedEnemyState
{
    private RangedEnemy enemy;

    public RangedEnemyFallingState(RangedEnemyStateMachine stateMachine, RangedEnemy enemy) : base(stateMachine)
    {
        this.enemy = enemy;
    }

    public override void Enter()
    {
        base.Enter();

        enemy.StopAllCoroutines();

        enemy.Agent.enabled = false;

        enemy.StartCoroutine(Fall(3));
    }

    public override void UpdateLogic()
    {
        base.UpdateLogic();

        enemy.transform.position += Vector3.down * 7f * Time.deltaTime;
    }

    private IEnumerator Fall(float timer)
    {
        yield return new WaitForSeconds(timer);

        stateMachine.ChangeState(new RangedEnemyDeadState(stateMachine, enemy));
    }
}
