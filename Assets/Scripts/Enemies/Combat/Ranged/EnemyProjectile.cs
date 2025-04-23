using UnityEngine;

public class EnemyProjectile : MonoBehaviour
{
    [SerializeField] private GameObject player;
    private Transform playerPosition;

    public float Speed;

    private Vector3 targetDirection;

    private void Start()
    {
        //player = GameManager.instance.playerInstance;
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
            other.GetComponent<PlayerHealthManager>().Damage(10);

            Destroy(gameObject);
        }
        else if (!other.isTrigger)
        {
            Destroy(gameObject);
        }
    }
}
