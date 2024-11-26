using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMoveState : PlayerBaseState
{
    private Coroutine _movementCoroutine;
    
    public PlayerMoveState(PlayerStateMachine context, PlayerStateFactory factory) : base(context, factory)
    {
    }

    public override void EnterState()
    {
        // Debug.Log("Player Entering Move State");
        Context.Animator.SetBool(Context.IsMovingHash, true);   
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
            
            Vector2Int endPosition2D = new Vector2Int((int) endPosition.x, (int) endPosition.z);
            float travelPercent = 0f;

            Context.transform.LookAt(endPosition);
            while (travelPercent < 1f)
            {
                travelPercent += Time.deltaTime * Context.MovementSpeed;
                Context.Unit.position = Vector3.Lerp(startPosition, endPosition, travelPercent);
                yield return null;
            }
            
            if (Context.CancellingPath)
            {
                Context.ClearPath();
                Context.PlayerTurnEventChannel.RaiseEvent();
                SwitchState(Factory.CreateIdle());
                Debug.Log("Player transitioning back to idle");
                yield break;
            }
        }

        SwitchState(Factory.CreateIdle());
    }

    private void StopMovement()
    {
        Context.CancellingPath = true;
    }
}