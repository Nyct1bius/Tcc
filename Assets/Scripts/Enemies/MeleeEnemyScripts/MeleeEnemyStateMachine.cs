using UnityEngine;

public abstract class MeleeEnemyState
{
    protected MeleeEnemyStateMachine stateMachine;

    public MeleeEnemyState(MeleeEnemyStateMachine stateMachine)
    {
        this.stateMachine = stateMachine;
    }

    public virtual void Enter() { }
    public virtual void UpdateLogic() { }
    public virtual void UpdatePhysics() { }
    public virtual void Exit() { }
}

public class MeleeEnemyStateMachine : MonoBehaviour
{
    private MeleeEnemyState currentState;

    public void InitializeStateMachine(MeleeEnemyState startingState)
    {
        currentState = startingState;
        currentState.Enter();
    }

    public void ChangeState(MeleeEnemyState newState)
    {
        currentState.Exit();
        currentState = newState;
        currentState.Enter();
    }

    private void Update()
    {
        currentState?.UpdateLogic();
    }

    private void FixedUpdate()
    {
        currentState?.UpdatePhysics();
    }
}
