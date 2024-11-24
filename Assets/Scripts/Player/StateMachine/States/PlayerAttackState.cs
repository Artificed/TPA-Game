using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttackState : PlayerBaseState
{
    public PlayerAttackState(PlayerStateMachine context, PlayerStateFactory factory) : base(context, factory)
    {
    }

    public override void EnterState()
    {
        Context.Animator.SetBool(Context.IsAttackingHash, true);
        Debug.Log("Player Entering Attack State!");
    }

    public override void UpdateState()
    {
        CheckSwitchStates();
    }

    public override void ExitState()
    {
        Context.Animator.SetBool(Context.IsAttackingHash, false);
        Debug.Log("Player Exiting Attack State!");
    }

    public override void CheckSwitchStates()
    {
        AnimatorStateInfo stateInfo = Context.Animator.GetCurrentAnimatorStateInfo(0);
        // Debug.Log(stateInfo.normalizedTime);
        if (stateInfo.IsName("Attacking"))  
        {
            if (stateInfo.normalizedTime >= 1.0f)  
            {
                Debug.Log("Player Attack Animation Done");
                Context.PlayerTurnEventChannel.RaiseEvent();
                SwitchState(Factory.CreateBattle());
            }
        }
    }
}
