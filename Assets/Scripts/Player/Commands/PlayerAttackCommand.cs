using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = System.Random;

public class PlayerAttackCommand : ICommand
{
    private PlayerStateMachine _context;
    private Enemy _target;
    private Random _random;
    public PlayerAttackCommand(PlayerStateMachine context, Enemy target)
    {
        _context = context;
        _target = target;
        _random = new Random();
    }

    public void Execute()
    {
        int damage = CalculateDamage();
        bool isCritical = RandomizeCritical();
        
        if (isCritical)
        {
            damage = CalculateCritical(damage);
            _context.CameraShakeEventChannel.RaiseEvent(0.2f, 0.02f);
        }

        _target.TakeDamage(damage);
        _target.EnemyStateMachine.showDamageText(damage, isCritical);
        
        Vector3 directionToTarget = _target.transform.position - _context.transform.position;
        directionToTarget.y = 0;
        _context.transform.forward = directionToTarget.normalized;
        _context.CurrentState.SwitchState(_context.StateFactory.CreateAttack());
    }

    private int CalculateDamage()
    {
        int defenseScalingFactor = 100;
        int defenseFactor = 1 - (_target.Defense / (_target.Defense + defenseScalingFactor));
        int damageOutput = Player.Instance.Attack * defenseFactor;

        return damageOutput + _random.Next(10);
    }

    private bool RandomizeCritical()
    {
        float criticalChance = Player.Instance.CriticalRate;
        float randomValue = UnityEngine.Random.value;
        return randomValue <= criticalChance;
    }

    private int CalculateCritical(int damage)
    {
        return (int) (Player.Instance.CriticalDamage * damage);
    }
}
