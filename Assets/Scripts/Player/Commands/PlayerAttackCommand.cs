using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttackCommand : ICommand
{
    private PlayerStateMachine _context;
    private Enemy _target;

    public PlayerAttackCommand(PlayerStateMachine context, Enemy target)
    {
        _context = context;
        _target = target;
    }

    public void Execute()
    {
        _target.TakeDamage(Player.Instance.Attack);
        
        Vector3 directionToTarget = _target.transform.position - _context.transform.position;
        directionToTarget.y = 0;
        _context.transform.rotation = Quaternion.LookRotation(directionToTarget);
        _context.CurrentState.SwitchState(_context.StateFactory.CreateAttack());
    }
}
