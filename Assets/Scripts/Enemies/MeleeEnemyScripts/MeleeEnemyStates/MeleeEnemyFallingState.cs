using System.Collections;
using UnityEngine;

public class MeleeEnemyFallingState : MeleeEnemyState
{
    private MeleeEnemy enemy;

    public MeleeEnemyFallingState(MeleeEnemyStateMachine stateMachine, MeleeEnemy enemy) : base(stateMachine)
    {
        this.enemy = enemy;
    }

    public override void Enter()
    {
        base.Enter();

        enemy.StopAllCoroutines();
        enemy.Agent.enabled = false;
        enemy.StartCoroutine(Fall(2));
    }

    public override void UpdateLogic()
    {
        base.UpdateLogic();

        enemy.transform.position += 6f * Time.deltaTime * Vector3.down;
    }

    private IEnumerator Fall(float timer)
    {
        yield return new WaitForSeconds(timer);

        stateMachine.ChangeState(new MeleeEnemyDeadState(stateMachine, enemy));
    }
}
