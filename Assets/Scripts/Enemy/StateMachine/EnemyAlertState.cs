using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAlertState : EnemyBaseState
{
    public EnemyAlertState(EnemyStateMachine context, EnemyStateFactory factory) : base(context, factory)
    {
        
    }

    public override void EnterState()
    {
        Debug.Log("Enemy in Alert State");
    }

    public override void UpdateState()
    {
        if (Context.GetDistanceFromPlayer() > Context.AlertRange)
        {
            SwitchState(Factory.CreateIdle());
        }
    }

    public override void ExitState()
    {
    }

    public override void CheckSwitchStates()
    {
    }
}
