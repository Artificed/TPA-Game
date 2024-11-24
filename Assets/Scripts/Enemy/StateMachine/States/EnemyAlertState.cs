using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAlertState : EnemyBaseState
{
    public EnemyAlertState(EnemyStateMachine context, EnemyStateFactory factory) : base(context, factory)
    {
        
    }

    public override void EnterState()
    {
        Debug.Log("Enemy in Alert State");
        Context.Animator.SetBool(Context.IsAlertHash, true);
    }

    public override void UpdateState()
    {
        if (Context.GetDistanceFromPlayer() > Context.AlertRange)
        {
            SwitchState(Factory.CreateIdle());
        } 
        else if (CheckLOS())
        {
            SwitchState(Factory.CreateAggro());
        }
    }

    private bool CheckLOS()
    {
        Transform enemyTransform = Context.transform;
        Transform playerTransform = PlayerStateMachine.Instance.transform;

        Vector3 src = new Vector3(enemyTransform.position.x, 0f, enemyTransform.position.z); 
        Vector3 dst = new Vector3(playerTransform.position.x, 0f, playerTransform.position.z); 
        
        Vector3 directionToPlayer = (dst - src).normalized; 
        float rayLength = Vector3.Distance(src, dst);
        
        Debug.DrawRay(src, directionToPlayer * rayLength, Color.red, 1f);
        List<Tile> tilesAlongRay = GetTilesAlongRay(src, directionToPlayer, rayLength);

        foreach (Tile tile in tilesAlongRay)
        {
            if (tile.Blocked)
            {
                // Debug.Log($"LOS blocked at {tile.coords}");
                return false;
            }
        }

        // Debug.Log("Valid");
        return true;
    }
    
    private List<Tile> GetTilesAlongRay(Vector3 rayStart, Vector3 rayDirection, float rayLength)
    {
        List<Tile> hitTiles = new List<Tile>();
        RaycastHit[] hits = Physics.RaycastAll(rayStart, rayDirection, rayLength);

        foreach (RaycastHit hit in hits)
        {
            Tile tile = hit.transform.GetComponent<Tile>();
            if (tile != null)
            {
                hitTiles.Add(tile);
            }
        }

        return hitTiles;
    }   

    public override void ExitState()
    {
    }

    public override void CheckSwitchStates()
    {
    }
}
