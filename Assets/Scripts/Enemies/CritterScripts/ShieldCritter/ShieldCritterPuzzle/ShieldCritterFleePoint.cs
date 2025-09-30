using System.Collections;
using UnityEngine;

public class ShieldCritterFleePoint : MonoBehaviour
{
    public GameObject Player;
    public ShieldCritter Critter;
    public ShieldCritterFleePointActivator Activator;

    [SerializeField] private bool isM1, isM2, isM3, isL1, isL2, isR1, isR2;

    [SerializeField] private Transform sideChecker;

    private bool chaseStarted = false, playerOnMyRight, isMyTurn;

    void Start()
    {
        Player = GameManager.instance.PlayerInstance;
        if (Player == null)
            StartCoroutine(WaitToFindPlayer());
    }

    private void FixedUpdate()
    {
        CheckPlayerSide();
        CheckCritterDistance();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (GameManager.instance.PlayerInstance == other.gameObject && isMyTurn)
        {
            if (isM1 && !chaseStarted)
            {
                Critter.SetL1OrL2();
                chaseStarted = true;
            }
            if (isM1 && chaseStarted)
            {
                if (playerOnMyRight)
                    Critter.SetL1();
                else
                    Critter.SetR1();
            }
            if (isL1)
            {
                if (playerOnMyRight)
                    Critter.SetL2();
                else
                    Critter.SetM1();
            }
            if (isL2)
            {
                if (playerOnMyRight)
                    Critter.SetL1();
                else
                    Critter.SetM2();
            }
            if (isR1)
            {
                if (playerOnMyRight)
                    Critter.SetM1();
                else
                    Critter.SetR2();

                Debug.Log("R1");
            }
            if (isR2)
            {
                if (playerOnMyRight)
                    Critter.SetM2();
                else
                    Critter.SetR1();
            }
            if (isM2 && !Critter.IsCornered)
            {
                if (playerOnMyRight)
                    Critter.SetL2();
                else
                    Critter.SetR2();
            }
            if (isM2 && Critter.IsCornered)
            {
                Critter.SetM3();;
            }

            Critter.IsWaiting = false;
            isMyTurn = false;
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
        if (Player.transform.position.x < sideChecker.position.x)
            playerOnMyRight = true;
        else
            playerOnMyRight = false;
    }

    private void CheckCritterDistance()
    {
        if (Vector3.Distance(transform.position, Critter.gameObject.transform.position) <= 10)
            isMyTurn = true;
        else
            isMyTurn = false;
    }
}
