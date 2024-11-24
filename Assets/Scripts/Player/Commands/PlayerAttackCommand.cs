using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttackCommand : ICommand
{
    private PlayerStateMachine _context;
    private Vector3 _targetPosition;

    public PlayerAttackCommand(PlayerStateMachine context, Vector3 targetPosition)
    {
        _context = context;
        _targetPosition = targetPosition;
    }

    public void Execute()
    {
        Vector3 directionToTarget = _targetPosition - _context.transform.position;
        directionToTarget.y = 0; 
        _context.transform.rotation = Quaternion.LookRotation(directionToTarget);

        _context.CurrentState.SwitchState(_context.StateFactory.CreateAttack());
    }
}
