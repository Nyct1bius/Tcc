using UnityEngine;

public class EnemyMeleeHitbox : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        Debug.Log(other.name);
        
        if (GameManager.instance.playerInstance == other.gameObject)
        {
            other.GetComponent<PlayerHealthManager>().Damage(10);
        }
    }
}
