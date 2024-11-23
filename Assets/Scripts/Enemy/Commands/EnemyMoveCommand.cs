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
    
    public void Execute()
    {
        Debug.Log("Start: " + _startCoords + " - Target: " + _targetCoords);
        _context.SetNewDestination(_startCoords, _targetCoords);
        _context.CurrentState.SwitchState(_context.StateFactory.CreateMoving());
    }
}
