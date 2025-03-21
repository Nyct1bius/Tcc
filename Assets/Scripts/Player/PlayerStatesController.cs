using UnityEngine;

public class PlayerStatesController : MonoBehaviour
{
    private PlayerStates currentState;
    private PlayerStates oldState;
    [SerializeField] private IdleState IdleState;

    private void Start()
    {
        currentState = IdleState;
        currentState.Enter();
    }

    private void Update()
    {
        currentState.Do();
    }
    private void FixedUpdate()
    {
        currentState.FixedDo();
    }
    public void ChangePlayerState( PlayerStates newState)
    {
        currentState.Exit();
        currentState = newState;
        currentState.Initialize();
        currentState.Enter();   

    }
}
