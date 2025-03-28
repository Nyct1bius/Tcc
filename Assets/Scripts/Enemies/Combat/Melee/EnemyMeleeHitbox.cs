using UnityEngine;

public class EnemyMeleeHitbox : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<PlayerMovement>())
        {
            other.GetComponent<PlayerHealthManager>();
        }
    }
}
