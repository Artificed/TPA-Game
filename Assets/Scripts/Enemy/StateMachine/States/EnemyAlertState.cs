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
        TurnManager.Instance.AddEnemy(Context);
        Context.Animator.SetBool(Context.IsAlertHash, true);

        _commandQueued = false;
    }

    public override void UpdateState()
    {
        CheckSwitchStates();
        
        if(_commandQueued) return;
        
        CheckInstantHit();

        EnemyCheckLOSCommand checkLosCommand = new EnemyCheckLOSCommand(Context);
        TurnManager.Instance.AddQueue(checkLosCommand);
        _commandQueued = true;
    }

    private void CheckInstantHit()
    {
        if (TurnManager.Instance.CurrentTurn == TurnType.EnemyTurn)
        {
            Vector2Int enemyCords = new Vector2Int(
                Mathf.RoundToInt(Context.Unit.position.x / Context.GridManager.UnityGridSize),
                Mathf.RoundToInt(Context.Unit.position.z / Context.GridManager.UnityGridSize)
            );
        
            Vector2Int playerCords = new Vector2Int(
                Mathf.RoundToInt(PlayerStateMachine.Instance.Unit.position.x / Context.GridManager.UnityGridSize),
                Mathf.RoundToInt(PlayerStateMachine.Instance.Unit.position.z / Context.GridManager.UnityGridSize)
            );

            if (Mathf.Abs(enemyCords.x - playerCords.x) + Mathf.Abs(enemyCords.y - playerCords.y) <= 1)
            {
                EnemyAttackCommand enemyAttackCommand = new EnemyAttackCommand(Context);
                TurnManager.Instance.AddQueue(enemyAttackCommand);
                _commandQueued = true;
            }
        }
    }

    public override void ExitState()
    {
        _commandQueued = false;
        Context.QuestionText.SetActive(false);
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
        get => _commandQueued;
        set => _commandQueued = value;
    }
}
