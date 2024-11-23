using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMoveCommand : ICommand
{
    private EnemyStateMachine _context;
    private Vector2Int _startCoords;
    private Vector2Int _targetCoords;

    public EnemyMoveCommand(EnemyStateMachine context, Vector2Int startCoords, Vector2Int targetCoords)
    {
        _context = context;
        _startCoords = startCoords;
        _targetCoords = targetCoords;
    }

    private bool ValidCoord()
    {
        List<EnemyStateMachine> enemies = TurnManager.Instance.Enemies;
        foreach (EnemyStateMachine enemy in enemies)
        {
            Debug.Log(enemy.Unit.position);
            Vector2Int enemyCoords = new Vector2Int((int) enemy.Unit.position.x, (int) enemy.Unit.position.z);
            Debug.Log(enemyCoords);
            if (_targetCoords == enemyCoords)
            {
                return false;
            }
        }
        return true;
    }
    
    public void Execute()
    {
        if (!ValidCoord()) return;
        _context.SetNewDestination(_startCoords, _targetCoords);
        _context.CurrentState.SwitchState(_context.StateFactory.CreateMoving());
    }
}
