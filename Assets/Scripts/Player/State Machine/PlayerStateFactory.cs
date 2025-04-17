
public class PlayerStateFactory 
{
    PlayerStateMachine _contex;

    public PlayerStateFactory (PlayerStateMachine contex)
    {
      
        _contex = contex; 
    
    }

    public State Idle() 
    {
        return new IdleState(_contex, this);
    }
    public State Jump()
    {
        return new JumpState(_contex, this);
    }
    public State Walk()
    {
        return new WalkState(_contex, this);
    }
    public State Attack()
    {
        return new AttackState(_contex, this);
    }
    public State Grounded()
    {
        return new GroundState(_contex, this);
    }
    public State Dash()
    {
        return new DashState(_contex, this);
    }

    public State Fall()
    {
        return new FallState(_contex, this);
    }

    public State PausedState()
    {
        return new PausedState(_contex, this);
    }

}
