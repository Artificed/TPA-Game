using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EnemyBaseState
{
    protected EnemyStateMachine Context;
    protected EnemyStateFactory Factory;

    public EnemyBaseState(EnemyStateMachine context, EnemyStateFactory factory)
    {
        Context = context;
        Factory = factory;
    }

    public abstract void EnterState();
    public abstract void UpdateState();
    public abstract void ExitState();
    public abstract void CheckSwitchStates();

    public void SwitchState(EnemyBaseState nextState)
    {
        Context.CurrentState.ExitState();
        Context.CurrentState = nextState;
        Context.CurrentState.EnterState();
    }
}
