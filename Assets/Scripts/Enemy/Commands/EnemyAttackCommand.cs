using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttackCommand : ICommand
{
    private EnemyStateMachine _context;

    public EnemyAttackCommand(EnemyStateMachine context)
    {
        _context = context;
    }

    public void Execute()
    {
        if (CheckPlayerPosition())
        {
            // Debug.Log("Enemy is Attacking!");
            _context.CurrentState.SwitchState(_context.StateFactory.CreateAttack());
        }
        else
        {
            // Debug.Log("Player too far, back to aggro!");
            _context.CurrentState.SwitchState(_context.StateFactory.CreateAggro());
        }
    }
    
    public bool CheckPlayerPosition()
    {
        Vector2Int enemyCords = new Vector2Int(
            Mathf.RoundToInt(_context.Unit.position.x / _context.GridManager.UnityGridSize),
            Mathf.RoundToInt(_context.Unit.position.z / _context.GridManager.UnityGridSize)
        );
        
        Vector2Int playerCords = new Vector2Int(
            Mathf.RoundToInt(PlayerStateMachine.Instance.Unit.position.x / _context.GridManager.UnityGridSize),
            Mathf.RoundToInt(PlayerStateMachine.Instance.Unit.position.z / _context.GridManager.UnityGridSize)
        );

        if (Mathf.Abs(enemyCords.x - playerCords.x) + Mathf.Abs(enemyCords.y - playerCords.y) > 1)
        {
            return false;
        }

        return true;
    }
}
