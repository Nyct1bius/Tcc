using UnityEngine;

public class EnemyProjectileGenerator : MonoBehaviour
{
    private ObjectPooler objectPooler; // Reference to the ObjectPooling instance


    #region Singleton
    // Singleton pattern to ensure only one instance of ObjectPooling exists
    public static EnemyProjectileGenerator Instance;

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

    public void SpawnProjectile(Transform spawnPosition)
    {
        EnemyProjectile enemyProjectile = objectPooler.SpawnFromPool("EnemyProjectile", spawnPosition.position, 
            spawnPosition.rotation).GetComponent<EnemyProjectile>();   

        enemyProjectile.OnObjectSpawn();
    }


}
