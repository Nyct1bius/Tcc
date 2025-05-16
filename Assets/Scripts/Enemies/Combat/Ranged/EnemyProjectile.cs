using UnityEngine;

public class EnemyProjectile : MonoBehaviour
{
    GameObject player;

    [SerializeField] GameObject nonPlayerTarget;

    Transform playerPosition, nonPlayerTargetPosition;

    Vector3 targetDirection;

    public float Speed, StartDespawnTimer, DespawnTimer;

    [SerializeField] bool isAimedProjectile;

    private void OnEnable()
    {
        if (isAimedProjectile)
        {
            player = GameManager.instance.PlayerInstance;
            playerPosition = player.transform;

            targetDirection = (playerPosition.position - transform.position).normalized;
        }
        else
        {
            nonPlayerTargetPosition = nonPlayerTarget.transform;

            targetDirection = (nonPlayerTargetPosition.position - transform.position).normalized;
        }
    }

    private void Update()
    {
        transform.position += targetDirection * Speed * Time.deltaTime;

        if (DespawnTimer <= 0)
        {
            gameObject.SetActive(false);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<PlayerMovement>())
        {
            other.GetComponent<PlayerHealthManager>().Damage(10, transform.position);

            gameObject.SetActive(false);
        }
        else if (!other.isTrigger)
        {           
            gameObject.SetActive(false);
        }
    }
}
