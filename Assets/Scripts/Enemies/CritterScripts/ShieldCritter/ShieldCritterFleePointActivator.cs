using System.Collections;
using UnityEngine;

public class ShieldCritterFleePointActivator : MonoBehaviour
{
    [SerializeField] private GameObject[] fleePoints;

    private void OnTriggerEnter(Collider other)
    {
        if (GameManager.instance.PlayerInstance == other.gameObject)
        {
            foreach (GameObject point in fleePoints)
            {
                point.SetActive(true);
            }
        }
    }

    public void EndChase()
    {
        StartCoroutine(DisableChaseAssets());
    }

    public IEnumerator DisableChaseAssets()
    {
        yield return new WaitForSeconds(1);
        
        foreach (GameObject point in fleePoints)
        {
            point.SetActive(false);
        }

        yield return new WaitForSeconds(0.5f);

        gameObject.SetActive(false);
    }
}
