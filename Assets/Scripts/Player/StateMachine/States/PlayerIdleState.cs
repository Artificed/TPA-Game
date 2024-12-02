using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerIdleState: PlayerBaseState
{
    private bool _commandQueued;
    public PlayerIdleState(PlayerStateMachine context, PlayerStateFactory factory) : base(context, factory)
    {
    }

    public override void EnterState()
    {
        // Debug.Log("Player Entering Idle!");
        Context.CancellingPath = false;
        Context.Animator.SetBool(Context.IsMovingHash, false);

        _commandQueued = false;
    }

    public override void UpdateState()
    {
        CheckSwitchStates();
        if(_commandQueued) return;
        
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit) && hit.transform.CompareTag("Tile"))
            {
                HandleTileRaycast(hit);
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

        PlayerMoveCommand playerMoveCommand = new PlayerMoveCommand(Context, startCords, targetCords);
        TurnManager.Instance.AddQueue(playerMoveCommand);

        _commandQueued = true;
    }
    
    public override void ExitState()
    { 
        _commandQueued = false;
    }
    
    public override void CheckSwitchStates()
    {
        if (TurnManager.Instance.IsBattling)
        {
            SwitchState(Factory.CreateBattleIdle());
        }
    }
}