using System.Collections;
using System.Collections.Generic;
using System.Runtime.ConstrainedExecution;
using UnityEngine;

public class EnemyReadyAttackState : EnemyBaseState
{
    private bool _commandQueued = false;
    public EnemyReadyAttackState(EnemyStateMachine context, EnemyStateFactory factory) : base(context, factory)
    {
    }

    public override void EnterState()
    {
        // Debug.Log("Enemy Now Ready to attack");
    }

    public override void UpdateState()
    {
        CheckDeathState();
        CheckSwitchStates();
        
        if (_commandQueued || TurnManager.Instance.CurrentTurn == TurnType.PlayerTurn) return;

        EnemyAttackCommand enemyAttackCommand = new EnemyAttackCommand(Context);
        TurnManager.Instance.AddQueue(enemyAttackCommand);
        _commandQueued = true;
    }

    public override void ExitState()
    {
        // Debug.Log("Exiting Ready to attack");
    }

    public override void CheckSwitchStates()
    {
        Vector2Int enemyCords = new Vector2Int(
            Mathf.RoundToInt(Context.Unit.position.x / Context.GridManager.UnityGridSize),
            Mathf.RoundToInt(Context.Unit.position.z / Context.GridManager.UnityGridSize)
        );
        
        Vector2Int playerCords = new Vector2Int(
            Mathf.RoundToInt(PlayerStateMachine.Instance.Unit.position.x / Context.GridManager.UnityGridSize),
            Mathf.RoundToInt(PlayerStateMachine.Instance.Unit.position.z / Context.GridManager.UnityGridSize)
        );

        if (Mathf.Abs(enemyCords.x - playerCords.x) + Mathf.Abs(enemyCords.y - playerCords.y) > 1.1f)
        {
            SwitchState(Factory.CreateAggro());
        }
    }
}
