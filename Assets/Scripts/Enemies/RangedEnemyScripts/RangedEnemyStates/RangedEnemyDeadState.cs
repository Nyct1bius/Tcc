using System.Collections;
using UnityEngine;

public class RangedEnemyDeadState : RangedEnemyState
{
    private RangedEnemy enemy;

    public RangedEnemyDeadState(RangedEnemyStateMachine stateMachine, RangedEnemy enemy) : base(stateMachine)
    {
        this.enemy = enemy;
    }

    public override void Enter()
    {
        base.Enter();

        enemy.StopAllCoroutines();

        ChooseDeathAnimation(Random.Range(1, 3));

        enemy.GetComponent<CapsuleCollider>().enabled = false;

        enemy.StartCoroutine(Despawn(2.2f));

        if (enemy.RoomManager != null)
        {
            enemy.RemoveSelfFromList();
        }
    }

    public override void UpdateLogic()
    {
        base.UpdateLogic();
        // Usually nothing here since enemy is dead
    }

    private IEnumerator Despawn(float timeToDespawn)
    {
        if (enemy.Agent.enabled)
            enemy.Agent.isStopped = true;

        yield return new WaitForSeconds(timeToDespawn);

        Vector3 originalScale = enemy.transform.localScale;
        float timer = 0f;

        while (timer < timeToDespawn)
        {
            float t = 1f - Mathf.Pow(1f - (timer / timeToDespawn), 2f);
            enemy.transform.localScale = Vector3.Lerp(originalScale, Vector3.zero, t);
            timer += Time.deltaTime;
            yield return null;
        }

        enemy.enabled = false;
    }

    private void ChooseDeathAnimation(int deathAnimationDice)
    {
        if (deathAnimationDice == 1)
            enemy.Animator.SetTrigger("Dead1");

        if (deathAnimationDice == 2)
            enemy.Animator.SetTrigger("Dead2");
    }
}
