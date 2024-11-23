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
        
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit) && hit.transform.CompareTag("Tile"))
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
        }
        else if (Input.GetKeyDown(KeyCode.Space))
        {
            PlayerSkipCommand playerSkipCommand = new PlayerSkipCommand(Context);
            TurnManager.Instance.AddQueue(playerSkipCommand);
            commandQueued = true;
        }
    }

    public override void ExitState()
    {
        commandQueued = false;
    }

    public override void CheckSwitchStates()
    {
        
    }
}
