using System.Collections;
using System.Collections.Generic;
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
            
            Vector2Int endPosition2D = new Vector2Int((int) endPosition.x, (int) endPosition.z);
            if (!ValidDestination(endPosition2D))
            {
                Debug.Log("Blocked by enemy/player at follow path");
                Context.ClearPath();
                Context.PlayerTurnEventChannel.RaiseEvent();
                SwitchState(Factory.CreateIdle());
                yield break;
            }
            
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
    
    private bool ValidDestination(Vector2Int targetCoords)
    {
        List<EnemyStateMachine> enemies = TurnManager.Instance.Enemies;
        foreach (EnemyStateMachine enemy in enemies)
        {
            Vector2Int enemyCoords = new Vector2Int((int) enemy.Unit.position.x, (int) enemy.Unit.position.z);
            if (targetCoords == enemyCoords) return false;
        }
        
        PlayerStateMachine player = PlayerStateMachine.Instance;
        Vector2Int playerCoords = new Vector2Int((int) player.Unit.position.x, (int) player.Unit.position.z);
        if (targetCoords == playerCoords) return false;
        
        return true;
    }

    private void StopMovement()
    {
        _shouldStop = true; 
        Context.CancellingPath = true;
    }
}