using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMoveCommand : ICommand
{
    private PlayerStateMachine _context;
    private Vector2Int _startCoords;
    private Vector2Int _targetCoords;

    public PlayerMoveCommand(PlayerStateMachine context, Vector2Int startCoords, Vector2Int targetCoords)
    {
        _context = context;
        _startCoords = startCoords;
        _targetCoords = targetCoords;
    }
    
    public void Execute()
    {
        _context.SetNewDestination(_startCoords, _targetCoords);
        _context.CurrentState.SwitchState(_context.StateFactory.CreateMoveBattle());
    }
}
