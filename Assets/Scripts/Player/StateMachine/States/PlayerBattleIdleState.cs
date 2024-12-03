using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerBattleIdleState : PlayerBaseState
{
    private bool _commandQueued = false;
    public PlayerBattleIdleState(PlayerStateMachine context, PlayerStateFactory factory) : base(context, factory)
    {
    }

    public override void EnterState()
    {
        // Debug.Log("Player Battling");
        _commandQueued = false;
    }

    public override void UpdateState()
    {
        CheckDeathState();
        CheckSwitchStates();
        if(_commandQueued) return;
        
        if (Input.GetKeyDown(KeyCode.Space))
        {
            PlayerSkipCommand playerSkipCommand = new PlayerSkipCommand(Context);
            TurnManager.Instance.AddQueue(playerSkipCommand);
            _commandQueued = true;
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
        
        else if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            PlayerLifeStealSkillCommand playerLifeStealSkillCommand = new PlayerLifeStealSkillCommand();
            playerLifeStealSkillCommand.Execute();
        } 
        
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            PlayerBashSkillCommand playerBashSkillCommand = new PlayerBashSkillCommand();
            playerBashSkillCommand.Execute();
        }
    }

    private void HandleTileRaycast(RaycastHit hit)
    {
        if (hit.transform.GetComponent<Tile>().Blocked) return;
        
        Vector2Int targetCords = hit.transform.GetComponent<Tile>().coords;
        Vector2Int startCords = new Vector2Int(
            Mathf.RoundToInt(Context.Unit.position.x / Context.GridManager.UnityGridSize),
            Mathf.RoundToInt(Context.Unit.position.z / Context.GridManager.UnityGridSize)
        );
        
        if(startCords == targetCords) return;
        if (Vector2Int.Distance(startCords, targetCords) <= 1)
        {
            if (GetEnemy(targetCords))
            {
                return;
            }
        }

        PlayerMoveBattleCommand playerMoveBattleCommand = new PlayerMoveBattleCommand(Context, startCords, targetCords);
        TurnManager.Instance.AddQueue(playerMoveBattleCommand);
        _commandQueued = true;
    }

    private void HandleEnemyRaycast(RaycastHit hit)
    {
        Vector3 targetPosition = hit.transform.position;
        Vector2Int targetCords = new Vector2Int((int)targetPosition.x, (int)targetPosition.z);
    
        if (!IsEnemyInRange(targetCords)) return;

        TurnManager.Instance.CurrentEnemyTarget = GetEnemy(targetCords).EnemyStateMachine;
        PlayerAttackCommand playerAttackCommand = new PlayerAttackCommand(Context, GetEnemy(targetCords));
        TurnManager.Instance.AddQueue(playerAttackCommand);
        _commandQueued = true;
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

    private Enemy GetEnemy(Vector2Int targetCords)
    {
        foreach (Enemy enemy in TurnManager.Instance.ActiveEnemies)
        {
            Vector2Int enemyCoords = new Vector2Int(
                Mathf.FloorToInt(enemy.EnemyStateMachine.Unit.position.x),
                Mathf.FloorToInt(enemy.EnemyStateMachine.Unit.position.z)
            );
            if (enemyCoords == targetCords)
            {
                return enemy;
            }
        }
        return null;
    }

    public override void ExitState()
    {
        _commandQueued = false;
    }

    public override void CheckSwitchStates()
    {
        if (!TurnManager.Instance.IsBattling)
        {
            SwitchState(Context.StateFactory.CreateIdle());
        }
    }
}
