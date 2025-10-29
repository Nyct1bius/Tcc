using UnityEngine;

public class CritterShieldProxy : MonoBehaviour
{
    ShieldCritter critter;

    private void Start()
    {
        critter = GetComponentInParent<ShieldCritter>();
    }

    public void SpawnCritterShield()
    {
        critter.SpawnShield();
    }
}
