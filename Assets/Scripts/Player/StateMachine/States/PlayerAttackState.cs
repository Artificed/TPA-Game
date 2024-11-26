using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = System.Random;

public class PlayerAttackState : PlayerBaseState
{
    private Random _random = new Random();
    private int _randomAttack;
    public PlayerAttackState(PlayerStateMachine context, PlayerStateFactory factory) : base(context, factory)
    {
    }

    public override void EnterState()
    {
        _randomAttack = _random.Next(1, 4);

        switch (_randomAttack)
        {
            case 1:
                Context.Animator.SetBool(Context.IsAttackingHash1, true);
                break;
            case 2:
                Context.Animator.SetBool(Context.IsAttackingHash2, true);
                break;
            case 3:
                Context.Animator.SetBool(Context.IsAttackingHash3, true);
                break;
        }
    }

    public override void UpdateState()
    {
        CheckSwitchStates();
    }

    public override void ExitState()
    {
        switch (_randomAttack)
        {
            case 1:
                Context.Animator.SetBool(Context.IsAttackingHash1, false);
                break;
            case 2:
                Context.Animator.SetBool(Context.IsAttackingHash2, false);
                break;
            case 3:
                Context.Animator.SetBool(Context.IsAttackingHash3, false);
                break;
        }
    }

    public override void CheckSwitchStates()
    {
        AnimatorStateInfo stateInfo = Context.Animator.GetCurrentAnimatorStateInfo(0);
        // Debug.Log(stateInfo.normalizedTime);
        if (stateInfo.IsName("Attack1") || stateInfo.IsName("Attack2") || stateInfo.IsName("Attack3"))    
        {
            if (stateInfo.normalizedTime >= 1.0f)  
            {
                // Debug.Log("Player Attack Animation Done");
                Context.PlayerTurnEventChannel.RaiseEvent();
                SwitchState(Factory.CreateBattleIdle());
            }
        }
    }
}
