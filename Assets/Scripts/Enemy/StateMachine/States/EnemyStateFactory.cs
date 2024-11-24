using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStateFactory
{
    private EnemyStateMachine _context;

    public EnemyStateFactory(EnemyStateMachine context)
    {
        _context = context;
    }

    public EnemyBaseState CreateIdle()
    {
        return new EnemyIdleState(_context, this);
    }

    public EnemyBaseState CreateAlert()
    {
        return new EnemyAlertState(_context, this);
    }

    public EnemyBaseState CreateReadyAttack()
    {
        return new EnemyReadyAttackState(_context, this);
    }

    public EnemyBaseState CreateAggro()
    {
        return new EnemyAggroState(_context, this);
    }

    public EnemyBaseState CreateMoving()
    {
        return new EnemyMoveState(_context, this);
    }

    public EnemyBaseState CreateAttack()
    {
        return new EnemyAttackState(_context, this);
    }
}
