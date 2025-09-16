using UnityEngine;

public abstract class ShieldCritterState
{
    protected ShieldCritterStateMachine stateMachine;

    public ShieldCritterState(ShieldCritterStateMachine stateMachine)
    {
        this.stateMachine = stateMachine;
    }

    public virtual void Enter() { }
    public virtual void UpdateLogic() { }
    public virtual void FixedUpdateLogic() { }
    public virtual void Exit() { }
}

public class ShieldCritterStateMachine : MonoBehaviour
{
    private ShieldCritterState currentState;

    public void InitializeStateMachine(ShieldCritterState startingState)
    {
        currentState = startingState;
        currentState.Enter();
    }

    public void ChangeState(ShieldCritterState newState)
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
        currentState?.FixedUpdateLogic();
    }
}
