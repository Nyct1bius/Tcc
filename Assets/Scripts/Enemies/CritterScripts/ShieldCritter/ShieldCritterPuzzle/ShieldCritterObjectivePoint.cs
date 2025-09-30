using UnityEngine;

public class ShieldCritterObjectivePoint : MonoBehaviour
{
    [SerializeField] private ShieldCritter critter;

    private void OnTriggerEnter(Collider other)
    {
        if (GameManager.instance.PlayerInstance && critter.IsCornered != true)
        {
            critter.IsCornered = true;
            gameObject.SetActive(false);
        }
    }
}
