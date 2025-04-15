using UnityEngine;

public class AnimationHandler : MonoBehaviour
{
   
    public void OnAnimationFinished()
    {
        PlayerEvents.OnAttackFinished();
    }

}
