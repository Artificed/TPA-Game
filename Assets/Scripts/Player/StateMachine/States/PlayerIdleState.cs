using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerIdleState: PlayerBaseState
{
    public PlayerIdleState(PlayerStateMachine context, PlayerStateFactory factory) : base(context, factory)
    {
    }

    public override void EnterState()
    {
        Debug.Log("Player Entering Idle!");
        Context.CancellingPath = false;
        Context.Animator.SetBool(Context.IsMovingHash, false);
    }

    public override void UpdateState()
    {
        if (TurnManager.Instance.IsBattling)
        {
            SwitchState(Factory.CreateBattle());
        }
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
                
                Context.SetNewDestination(startCords, targetCords);
                SwitchState(Factory.CreateMoving());
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