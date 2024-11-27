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
        int damage = Player.Instance.Attack;
        bool isCritical = RandomizeCritical();
        
        if (isCritical)
        {
            damage = CalculateCritical(damage);
            _context.CameraShakeEventChannel.RaiseEvent(0.2f, 0.02f);
        }
        
        damage = RandomizeDamage(damage);
        damage = ApplySkills(damage);
        damage = CalculateDamageOutput(damage);
        
        if (EnemyDead(damage))
        {
            Player.Instance.AddExp(_target.XpDrop);
            Player.Instance.AddZhen(_target.ZhenDrop);
        }
        
        _target.TakeDamage(damage);
        _target.EnemyStateMachine.showDamageText(damage, isCritical);
        
        RotatePlayer();
        _context.CurrentState.SwitchState(_context.StateFactory.CreateAttack());
    }

    private int CalculateDamageOutput(int damage)
    {
        int defenseScalingFactor = 100;
        int defenseFactor = 1 - (_target.Defense / (_target.Defense + defenseScalingFactor));
        int damageOutput = damage * defenseFactor;

        return damageOutput;
    }

    private int RandomizeDamage(int damage)
    {
        return damage + _random.Next(10);
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

    private bool EnemyDead(int damage)
    {
        return damage >= _target.Health;
    }

    private int ApplySkills(int damage)
    {
        ActiveSkill bashSkill = (ActiveSkill) SkillManager.Instance.GetSkill("Bash");
        if (bashSkill.IsActive)
        {
            damage += (int) (damage * 0.5);
            Debug.Log("Bash Used - Damage: " + damage);
            bashSkill.UseSkill();
        }

        return damage;
    }

    private void RotatePlayer()
    {
        Vector3 directionToTarget = _target.transform.position - _context.transform.position;
        directionToTarget.y = 0;
        _context.transform.forward = directionToTarget.normalized;
    }
}
