using System.Collections;
using UnityEngine;

public class TestEnemyHealth : MonoBehaviour, IHealth
{
    [SerializeField] private float maxHealth;
    [SerializeField] private float immortalityTime;
    [SerializeField] private AnimationClip IdleAnim;
    [SerializeField] private AnimationClip hitAnimation;
    [SerializeField] private AnimationClip deathAnimetion;
    [SerializeField] private Material _whiteMat;
    [SerializeField] private SkinnedMeshRenderer _dummyVisual;
    private Material _deafaultMat;
    public bool isImmortal;
    private float currenthealth;
    private Animator animator;

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
        if (!isImmortal)
        {
            animator.CrossFade(IdleAnim.name, 0.2f);
        }
    }

    public void Damage(float damage, Vector3 DamageSourcePos)
    {
        if (!isImmortal)
        {
            //currenthealth -= damage;
            isImmortal = true;
            StartCoroutine(Immortal());
            Vector3 dirToKnockBack = (DamageSourcePos - transform.position).normalized;
            dirToKnockBack.y = 0;
            transform.position += dirToKnockBack * -1f;
            animator.CrossFade(hitAnimation.name,0.2f);
            FacePlayer(DamageSourcePos);
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
        currenthealth = maxHealth;
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
