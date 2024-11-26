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
        // Debug.Log("FollowPath Entered");
        if (Context.Path.Count == 0)
        {
            // Debug.Log("Count 0 at follow path");
            Context.EnemyActionCompleteEventChannel.RaiseEvent();
            SwitchState(Factory.CreateAggro());
            yield break;
        }
        
        Tile targetNode = Context.Path[1];
        if (targetNode.Blocked)
        {
            // Debug.Log("Blocked tile at follow path");
            Context.ClearPath();
            Context.EnemyActionCompleteEventChannel.RaiseEvent();
            SwitchState(Factory.CreateAggro());
            yield break;
        }

        // Debug.Log("Follow Path ok");
        Vector3 startPosition = Context.Unit.position;
        Vector3 endPosition = Context.GridManager.GetPositionFromCoordinates(targetNode.coords);

        Vector2Int endPosition2D = new Vector2Int((int) endPosition.x, (int) endPosition.z);
        if (!ValidDestination(endPosition2D))
        {
            // Debug.Log("Blocked by enemy/player at follow path");
            Context.ClearPath();
            Context.EnemyActionCompleteEventChannel.RaiseEvent();
            SwitchState(Factory.CreateAggro());
            yield break;
        }
        
        endPosition.y = 0.1f;
        float travelPercent = 0f;

        Context.Unit.LookAt(endPosition);

        while (travelPercent < 1f)
        {
            travelPercent += Time.deltaTime * Context.MovementSpeed;
            Context.Unit.position = Vector3.Lerp(startPosition, endPosition, travelPercent);
            yield return null;
        }

        Context.EnemyActionCompleteEventChannel.RaiseEvent();
        SwitchState(Factory.CreateAggro());
    }
    
    private bool ValidDestination(Vector2Int targetCoords)
    {
        List<Enemy> enemies = TurnManager.Instance.ActiveEnemies;
        foreach (Enemy enemy in enemies)
        {
            Vector2Int enemyCoords = new Vector2Int((int) enemy.EnemyStateMachine.Unit.position.x, (int) enemy.EnemyStateMachine.Unit.position.z);
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
