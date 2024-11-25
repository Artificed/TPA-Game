using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMoveBattleState : PlayerBaseState
{
    private Coroutine _movementCoroutine;
    public PlayerMoveBattleState(PlayerStateMachine context, PlayerStateFactory factory) : base(context, factory)
    {
    }

    public override void EnterState()
    {
        Debug.Log("Player Entering Battle Move State");
        Context.Animator.SetBool(Context.IsMovingHash, true);   
        _movementCoroutine = Context.StartCoroutine(FollowPath());
        Context.CancellingPath = false;
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
            SwitchState(Factory.CreateBattleIdle());
            yield break;
        }
        
        Tile targetNode = Context.Path[1];
        if (targetNode.Blocked)
        {
            Context.ClearPath();
            SwitchState(Factory.CreateBattleIdle());
            yield break;
        }

        Vector3 startPosition = Context.Unit.position;
        Vector3 endPosition = Context.GridManager.GetPositionFromCoordinates(targetNode.coords);
            
        Vector2Int endPosition2D = new Vector2Int((int) endPosition.x, (int) endPosition.z);
            
        if (!ValidDestination(endPosition2D))
        {
            Debug.Log("Blocked by enemy/player at follow path");
            Context.ClearPath();
            if (TurnManager.Instance.IsBattling)
            {
                Context.PlayerTurnEventChannel.RaiseEvent();
                SwitchState(Factory.CreateBattleIdle());
            }
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

        Context.ClearPath();
        Context.PlayerTurnEventChannel.RaiseEvent();
        if (TurnManager.Instance.Enemies.Count > 0)
        {
            SwitchState(Factory.CreateBattleIdle());
            Debug.Log("Player Transitioning to battle");
        }
        else
        {
            SwitchState(Factory.CreateIdle());
            Debug.Log("Player Transitioning to idle from battle");
        }
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

    public override void CheckSwitchStates()
    {
    }
}
