using UnityEngine;

public class CombatHealGenerator : MonoBehaviour
{
    private ObjectPooler objectPooler; // Reference to the ObjectPooling instance
    #region Singleton
    // Singleton pattern to ensure only one instance of ObjectPooling exists
    public static CombatHealGenerator Instance;

    private void Awake()
    {
        if (Instance == null)
            Instance = this; // Set the static instance to this instance
        else
            Destroy(gameObject);
    }
    #endregion

    void Start()
    {
        objectPooler = ObjectPooler.Instance; // Get the instance of ObjectPooling
    }

    public void SpawnHeal(Transform spawnPosition)
    {
        HealingBattery healingBattery = objectPooler.SpawnFromPool("HealingBattery", spawnPosition.position,
            spawnPosition.rotation).GetComponent<HealingBattery>();

        healingBattery.OnObjectSpawn();
    }


}
