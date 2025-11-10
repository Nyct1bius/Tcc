using UnityEngine;

public class BossMeleeHitbox : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (GameManager.instance.PlayerInstance == other.gameObject)
        {
            other.GetComponent<PlayerHealthManager>().Damage(20, transform.position);
        }
    }
}
