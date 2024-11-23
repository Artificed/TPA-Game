using System.Collections;
using UnityEngine;

public class PlayerMoveState : PlayerBaseState
{
    private Coroutine _movementCoroutine;

    private bool _shouldStop;

    public PlayerMoveState(PlayerStateMachine context, PlayerStateFactory factory) : base(context, factory)
    {
    }

    public override void EnterState()
    {
        Context.Animator.SetBool(Context.IsMovingHash, true);   
        _shouldStop = false;
        _movementCoroutine = Context.StartCoroutine(FollowPath());
        Context.CancellingPath = false;
    }

    public override void UpdateState()
    {
        if (Input.GetMouseButtonDown(0) && !Context.CancellingPath)
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit) && hit.transform.CompareTag("Tile"))
            {
                StopMovement();
            }
        }
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

            if (_shouldStop || Context.WithinEnemyRange || Context.CancellingPath)
            {
                Context.ClearPath();
                Context.PlayerTurnEventChannel.RaiseEvent();
                SwitchState(Factory.CreateIdle());
                yield break;
            }
        }

        SwitchState(Factory.CreateIdle());
    }

    private void StopMovement()
    {
        _shouldStop = true; 
        Context.CancellingPath = true;
    }
}