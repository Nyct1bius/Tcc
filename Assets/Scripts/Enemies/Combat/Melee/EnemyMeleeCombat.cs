using System.Collections;
using UnityEngine;

public class EnemyMeleeCombat : MonoBehaviour
{
    [SerializeField] private float timeBetweenAttacks = 1.5f;

    [Header("References")]
    [SerializeField] private GameObject player;
    [SerializeField] private GameObject attackHitbox;
    [SerializeField] private float targetDistanceToPlayer;

    private Vector3 playerXZ;
    private bool attacked = false;

    //Temporary
    private Color originalColor;
    private Renderer enemyRenderer;


    private void Awake()
    {
        playerXZ = new Vector3(player.transform.position.x, 1.25f, player.transform.position.z);

        enemyRenderer = GetComponent<Renderer>();
        originalColor = enemyRenderer.material.color;
    }

    void Update()
    {
        MoveToTarget();
    }

    void MoveToTarget()
    {
        float distanceToPlayer = Vector3.Distance(transform.position, playerXZ);

        if (distanceToPlayer > targetDistanceToPlayer)
        {
            MoveTowardsPlayer(2.5f);
        }
        if (distanceToPlayer <= targetDistanceToPlayer && !attacked)
        {
            StartCoroutine(Attack());
            attacked = true;
        }
    }

    private void MoveTowardsPlayer(float enemySpeed)
    {
        transform.position = Vector3.MoveTowards(transform.position, playerXZ, enemySpeed * Time.deltaTime);
    }

    private IEnumerator Attack()
    {
        yield return new WaitForSeconds(timeBetweenAttacks);

        Debug.Log("Attack");

        // Temporary
        StartCoroutine(ChangeColorWhenAttacking());

        StartCoroutine(AttackHitboxOnThenOff());

        attacked = false;
    }

    private IEnumerator AttackHitboxOnThenOff()
    {
        attackHitbox.SetActive(true);

        yield return new WaitForSeconds(0.2f);

        attackHitbox.SetActive(false);
    }

    // Temporary
    private IEnumerator ChangeColorWhenAttacking()
    {
        enemyRenderer.material.color = Color.yellow;

        yield return new WaitForSeconds(0.2f);

        enemyRenderer.material.color = originalColor;
    }
}
