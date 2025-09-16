using System.Collections;
using UnityEngine;

public class ShieldCritterFleePoint : MonoBehaviour
{
    public GameObject Player;
    public ShieldCritter critter;

    [SerializeField] private bool isM1, isM2, isL1, isL2, isR1, isR2;

    private bool chaseStarted = false, playerOnMyLeft;

    void Start()
    {
        Player = GameManager.instance.PlayerInstance;
        if (Player == null)
            StartCoroutine(WaitToFindPlayer());
    }

    private void FixedUpdate()
    {
        CheckPlayerSide();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other == Player)
        {
            if (isM1 && !chaseStarted)
            {
                critter.SetL1OrL2();
                chaseStarted = true;
            }
            if (isM1 && chaseStarted)
            {
                if (playerOnMyLeft)
                    critter.SetR1();
                else
                    critter.SetL1();
            }
            if (isL1)
            {
                if (playerOnMyLeft)
                    critter.SetL2();
                else
                    critter.SetM1();
            }
        }
    }

    IEnumerator WaitToFindPlayer()
    {
        yield return new WaitForSeconds(0.25f);
        if (GameManager.instance.PlayerInstance != null)
        {
            while (Player == null)
            {
                Player = GameManager.instance.PlayerInstance;
                yield return null;
            }
        }
        else
        {
            Debug.LogWarning("GameManager Instance not found");
        }
    }

    private void CheckPlayerSide()
    {
        if (Player.transform.position.x > transform.position.x)
            playerOnMyLeft = true;
        else
            playerOnMyLeft = false;
    }
}
