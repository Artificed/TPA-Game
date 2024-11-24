using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyCheckLOSCommand : ICommand
{
    private EnemyStateMachine _context;

    public EnemyCheckLOSCommand(EnemyStateMachine context)
    {
        _context = context;
    }

    public void Execute()
    {
        if (CheckLOS())
        {
            // Debug.Log("LOS Valid");
            _context.EnemyActionCompleteEventChannel.RaiseEvent();
            _context.CurrentState.SwitchState(_context.StateFactory.CreateAggro());
        }
        else
        {
            // Debug.Log("LOS Blocked!");
            _context.EnemyActionCompleteEventChannel.RaiseEvent();
            if (!(_context.CurrentState is EnemyAlertState))
            {
                _context.CurrentState.SwitchState(_context.StateFactory.CreateAlert());
            }
        }
    }
    
    private bool CheckLOS()
    {
        Transform enemyTransform = _context.transform;
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
                return false;
            }
        }

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
}
