using UnityEngine;

public class BossAttackProxy : MonoBehaviour
{
    public Boss Boss;

    public void BounceAttack()
    {
        GameObject player = Boss.Player;
        Vector3 playerPos = new Vector3(player.transform.position.x, Boss.transform.position.y, player.transform.position.z);

        if (Vector3.Distance(Boss.transform.position, playerPos) < Boss.Stats.BounceAttackRange)
        {
            Boss.Player.GetComponent<PlayerHealthManager>().Damage(25, transform.position);
        }
    }
    public void EnableBossCollider()
    {
        Boss.EnableCollider();
    }
    public void DisableBossCollider()
    {
        Boss.DisableCollider();
    }

    public void EnableBossDiveTrigger()
    {
        Boss.EnableDiveHitbox();
        Boss.Stats.AttackMovementSpeed = 40f;
    }
    public void DisableBossDiveTigger()
    {
        Boss.DisableDiveHitbox();
    }

    public void SetBossLaserOn()
    {
        Boss.SetLaserOn();
        Boss.Stats.AttackMovementSpeed = 40f;
    }
    public void SetBossLaserOff()
    {
        Boss.SetLaserOff();
    }
}
