using UnityEngine;

public abstract class Core : MonoBehaviour
{
    public Rigidbody body;
    public Animator animator;

    public GroundSensor groundSensor;

    public StateMachine machine;

    public void SetupInstances()
    {
        machine = new StateMachine();

        State[] AllStateChildren = GetComponentsInChildren<State>();
        foreach (State state in AllStateChildren)
        {
            state.SetCore(this);
        }

    }
}
