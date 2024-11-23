using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBattleState : PlayerBaseState
{
    public PlayerBattleState(PlayerStateMachine context, PlayerStateFactory factory) : base(context, factory)
    {
    }

    public override void EnterState()
    {
        Debug.Log("Player Battling");
    }

    public override void UpdateState()
    {
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
                TurnManager.Instance.SwitchToEnemyTurn();
            }
        }
    }

    public override void ExitState()
    {
        
    }

    public override void CheckSwitchStates()
    {
        
    }
}
