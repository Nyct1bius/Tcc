using System.Collections;
using UnityEngine;

public class ShieldCritterFleePoint : MonoBehaviour
{
    public GameObject Player;
    public ShieldCritter critter;

    [SerializeField] private bool isM1, isM2, isL1, isL2, isR1, isR2;

    private bool chaseStarted = false, playerOnMyRight;

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
        if (GameManager.instance.PlayerInstance == other.gameObject)
        {
            if (isM1 && !chaseStarted)
            {
                critter.SetL1OrL2();
                chaseStarted = true;
            }
            if (isM1 && chaseStarted)
            {
                if (playerOnMyRight)
                    critter.SetL1();
                else
                    critter.SetR1();
            }
            if (isL1)
            {
                if (playerOnMyRight)
                    critter.SetL2();
                else
                    critter.SetM1();
            }
            if (isL2)
            {
                if (playerOnMyRight)
                    critter.SetL1();
                else
                    critter.SetM2();
            }
            if (isR1)
            {
                if (playerOnMyRight)
                    critter.SetM1();
                else
                    critter.SetR2();
            }
            if (isR2)
            {
                if (playerOnMyRight)
                    critter.SetM2();
                else
                    critter.SetR1();
            }
            if (isM2)
            {
                if (playerOnMyRight)
                    critter.SetL2();
                else
                    critter.SetR2();
            }

            critter.IsWaiting = false;
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
        if (Player.transform.position.x < transform.position.x)
            playerOnMyRight = true;
        else
            playerOnMyRight = false;
    }
}
