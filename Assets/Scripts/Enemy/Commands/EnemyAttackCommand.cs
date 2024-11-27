using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = System.Random;

public class EnemyAttackCommand : ICommand
{
    private EnemyStateMachine _context;
    private Random _random = new Random();

    public EnemyAttackCommand(EnemyStateMachine context)
    {
        _context = context;
    }

    public void Execute()
    {
        if (CheckPlayerPosition())
        {
            int damage = CalculateDamage();
            bool isCritical = RandomizeCritical();
            
            if (isCritical)
            {
                damage = CalculateCritical(damage);
                _context.CameraShakeEventChannel.RaiseEvent(0.2f, 0.02f);
            }
            
            Player.Instance.TakeDamage(damage);
            PlayerStateMachine.Instance.showDamageText(damage, isCritical);
            
            _context.CurrentState.SwitchState(_context.StateFactory.CreateAttack());
        }
        else
        {
            // Debug.Log("Player too far, back to aggro!");
            _context.CurrentState.SwitchState(_context.StateFactory.CreateAggro());
        }
    }
    
    public bool CheckPlayerPosition()
    {
        Vector2Int enemyCords = new Vector2Int(
            Mathf.RoundToInt(_context.Unit.position.x / _context.GridManager.UnityGridSize),
            Mathf.RoundToInt(_context.Unit.position.z / _context.GridManager.UnityGridSize)
        );
        
        Vector2Int playerCords = new Vector2Int(
            Mathf.RoundToInt(PlayerStateMachine.Instance.Unit.position.x / _context.GridManager.UnityGridSize),
            Mathf.RoundToInt(PlayerStateMachine.Instance.Unit.position.z / _context.GridManager.UnityGridSize)
        );

        if (Mathf.Abs(enemyCords.x - playerCords.x) + Mathf.Abs(enemyCords.y - playerCords.y) > 1)
        {
            return false;
        }

        return true;
    }
    
    private int CalculateDamage()
    {
        int defenseScalingFactor = 100;
        int defenseFactor = 1 - (Player.Instance.Defense / (Player.Instance.Defense + defenseScalingFactor));
        int damageOutput = _context.Enemy.Attack * defenseFactor;

        switch (_context.Enemy.EnemyType)
        {
            case EnemyType.Common:
                damageOutput = Mathf.CeilToInt((damageOutput) + _random.Next(1)); 
                break;
             
            case EnemyType.Medium:
                damageOutput = Mathf.CeilToInt((damageOutput) + _random.Next(6)); 
                break;
             
            case EnemyType.Elite:
                damageOutput = Mathf.CeilToInt((damageOutput) + _random.Next(10)); 
                break;
        }
        return damageOutput;
    }
    
    private bool RandomizeCritical()
    {
        float criticalChance = _context.Enemy.CriticalRate;
        float randomValue = UnityEngine.Random.value;
        return randomValue <= criticalChance;
    }

    private int CalculateCritical(int damage)
    {
        return (int) (_context.Enemy.CriticalDamage * damage);
    }
}
