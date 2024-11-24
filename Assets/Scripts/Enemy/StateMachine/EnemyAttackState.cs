using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttackState : EnemyBaseState
{
    public EnemyAttackState(EnemyStateMachine context, EnemyStateFactory factory) : base(context, factory)
    {
    }

    public override void EnterState()
    {
        Context.Animator.SetBool(Context.IsAttackingHash, true);
        Debug.Log("Enemy Enter Attacking");
    }

    public override void UpdateState()
    {
        CheckSwitchStates();
    }

    public override void ExitState()
    {
        Context.Animator.SetBool(Context.IsAttackingHash, false);
        Debug.Log("Enemy Exit Attacking");
    }

    public override void CheckSwitchStates()
    {
        // if (TurnManager.Instance.CurrentTurn == TurnType.PlayerTurn)
        // {
        //     SwitchState(Factory.CreateReadyAttack());
        // }
        //
        AnimatorStateInfo stateInfo = Context.Animator.GetCurrentAnimatorStateInfo(0);
        Debug.Log(stateInfo.normalizedTime);
        if (stateInfo.IsName("Attacking"))  
        {
            if (stateInfo.normalizedTime >= 1.0f)  
            {
                Debug.Log("Attack Animation Done");
                Context.EnemyActionCompleteEventChannel.RaiseEvent();
                SwitchState(Factory.CreateReadyAttack());
            }
        }
    }
}
