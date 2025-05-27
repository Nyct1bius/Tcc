using System.Collections;
using UnityEngine;

public class TestEnemyHealth : MonoBehaviour, IHealth
{
    [SerializeField] private float maxHealth;
    [SerializeField] private float immortalityTime;
    [SerializeField] private AnimationClip IdleAnim;
    [SerializeField] private AnimationClip hitAnimation;
    [SerializeField] private AnimationClip deathAnimation;
    [SerializeField] private AnimationClip reviveAnimation;
    [SerializeField] private Material _whiteMat;
    [SerializeField] private SkinnedMeshRenderer _dummyVisual;
    private Material _deafaultMat;
    public bool isImmortal;
    private float currenthealth;
    private Animator animator;
    private bool _isEnemyDead;

    private void Start()
    {
        currenthealth = maxHealth;
        animator = GetComponent<Animator>();
        _deafaultMat = _dummyVisual.material;
    }

    public void HealHealth(float healing)
    {
        throw new System.NotImplementedException();
    }

    private void Update()
    {
        if (!isImmortal && !_isEnemyDead)
        {
            animator.CrossFade(IdleAnim.name, 0.2f);
        }
    }

    public void Damage(float damage, Vector3 DamageSourcePos)
    {
        if (!isImmortal && currenthealth > 0)
        {
            currenthealth -= damage;
            isImmortal = true;
            StartCoroutine(Immortal());
            Vector3 dirToKnockBack = (DamageSourcePos - transform.position).normalized;
            dirToKnockBack.y = 0;
            transform.position += dirToKnockBack * -1f;
            animator.CrossFade(hitAnimation.name,0.2f);
            FacePlayer(DamageSourcePos);
        }
        else
        {
            FacePlayer(DamageSourcePos);
            Death();
        }
        
    }

    private IEnumerator Immortal()
    {
        yield return new WaitForSeconds(immortalityTime);
        isImmortal = false;
    }

    private void FacePlayer(Vector3 DamageSourcePos)
    {
        Vector3 _lookDirection = (DamageSourcePos - transform.position).normalized;
        Quaternion _lookRotation = Quaternion.LookRotation(new Vector3(_lookDirection.x, 0, _lookDirection.z));
        transform.rotation = Quaternion.Slerp(transform.rotation, _lookRotation, 1);
    }

    public void Death()
    {
        _isEnemyDead = true;
        animator.CrossFade(deathAnimation.name, 0.2f);
        StartCoroutine(ReviveEnemy());
    }
    IEnumerator ReviveEnemy()
    {
        CapsuleCollider collider = GetComponent<CapsuleCollider>();
        collider.enabled = false;
        yield return new WaitForSeconds(3f);
        animator.CrossFade(reviveAnimation.name, 0.2f);
        yield return new WaitForSeconds(reviveAnimation.length);
        collider.enabled = true;
        currenthealth = maxHealth;
        _isEnemyDead  = false;
    }
    public void ChangeMaterialToWhite()
    {
        _dummyVisual.material = _whiteMat;
    }

    public void ChangeMaterialToOriginal()
    {
        _dummyVisual.material = _deafaultMat;
    }
}
