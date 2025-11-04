using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class CritterDeadState : CritterState
{
    private Critter critter;

    public CritterDeadState(CritterStateMachine stateMachine, Critter critter) : base(stateMachine)
    {
        this.critter = critter;
    }

    public override void Enter()
    {
        base.Enter();

        ChooseDeathAnimation(Random.Range(1, 3));
        critter.GetComponent<CapsuleCollider>().enabled = false;
        critter.StartCoroutine(Despawn(1));
    }

    private IEnumerator Despawn(float timeToDespawn)
    {
        if (critter.Agent.enabled)
            critter.Agent.isStopped = true;

        yield return new WaitForSeconds(timeToDespawn);

        Vector3 originalScale = critter.transform.localScale;
        float timer = 0f;

        while (timer < timeToDespawn)
        {
            float t = 1f - Mathf.Pow(1f - (timer / timeToDespawn), 2f);
            critter.transform.localScale = Vector3.Lerp(originalScale, Vector3.zero, t);
            timer += Time.deltaTime;
            yield return null;
        }

        critter.enabled = false;
    }

    private void ChooseDeathAnimation(int deathAnimationDice)
    {
        if (deathAnimationDice == 1)
            critter.Animator.SetTrigger("DieLeft");
        if (deathAnimationDice == 2)
            critter.Animator.SetTrigger("DieRight");
    }
}
