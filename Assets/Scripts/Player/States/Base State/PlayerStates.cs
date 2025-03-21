using UnityEngine;

public abstract class PlayerStates : MonoBehaviour
{

    protected Rigidbody body;
    protected Animator animator;
    protected PlayerMovement inputs;

    public bool isComplete { get; protected set;}

    protected float startTime;
    public float time => Time.time -startTime;


    public virtual void Enter() { }
            
    public virtual void Do() { }
            
    public virtual void FixedDo() { }
            
    public virtual void Exit() { }

    public virtual void Setup(Rigidbody _body, Animator _animator, PlayerMovement _inputs) 
    {
        body = _body;
        animator = _animator;
        inputs = _inputs;
    
    }

    public virtual void Initialize() 
    {
        isComplete = false;
        startTime = Time.time;  
    }
}
