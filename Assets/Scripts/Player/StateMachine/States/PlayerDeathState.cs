using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDeathState : PlayerBaseState
{
    public PlayerDeathState(PlayerStateMachine context, PlayerStateFactory factory) : base(context, factory)
    {
    }

    public override void EnterState()
    {
        Debug.Log("Enemy Died!");
        SoundFXManager.Instance.PlayDeathClip(Context.transform);
        Context.Animator.SetTrigger(Context.IsDeadHash);
    }

    public override void UpdateState()
    {
        CheckSwitchStates();
    }

    public override void ExitState()
    {
        
    }

    public override void CheckSwitchStates()
    {
        AnimatorStateInfo stateInfo = Context.Animator.GetCurrentAnimatorStateInfo(0);

        if (stateInfo.IsName("Death") && stateInfo.normalizedTime >= 1.0f)
        {
            Object.Destroy(Context.gameObject);
            Context.PlayerDeathEventChannel.RaiseEvent();
        }
    }
}
