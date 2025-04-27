using UnityEngine;

public class EnemyMeleeHitbox : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        
        if (GameManager.instance.PlayerInstance == other.gameObject)
        {
            other.GetComponent<PlayerHealthManager>().Damage(10, transform.position);
        }
    }
}
