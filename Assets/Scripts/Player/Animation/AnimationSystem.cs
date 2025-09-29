using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.Playables;
using System.Collections.Generic;
using MEC;
using System;


public class AnimationSystem
{
    PlayableGraph _playableGraph;
    readonly private AnimationMixerPlayable _topLevelMixer;
    readonly private AnimatorControllerPlayable _movementController;

    AnimationClipPlayable _oneShotPlayable;
    public AnimationClipPlayable _attackShotPlayable;

    CoroutineHandle _blendInHandle;
    CoroutineHandle _blendOutHandle;


    public AnimationSystem(Animator baseAnimator)
    {
        _playableGraph = PlayableGraph.Create("AnimationSystem");
        AnimationPlayableOutput animationPlayableOutput = AnimationPlayableOutput.Create(_playableGraph, "Animation", baseAnimator);

        _topLevelMixer = AnimationMixerPlayable.Create(_playableGraph, 2);

        animationPlayableOutput.SetSourcePlayable(_topLevelMixer);

        _movementController = AnimatorControllerPlayable.Create(_playableGraph, baseAnimator.runtimeAnimatorController);

        _topLevelMixer.ConnectInput(0, _movementController, 0);
        _playableGraph.GetRootPlayable(0).SetInputWeight(0, 1f);
        baseAnimator.runtimeAnimatorController = null;
        _playableGraph.Play();

    }

    public void PlayOneShot(AnimationClip oneShotClip)
    {
        if (_oneShotPlayable.IsValid() && _oneShotPlayable.GetAnimationClip() == oneShotClip) return;

        InterrupOneShot();
        _oneShotPlayable = AnimationClipPlayable.Create(_playableGraph, oneShotClip);
        _topLevelMixer.ConnectInput(1, _oneShotPlayable, 0);
        _topLevelMixer.SetInputWeight(1, 1f);

        float blenDuration = .2f;

        BlendIn(blenDuration);
        BlendOut(blenDuration, oneShotClip.length - blenDuration,DisconnectOneShot);

    }

    private void BlendIn(float blenDuration)
    {
        _blendInHandle = Timing.RunCoroutine(Blend(blenDuration, blendTime => {
            float weight = Mathf.Lerp(1f, 0f, blendTime);
            _topLevelMixer.SetInputWeight(0, weight);
            _topLevelMixer.SetInputWeight(1, 1f - weight);
        }));
    }

    void BlendOut(float duration, float delay, Action finishedCallback)
    {
        _blendOutHandle = Timing.RunCoroutine(Blend(duration, blendTime => {
            float weight = Mathf.Lerp(0f, 1f, blendTime);
            _topLevelMixer.SetInputWeight(0, weight);
            _topLevelMixer.SetInputWeight(1, 1f - weight);
        }, delay, finishedCallback));
    }
    

    IEnumerator<float> Blend(float duration, Action<float> blendCallback, float delay = 0f, Action finishedCallback = null)
    {
        if (delay > 0f)
        {
            yield return Timing.WaitForSeconds(delay);
        }

        float blendTime = 0f;
        while(blendTime < 1f)
        {
             blendTime += Time.deltaTime/ duration;
            blendCallback(blendTime);
            yield return blendTime;

        }

        blendCallback(1f);

        finishedCallback?.Invoke();
    }

    private void InterrupOneShot()
    {
        Timing.KillCoroutines(_blendInHandle);
        Timing.KillCoroutines(_blendOutHandle);

        _topLevelMixer.SetInputWeight(0,1f);
        _topLevelMixer.SetInputWeight(1, 0);

        if(_oneShotPlayable.IsValid())
        {
            DisconnectOneShot();
        }
    }

    private void DisconnectOneShot()
    {
        _topLevelMixer.DisconnectInput(1);
        _playableGraph.DestroyPlayable(_oneShotPlayable);
    }

    public void PlayAttack(AnimationClip oneShotClip)
    {
        if (_attackShotPlayable.IsValid() && _attackShotPlayable.GetAnimationClip() == oneShotClip) return;

        InterrupAttack();
        _attackShotPlayable = AnimationClipPlayable.Create(_playableGraph, oneShotClip);
        _topLevelMixer.ConnectInput(1, _attackShotPlayable, 0);
        _topLevelMixer.SetInputWeight(1, 1f);

        float blenDuration = .2f;

        BlendIn(blenDuration);
        BlendOut(blenDuration, oneShotClip.length - blenDuration,DisconnectAttack);

    }

    private void InterrupAttack()
    {
        Timing.KillCoroutines(_blendInHandle);
        Timing.KillCoroutines(_blendOutHandle);

        _topLevelMixer.SetInputWeight(0, 1f);
        _topLevelMixer.SetInputWeight(1, 0);

        if (_oneShotPlayable.IsValid())
        {
            DisconnectAttack();
        }
    }
    private void DisconnectAttack()
    {
        PlayerEvents.OnAttackFinished();
        _topLevelMixer.DisconnectInput(1);
        _playableGraph.DestroyPlayable(_attackShotPlayable);
    }

    #region Movement
    public void UpdateMovement(float speed)
    {
        _movementController.SetFloat("Speed",speed);
    }
    public void UpdateJump(float speed)
    {
        _movementController.SetFloat("YSpeed", speed);
    }
    public void UpdateDash(bool active)
    {
        _movementController.SetBool("IsDashing", active);
    }

    public void Jump()
    {
        _movementController.SetTrigger("OnAir");
    }
    public void UpdateGrounded(bool active)
    {
         _movementController.SetBool("IsGrounded", active);
    }

    public void Hited()
    {
        _movementController.SetTrigger("Hited");
    }
    public void Death()
    {
        _movementController.SetTrigger("Death");
    }

    #endregion

    #region Shield
    public void UpdateShieldMovement(Vector2 input)
    {
        _movementController.SetFloat("inputX", input.x);
        _movementController.SetFloat("inputY", input.y);
    }
    public void ShieldUp()
    {
        _movementController.SetTrigger("ShieldUp");
        _movementController.SetBool("IsBlocking", true);
    }
    public void ShieldDown()
    {
        _movementController.SetBool("IsBlocking", false);
    }
    #endregion
    public void Destroy()
    {
        if(_playableGraph.IsValid())
        {
            _playableGraph.Destroy();
        }
    }

}
