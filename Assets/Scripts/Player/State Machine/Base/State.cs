using UnityEngine;

public abstract class State : MonoBehaviour
{

    protected Core core;

    public bool IsComplete { get; protected set; }

    protected Rigidbody body => core.body;
    protected Animator animator => core.animator;

    protected float startTime;
    public float Time => UnityEngine.Time.time - startTime;


    public virtual void Enter() { }

    public virtual void Do() { }

    public virtual void FixedDo() { }

    public virtual void Exit() { }

    public void SetCore (Core _core)
    {
         this.core = _core;
    }

    public virtual void Initialize()
    {
        IsComplete = false;
        startTime = UnityEngine.Time.time;
    }
}

