using UnityEngine;

public abstract class BossState
{
    protected BossStateMachine stateMachine;

    public BossState(BossStateMachine stateMachine)
    {
        this.stateMachine = stateMachine;
    }

    public virtual void Enter() { }
    public virtual void Exit() { }
    public virtual void UpdatePhysics() { }
    public virtual void UpdateLogic() { }
}

public class BossStateMachine : MonoBehaviour
{
    private BossState currentState;
    
    public void InitializeStateMachine(BossState startingState)
    {
        currentState = startingState;
        currentState.Enter();
    }

    public void ChangeState(BossState newState)
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
