using UnityEngine;
using UnityEngine.UI;

public class PlayerHealthManager : MonoBehaviour,IHealth
{
    [SerializeField] private PlayerUIManager _playerUIManager;
    [SerializeField] private Shield _shield;  
    [SerializeField] private PlayerStatsSO _stats;
    private float _currentHealth;
    [SerializeField] private float _immortalityTime;

    private bool _isInvulnerable;
    private bool _isDamaged;
    private float _damageToReceive;

    [Header("KnockBack")]

    [SerializeField] private float _horizontalKnockBackForce;
    [SerializeField] private float _verticalKnockBackForce;
    private Vector3 _knockBackDirection;
    #region Getters and Setters

    public PlayerUIManager PlayerUIManager {  get { return _playerUIManager; } }
    public float CurrentHealth { get { return _currentHealth; } set { _currentHealth = value; } }
    public float ImmortalityTime { get { return _immortalityTime; } }
    public float DamageToReceive {  get { return _damageToReceive; } }

    public bool IsInvulnerable { get { return _isInvulnerable; } set { _isInvulnerable = value; } }
    public bool IsDamaged { get {return _isDamaged; } set { _isDamaged = value; } }
    
   public float HorizontalKnockBackForce { get { return _horizontalKnockBackForce; } }
   public float VerticalKnockBackForce { get { return _verticalKnockBackForce; } }
   public Vector3 KnockBackDirection { get {  return _knockBackDirection; } }


    #endregion

    private void Awake()
    {
        _currentHealth = _stats.maxHealth;
        _playerUIManager.AtualizePlayerHealthUI(_currentHealth);
    }
    public void Death()
    {
        GameManager.instance.RespawnPlayer();
    }

    public void HealHealth(float healing)
    {
       _currentHealth += healing;
        if(_currentHealth > _stats.maxHealth)
        {
            _currentHealth = _stats.maxHealth;
        } 
        _playerUIManager.AtualizePlayerHealthUI(_currentHealth);
       
    }

    public void Damage(float damage, Vector3 DamageSourcePos)
    {
   
        if (_shield.CanBlock(DamageSourcePos)) return;

        _knockBackDirection = (transform.position - DamageSourcePos).normalized;
        if (!_isInvulnerable)
        {
            _isDamaged = true;
            _damageToReceive = damage;
        }
        Debug.Log("Took damage");
    }
}
