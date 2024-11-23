using UnityEngine;

public class PlayerSkipCommand : ICommand
{
    private PlayerStateMachine _context;

    public PlayerSkipCommand(PlayerStateMachine context)
    {
        _context = context;
    }
    public void Execute()
    {
        _context.PlayerTurnEventChannel.RaiseEvent();
        _context.CurrentState.SwitchState(_context.StateFactory.CreateIdle());
    }
}
