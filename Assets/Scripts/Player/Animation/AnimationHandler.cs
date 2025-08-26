using FMODUnity;
using UnityEngine;

public class AnimationHandler : MonoBehaviour
{
    public void OnAtackAnimationFinished()
    {
        PlayerEvents.OnAttackFinished();
    }

    public void OnDashAnimationFinished()
    {
        PlayerEvents.OnDashFinished();
    }

    public void OnSpawnAttackVFX()
    {
        //PlayerEvents.OnAttackVfx();
    }

    public void OnStartAttackDetection()
    {
        //PlayerEvents.OnStartAttackDetection();
    }

    #region SFX
    public void OnPlayAttackSFX()
    {
        PlayerEvents.OnAttackSFX();
    }
    public void OnPLayWalkSFX()
    {
        PlayerEvents.OnWalkSFX();
    }

    public void OnPLayChickenSFX()
    {
        PlayerEvents.OnChickenSFX();
    }
    public void OnStopChickenSFX()
    {
        PlayerEvents.OnStopChickenSFX();
    }
    #endregion
}
