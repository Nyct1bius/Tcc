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
        PlayerEvents.OnAttackVfx();
    }
}
