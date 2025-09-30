using System.Collections;
using UnityEngine;

public class ShieldCritterDeadState : ShieldCritterState
{
    private ShieldCritter critter;

    public ShieldCritterDeadState(ShieldCritterStateMachine stateMachine, ShieldCritter critter) : base(stateMachine)
    {
        this.critter = critter;
    }

    public override void Enter()
    {
        base.Enter();

        critter.StopAllCoroutines();
        critter.Animator.SetTrigger("Die");
        critter.GetComponent<CapsuleCollider>().enabled = false;
        critter.RemoveSelfFromList();
        critter.StartCoroutine(Despawn(2.2f));
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
}
