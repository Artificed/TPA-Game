using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAggroState : EnemyBaseState
{
    public EnemyAggroState(EnemyStateMachine context, EnemyStateFactory factory) : base(context, factory)
    {
    }

    public override void EnterState()
    {
        Transform playerTransform = PlayerStateMachine.Instance.transform;
        
        Vector3 playerDirection = playerTransform.position - Context.transform.position;
        playerDirection.y = 0;
        
        Quaternion lookRotation = Quaternion.LookRotation(playerDirection);
        Context.transform.rotation = lookRotation;
    }

    public override void UpdateState()
    {
        if (TurnManager.Instance.CurrentTurn == TurnType.PlayerTurn) return;

        Vector2Int startCords = new Vector2Int(
            Mathf.RoundToInt(Context.Unit.position.x / Context.GridManager.UnityGridSize),
            Mathf.RoundToInt(Context.Unit.position.z / Context.GridManager.UnityGridSize)
        );
        
        Vector2Int targetCords = new Vector2Int(
            Mathf.RoundToInt(PlayerStateMachine.Instance.Unit.position.x / Context.GridManager.UnityGridSize),
            Mathf.RoundToInt(PlayerStateMachine.Instance.Unit.position.z / Context.GridManager.UnityGridSize)
        );

        EnemyMoveCommand enemyMoveCommand = new EnemyMoveCommand(Context, startCords, targetCords);
        TurnManager.Instance.AddQueue(enemyMoveCommand);
    }

    public override void ExitState()
    {
    }

    public override void CheckSwitchStates()
    {
    }
}
