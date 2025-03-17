using UnityEngine;

public abstract class PlayerStates : MonoBehaviour
{

    protected Rigidbody body;
    protected Animator animator;
    protected InputManager inputManager;

    public bool isComplete { get; protected set;}


    public virtual void Enter() { }
            
    public virtual void Do() { }
            
    public virtual void FixedDo() { }
            
    public virtual void Exit() { }

    public virtual void Setup(Rigidbody _body, Animator _animator,InputManager _inputManager) 
    {
        body = _body;
        animator = _animator;
        inputManager = _inputManager;
    
    }
}
