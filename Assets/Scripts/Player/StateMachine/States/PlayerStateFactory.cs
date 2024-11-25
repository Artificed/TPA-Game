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

    public PlayerBaseState CreateBattleIdle()
    {
        return new PlayerBattleIdleState(_context, this);
    }

    public PlayerBaseState CreateAttack()
    {
        return new PlayerAttackState(_context, this);
    }

    public PlayerBaseState CreateMoveBattle()
    {
        return new PlayerMoveBattleState(_context, this);
    }
}