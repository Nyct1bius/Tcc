using UnityEngine;

public abstract class RangedEnemyState
{
    protected RangedEnemyStateMachine stateMachine;

    public RangedEnemyState(RangedEnemyStateMachine stateMachine)
    {
        this.stateMachine = stateMachine;
    }

    public virtual void Enter() { }
    public virtual void UpdateLogic() { }
    public virtual void UpdatePhysics() { }
    public virtual void Exit() { }
}

public class RangedEnemyStateMachine : MonoBehaviour
{
    private RangedEnemyState currentState;

    public void InitializeStateMachine(RangedEnemyState startingState)
    {
        currentState = startingState;
        currentState.Enter();
    }

    public void ChangeState(RangedEnemyState newState)
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
