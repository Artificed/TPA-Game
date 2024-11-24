using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAlertState : EnemyBaseState
{
    private bool commandQueued = false;
    public EnemyAlertState(EnemyStateMachine context, EnemyStateFactory factory) : base(context, factory)
    {
        
    }

    public override void EnterState()
    {
        // Debug.Log("Enemy in Alert State");
        TurnManager.Instance.AddEnemy(Context);
        Context.Animator.SetBool(Context.IsAlertHash, true);

        commandQueued = false;
    }

    public override void UpdateState()
    {
        CheckSwitchStates();
        
        if(commandQueued) return;

        Debug.Log("Queueing LOS Command");
        EnemyCheckLOSCommand checkLosCommand = new EnemyCheckLOSCommand(Context);
        TurnManager.Instance.AddQueue(checkLosCommand);
        commandQueued = true;
    }

    public override void ExitState()
    {
        commandQueued = false;
    }

    public override void CheckSwitchStates()
    {
        if (Context.GetDistanceFromPlayer() > Context.AlertRange)
        {
            Debug.Log("Enemy no longer alerted!");
            SwitchState(Factory.CreateIdle());
            TurnManager.Instance.RemoveEnemy(Context);
        } 
    }

    public bool CommandQueued
    {
        get => commandQueued;
        set => commandQueued = value;
    }
}
