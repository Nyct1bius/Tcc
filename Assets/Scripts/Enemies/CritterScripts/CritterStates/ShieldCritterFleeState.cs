using UnityEngine;

public class ShieldCritterFleeState : CritterState
{
    private Critter critter;

    private Vector3 playerPosition;

    public ShieldCritterFleeState(CritterStateMachine stateMachine, Critter critter) : base(stateMachine)
    {
        this.critter = critter;
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
