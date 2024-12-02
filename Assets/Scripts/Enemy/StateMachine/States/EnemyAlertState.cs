using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAlertState : EnemyBaseState
{
    private bool _commandQueued = false;
    public EnemyAlertState(EnemyStateMachine context, EnemyStateFactory factory) : base(context, factory)
    {
        
    }

    public override void EnterState()
    {
        // Debug.Log("Enemy in Alert State");
        Context.QuestionText.SetActive(true);
        TurnManager.Instance.AddEnemy(Context.Enemy);
        Context.Animator.SetBool(Context.IsAlertHash, true);

        _commandQueued = false;
    }

    public override void UpdateState()
    {
        if (Context.GetDistanceFromPlayer() > Context.AlertRange)
        {
            Debug.Log("Enemy no longer alerted!");
            SwitchState(Factory.CreateIdle());
            TurnManager.Instance.RemoveEnemy(Context.Enemy);
            return;
        } 
        
        if(_commandQueued) return;
        
        EnemyCheckLOSCommand checkLosCommand = new EnemyCheckLOSCommand(Context);
        TurnManager.Instance.AddQueue(checkLosCommand);
        _commandQueued = true;
    }

    public override void ExitState()
    {
        _commandQueued = false;
        Context.QuestionText.SetActive(false);
    }

    public override void CheckSwitchStates()
    {
        
    }

    public bool CommandQueued
    {
        get => _commandQueued;
        set => _commandQueued = value;
    }
}
