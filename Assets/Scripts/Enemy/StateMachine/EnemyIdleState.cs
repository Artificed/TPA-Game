using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyIdleState : EnemyBaseState
{
    public EnemyIdleState(EnemyStateMachine context, EnemyStateFactory factory) : base(context, factory)
    {
    }

    public override void EnterState()
    {
        Debug.Log("Enemy in Idle State");
        Context.Animator.SetBool(Context.IsMovingHash, false);
    }

    public override void UpdateState()
    {
        if (Context.GetDistanceFromPlayer() <= Context.AlertRange)
        {
            SwitchState(Factory.CreateAlert());
        }
    }

    public override void ExitState()
    {
    }

    public override void CheckSwitchStates()
    {
        
    }
}
