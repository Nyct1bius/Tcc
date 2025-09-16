using UnityEngine;

public abstract class CritterState
{
    protected CritterStateMachine stateMachine;

    public CritterState(CritterStateMachine stateMachine)
    {
        this.stateMachine = stateMachine;
    }

    public virtual void Enter() { }
    public virtual void UpdateLogic() { }
    public virtual void FixedUpdateLogic() { }
    public virtual void Exit() { }
}

public class CritterStateMachine : MonoBehaviour
{
    private CritterState currentState;

    public void InitializeStateMachine(CritterState startingState)
    {
        currentState = startingState;
        currentState.Enter();
    }

    public void ChangeState(CritterState newState)
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
