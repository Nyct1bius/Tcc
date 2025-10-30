using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.VFX;

public class TurretController : MonoBehaviour
{
    public enum TurretType
    {
        KillPlayer,
        DamagePlayer
    }

    [SerializeField] TurretType type;
    [SerializeField] VisualEffect[] lasersEffec;
    [SerializeField] LayerMask LayerMask;
    private Coroutine lookForCollision;
    private void Start()
    {
    }

    private void OnTriggerExit(Collider other)
    {
        StopCoroutine(lookForCollision);

        for (int i = 0; i < lasersEffec.Length; i++)
        {
            lasersEffec[i].SetFloat("Distance", 5f);

        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.activeSelf)
        {
            lookForCollision = StartCoroutine(LoopCollision());
        }
    }


    IEnumerator LoopCollision()
    {
        while (true)
        {
            for (int i = 0; i < lasersEffec.Length; i++)
            {
                var vfx = lasersEffec[i];
                var origin = vfx.transform.position;
                var direction = vfx.transform.forward;
                Debug.DrawRay(origin, direction * 10f, Color.green, 0.03f);
                RaycastHit hit;

                if (Physics.Raycast(origin, direction, out hit, 100f, LayerMask))
                {
                    float distance = hit.distance;
                    distance = Mathf.Clamp(distance, 0, 15);
                    vfx.SetFloat("Distance", distance/3);

                    var playerHealth = GameManager.instance.PlayerInstance.GetComponent<PlayerHealthManager>();
                    if (GameManager.instance.PlayerInstance == hit.transform.gameObject)
                    {
                        if(type == TurretType.KillPlayer && GameManager.instance.canRespawnPlayer)
                        {
                            if (!playerHealth.shield.CanBlock(origin))
                            {
                                playerHealth.Damage(10, transform.position);
                                GameManager.instance.RespawnPlayer(0.5f);
                            }
                             
                        }
                        else if(type == TurretType.DamagePlayer)
                        {
                            playerHealth.Damage(10, transform.position);
                        }
                    }
                }
                else
                {
                    vfx.SetFloat("Distance", 5f);
                }
                yield return null;
            }

        }
    }


}
