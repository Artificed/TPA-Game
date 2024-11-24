using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttackCommand : ICommand
{
    private PlayerStateMachine _context;

    public PlayerAttackCommand(PlayerStateMachine context)
    {
        _context = context;
    }

    public void Execute()
    {
        _context.CurrentState.SwitchState(_context.StateFactory.CreateAttack());
    }
}
