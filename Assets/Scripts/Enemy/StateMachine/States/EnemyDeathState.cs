using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDeathState : EnemyBaseState
{
    public EnemyDeathState(EnemyStateMachine context, EnemyStateFactory factory) : base(context, factory)
    {
    }

    public override void EnterState()
    {
        Debug.Log("Enemy Died!");
        SoundFXManager.Instance.PlayDeathClip(Context.transform);
        TurnManager.Instance.RemoveAggroEnemy(Context.Enemy);
        Context.Animator.SetTrigger(Context.IsDeadHash);
    }

    public override void UpdateState()
    {
        CheckSwitchStates();
    }

    public override void ExitState()
    {
        
    }

    public override void CheckSwitchStates()
    {
        AnimatorStateInfo stateInfo = Context.Animator.GetCurrentAnimatorStateInfo(0);

        if (stateInfo.IsName("Death") && stateInfo.normalizedTime >= 1.0f)
        {
            TurnManager.Instance.RemoveEnemy(Context.Enemy);
            Object.Destroy(Context.gameObject);
        }
    }
}
