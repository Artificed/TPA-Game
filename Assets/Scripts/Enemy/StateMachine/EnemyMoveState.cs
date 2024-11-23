using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMoveState : EnemyBaseState
{
    private Coroutine _movementCoroutine;
    public EnemyMoveState(EnemyStateMachine context, EnemyStateFactory factory) : base(context, factory)
    {
    }

    public override void EnterState()
    {
        Context.Animator.SetBool(Context.IsMovingHash, true);   
        _movementCoroutine = Context.StartCoroutine(FollowPath());
    }

    public override void UpdateState()
    {
    }

    public override void ExitState()
    {
        Context.Animator.SetBool(Context.IsMovingHash, false);
        if (_movementCoroutine != null)
        {
            Context.StopCoroutine(_movementCoroutine);
            _movementCoroutine = null;
        }
    }
    
    private IEnumerator FollowPath()
    {
        if (Context.Path.Count == 0)
        {
            SwitchState(Factory.CreateIdle());
            yield break;
        }
        
        Tile targetNode = Context.Path[1];
        if (targetNode.Blocked)
        {
            Context.ClearPath();
            SwitchState(Factory.CreateIdle());
            yield break;
        }

        Vector3 startPosition = Context.Unit.position;
        Vector3 endPosition = Context.GridManager.GetPositionFromCoordinates(targetNode.coords);
        endPosition.y = 0.1f;
        float travelPercent = 0f;

        Context.Unit.LookAt(endPosition);

        while (travelPercent < 1f)
        {
            travelPercent += Time.deltaTime * Context.MovementSpeed;
            Context.Unit.position = Vector3.Lerp(startPosition, endPosition, travelPercent);
            yield return null;
        }

        SwitchState(Factory.CreateAggro());
    }

    public override void CheckSwitchStates()
    {
    }
}
