using UnityEngine;


[CreateAssetMenu(menuName = "ScriptableObject / PlayerStats")]
public class PlayerStatsSO : ScriptableObject
{
    public float maxHealth;
    public Transform respawnPoint;
    public bool hasSword;
    public bool hasShield;

    private void OnDisable()
    {
        hasSword = false;
        hasShield = false;
    }
}
