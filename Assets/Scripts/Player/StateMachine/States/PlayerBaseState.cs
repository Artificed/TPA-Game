using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PlayerBaseState
{
    protected PlayerStateMachine Context;
    protected PlayerStateFactory Factory;

    protected PlayerBaseState(PlayerStateMachine context, PlayerStateFactory factory)
    {
        Context = context;
        Factory = factory;
    }
    
    public abstract void EnterState();
    public abstract void UpdateState();
    public abstract void ExitState();
    public abstract void CheckSwitchStates();
    public void SwitchState(PlayerBaseState nextState)
    {
        Context.CurrentState.ExitState();
        Context.CurrentState = nextState;
        Context.CurrentState.EnterState();
    }

    public void CheckDeathState()
    {
        if (Player.Instance.Health <= 0)
        {
            SwitchState(Factory.CreateDeath());
        }
    }
}