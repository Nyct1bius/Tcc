using UnityEngine;


[CreateAssetMenu(menuName = "ScriptableObject / PlayerStats")]
public class PlayerStatsSO : ScriptableObject
{
    public float maxHealth;
    public float currentHealth;
    public Transform respawnPoint;
    public bool hasSword;
    public bool hasShield;
}
