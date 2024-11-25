using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAggroState : EnemyBaseState
{
    private bool _commandQueued = false;
    public EnemyAggroState(EnemyStateMachine context, EnemyStateFactory factory) : base(context, factory)
    {
    }

    public override void EnterState()
    {
        // Debug.Log("Enemy Aggro");
        Context.ExclamationText.SetActive(true);
        Context.StartCoroutine(DeactivateExclamationAfterDelay(1.0f));
        
        Transform playerTransform = PlayerStateMachine.Instance.transform;
        
        Vector3 playerDirection = playerTransform.position - Context.transform.position;
        playerDirection.y = 0;
        
        Quaternion lookRotation = Quaternion.LookRotation(playerDirection);
        Context.transform.rotation = lookRotation;
        
        _commandQueued = false;
    }
    
    private IEnumerator DeactivateExclamationAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        Context.ExclamationText.SetActive(false);
    }

    public override void UpdateState()
    {
        CheckSwitchStates();
        
        if (TurnManager.Instance.CurrentTurn == TurnType.PlayerTurn || _commandQueued) return;

        Vector2Int startCords = new Vector2Int(
            Mathf.RoundToInt(Context.Unit.position.x / Context.GridManager.UnityGridSize),
            Mathf.RoundToInt(Context.Unit.position.z / Context.GridManager.UnityGridSize)
        );
        
        Vector2Int targetCords = new Vector2Int(
            Mathf.RoundToInt(PlayerStateMachine.Instance.Unit.position.x / Context.GridManager.UnityGridSize),
            Mathf.RoundToInt(PlayerStateMachine.Instance.Unit.position.z / Context.GridManager.UnityGridSize)
        );

        EnemyMoveCommand enemyMoveCommand = new EnemyMoveCommand(Context, startCords, targetCords);
        TurnManager.Instance.AddQueue(enemyMoveCommand);
        _commandQueued = true;
    }

    public override void ExitState()
    {
        _commandQueued = false;
    }

    public override void CheckSwitchStates()
    {
        Vector2Int enemyCords = new Vector2Int(
            Mathf.RoundToInt(Context.Unit.position.x / Context.GridManager.UnityGridSize),
            Mathf.RoundToInt(Context.Unit.position.z / Context.GridManager.UnityGridSize)
        );
        
        Vector2Int playerCords = new Vector2Int(
            Mathf.RoundToInt(PlayerStateMachine.Instance.Unit.position.x / Context.GridManager.UnityGridSize),
            Mathf.RoundToInt(PlayerStateMachine.Instance.Unit.position.z / Context.GridManager.UnityGridSize)
        );

        if (Mathf.Abs(enemyCords.x - playerCords.x) + Mathf.Abs(enemyCords.y - playerCords.y) == 1)
        {
            SwitchState(Factory.CreateReadyAttack());
        }
    }
}
