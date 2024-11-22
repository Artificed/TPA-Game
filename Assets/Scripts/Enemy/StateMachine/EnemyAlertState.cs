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
        Context.EnemyAlertEventChannel.RaiseEvent();
    }

    public override void UpdateState()
    {
        
    }

    public override void ExitState()
    {
    }

    public override void CheckSwitchStates()
    {
    }
}
