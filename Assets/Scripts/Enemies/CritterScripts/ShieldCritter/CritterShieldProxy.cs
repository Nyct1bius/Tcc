using UnityEngine;

public class CritterShieldProxy : MonoBehaviour
{
    ShieldCritter critter;

    public void SpawnCritterShield()
    {
        critter.SpawnShield();
    }
}
