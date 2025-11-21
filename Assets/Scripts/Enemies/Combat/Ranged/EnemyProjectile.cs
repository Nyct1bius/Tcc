using System.Collections;
using UnityEngine;

public class EnemyProjectile : MonoBehaviour, IPooledObject
{
    GameObject player;

    [SerializeField] GameObject nonPlayerTarget;

    Transform playerPosition;

    Vector3 targetDirection;

    public float Speed, StartDespawnTimer, DespawnTimer, DamageToPlayer;

    private void Update()
    {
        transform.position += targetDirection * Speed * Time.deltaTime;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (GameManager.instance.PlayerInstance == other.gameObject)
        {
            other.GetComponent<PlayerHealthManager>().Damage(DamageToPlayer, transform.position);

            StartCoroutine(DeactivateAfterHit());
        }
        else if (!other.isTrigger)
        {
            StartCoroutine(DeactivateAfterHit());
        }
    }

    public void OnObjectSpawn()
    {
        StartCoroutine(DeactivateFromTime());

        player = GameManager.instance.PlayerInstance;
        playerPosition = player.transform;

        targetDirection = (playerPosition.position - transform.position).normalized;
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
