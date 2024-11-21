using System.Collections;
using UnityEngine;

public class PlayerMoveState : PlayerBaseState
{
    public PlayerMoveState(PlayerStateMachine context, PlayerStateFactory factory) : base(context, factory)
    {
    }

    public override void EnterState()
    {
        Context.Animator.SetBool(Context.IsMovingHash, true);
        Context.StartCoroutine(FollowPath());
    }

    public override void UpdateState()
    {
    }

    public override void ExitState()
    {
        Context.Animator.SetBool(Context.IsMovingHash, false);
    }

    public override void CheckSwitchStates()
    {
    }

    private IEnumerator FollowPath()
    {
        if (Context.Path.Count == 0)
        {
            SwitchState(Factory.CreateIdle());
            yield break;
        }

        for (int i = 1; i < Context.Path.Count; i++)
        {
            Tile targetNode = Context.Path[i];
            if (targetNode.Blocked)
            {
                Context.ClearPath();
                SwitchState(Factory.CreateIdle());
                yield break;
            }

            Vector3 startPosition = Context.Unit.position;
            Vector3 endPosition = Context.GridManager.GetPositionFromCoordinates(targetNode.coords);
            float travelPercent = 0f;

            Context.Unit.LookAt(endPosition);

            while (travelPercent < 1f)
            {
                travelPercent += Time.deltaTime * Context.MovementSpeed;
                Context.Unit.position = Vector3.Lerp(startPosition, endPosition, travelPercent);
                yield return null;
            }
        }

        SwitchState(Factory.CreateIdle());
    }

}