using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBattleState : PlayerBaseState
{
    private bool commandQueued = false;
    public PlayerBattleState(PlayerStateMachine context, PlayerStateFactory factory) : base(context, factory)
    {
    }

    public override void EnterState()
    {
        Debug.Log("Player Battling");
        commandQueued = false;
    }

    public override void UpdateState()
    {
        if(commandQueued) return;
        
        if (Input.GetKeyDown(KeyCode.Space))
        {
            PlayerSkipCommand playerSkipCommand = new PlayerSkipCommand(Context);
            TurnManager.Instance.AddQueue(playerSkipCommand);
            commandQueued = true;
        }
        
        else if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            
            if (Physics.Raycast(ray, out hit))
            {
                if (hit.transform.CompareTag("Tile"))
                {
                    HandleTileRaycast(hit);
                }
                else if (hit.transform.CompareTag("Enemy"))
                {
                    HandleEnemyRaycast(hit);
                }
            }
        }
    }

    private void HandleTileRaycast(RaycastHit hit)
    {
        Vector2Int targetCords = hit.transform.GetComponent<Tile>().coords;
        Vector2Int startCords = new Vector2Int(
            Mathf.RoundToInt(Context.Unit.position.x / Context.GridManager.UnityGridSize),
            Mathf.RoundToInt(Context.Unit.position.z / Context.GridManager.UnityGridSize)
        );

        PlayerMoveCommand playerMoveCommand = new PlayerMoveCommand(Context, startCords, targetCords);
        TurnManager.Instance.AddQueue(playerMoveCommand);
        commandQueued = true;
    }

    private void HandleEnemyRaycast(RaycastHit hit)
    {
        Vector2Int targetCords = new Vector2Int((int) hit.transform.position.x, (int) hit.transform.position.z);
        if (!IsEnemyInRange(targetCords)) return;

        PlayerAttackCommand playerAttackCommand = new PlayerAttackCommand(Context);
        TurnManager.Instance.AddQueue(playerAttackCommand);
        commandQueued = true;
    }

    private bool IsEnemyInRange(Vector2Int cord)
    {
        Vector2Int playerCords = new Vector2Int(
            Mathf.RoundToInt(Context.Unit.position.x / Context.GridManager.UnityGridSize),
            Mathf.RoundToInt(Context.Unit.position.z / Context.GridManager.UnityGridSize)
        );
        
        if (Mathf.Abs(cord.x - playerCords.x) + Mathf.Abs(cord.y - playerCords.y) <= 1.0f)
        {
            return true;
        }

        return false;
    }

    public override void ExitState()
    {
        commandQueued = false;
    }

    public override void CheckSwitchStates()
    {
        
    }
}
