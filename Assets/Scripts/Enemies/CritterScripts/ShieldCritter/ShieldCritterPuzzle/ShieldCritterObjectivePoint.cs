using UnityEngine;

public class ShieldCritterObjectivePoint : MonoBehaviour
{
    [SerializeField] private ShieldCritter critter;
    [SerializeField] private bool isResetPoint;

    private void OnTriggerEnter(Collider other)
    {
        if (GameManager.instance.PlayerInstance)
        {
            if (!isResetPoint)
            {
                critter.IsCornered = true;
                Debug.Log("Cornered");
            }
            else
            {
                critter.IsCornered = false;
                Debug.Log("Not cornered");
            }
        }
    }
}
