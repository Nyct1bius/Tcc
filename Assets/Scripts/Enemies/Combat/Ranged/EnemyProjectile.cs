using System.Collections;
using UnityEngine;

public class EnemyProjectile : MonoBehaviour, IPooledObject
{
    GameObject player;

    [SerializeField] GameObject nonPlayerTarget;

    Transform playerPosition, nonPlayerTargetPosition;

    Vector3 targetDirection;

    public float Speed, StartDespawnTimer, DespawnTimer;

    [SerializeField] bool isAimedProjectile;

    private void OnEnable()
    {
        
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

            StartCoroutine(DeactivateAfterHit());
        }
        else if (!other.isTrigger)
        {
            StartCoroutine(DeactivateAfterHit());
        }
    }

    public void OnObjectSpawn()
    {
        print("Has spawen from pool");
        // The other functionality comes here
        StartCoroutine(DeactivateFromTime());

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
    IEnumerator DeactivateFromTime()
    {
        yield return new WaitForSeconds(DespawnTimer);
        gameObject.SetActive(false);
    }
    IEnumerator DeactivateAfterHit()
    {
        StopCoroutine(DeactivateFromTime());
        yield return new WaitForSeconds(0.05f);
        gameObject.SetActive(false);

    }
}
