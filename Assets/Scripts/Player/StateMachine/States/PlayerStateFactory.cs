using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class PlayerStateFactory
{
    private PlayerStateMachine _context;

    public PlayerStateFactory(PlayerStateMachine context)
    {
        _context = context;
    }

    public PlayerBaseState CreateIdle()
    {
        return new PlayerIdleState(_context, this);
    }

    public PlayerBaseState CreateMoving()
    {
        return new PlayerMoveState(_context, this);
    }

    public PlayerBaseState CreateBattle()
    {
        return new PlayerBattleState(_context, this);
    }

    public PlayerBaseState CreateAttack()
    {
        return new PlayerAttackState(_context, this);
    }
}