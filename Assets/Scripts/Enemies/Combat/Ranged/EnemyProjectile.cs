using UnityEngine;

public class EnemyProjectile : MonoBehaviour
{
    GameObject player;
    Transform playerPosition;

    Vector3 targetDirection;

    public float Speed;

    private void Start()
    {
        player = GameManager.instance.PlayerInstance;
        playerPosition = player.transform;

        targetDirection = (playerPosition.position - transform.position).normalized;
    }

    private void Update()
    {
        transform.position += targetDirection * Speed * Time.deltaTime;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<PlayerMovement>())
        {
            other.GetComponent<PlayerHealthManager>().Damage(10, transform.position);

            Destroy(gameObject);
        }
        else if (!other.isTrigger)
        {
            Destroy(gameObject);
        }
    }
}
