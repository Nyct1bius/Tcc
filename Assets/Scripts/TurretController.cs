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
                    

                    if(GameManager.instance.PlayerInstance == hit.transform.gameObject)
                    {
                        if(type == TurretType.KillPlayer)
                        {
                            GameManager.instance.RespawnPlayer();
                        }
                        else if(type == TurretType.DamagePlayer)
                        {
                            GameManager.instance.PlayerInstance.GetComponent<PlayerHealthManager>().Damage(10, origin);
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
