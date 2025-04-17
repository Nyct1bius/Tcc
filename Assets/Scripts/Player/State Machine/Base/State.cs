public abstract class State
{
    protected bool isRootState = false;
    protected PlayerStateMachine _ctx;
    protected PlayerStateFactory _factory;
    protected State _currentSuperState;
    protected State _currentSubState;

    public State (PlayerStateMachine ctx, PlayerStateFactory factory)
    {
        _ctx = ctx;
        _factory = factory;
    }

    public abstract void Enter() ;
         
    public abstract void Do() ;

    public abstract void FixedDo();

    public abstract void Exit();
    public abstract void CheckSwitchState();
    public abstract void InitializeSubState();


    public void UpdateStates() 
    {
        Do();
        _currentSubState?.UpdateStates();
    }

    public void FixedUpdateStates()
    {
        FixedDo();
        _currentSubState?.FixedUpdateStates();
    }
    protected void SwitchStates(State newState) 
    {
        _ctx.OldState = _ctx.CurrentState;
        //Current State exit 
        Exit();

        newState.Enter();
        if (isRootState)
        {
            _ctx.CurrentState = newState;
        }else if(_currentSuperState != null)
        {
            _currentSuperState.SetSubState(newState);
        }
        
    }
    protected void SetSuperState(State newSuperState) 
    {
        _currentSuperState = newSuperState;
    }

    protected void SetSubState(State newSubState) 
    {
        _currentSubState= newSubState;
        newSubState.SetSuperState(this);
    }


           
}

