using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuffSkill : Skill
{
    private int _activeTime;
    private int _remainingTurns;
    
    public override void Initialize(SkillDataSO data)
    {
        SkillName = data.skillName;
        Description = data.description;
        UnlockLevel = data.unlockLevel;
        CooldownTime = data.cooldownTime;
        _activeTime = data.activeTime;
        ImageIcon = data.imageIcon;
        SkillKey = data.skillKey;
        
        IsUnlocked = Player.Instance.Level >= UnlockLevel;
        
        RemainingCooldown = 0;
        _remainingTurns = 0;
    }

    public override void HandlePlayerTurn()
    {
        DecreaseCooldown();
        Debug.Log("Cooldown decreased");
        PlayerStateMachine.Instance.PlayerBuffSkillEventChannel.RaiseEvent(this);
    }

    public void ToggleSkill()
    {
        if(RemainingCooldown > 0) return;
        PlayerStateMachine.Instance.ActivateParticles();
        
        _remainingTurns = _activeTime;
        RemainingCooldown = CooldownTime;
        
        PlayerStateMachine.Instance.Animator.SetTrigger(PlayerStateMachine.Instance.IsBuffHash);
        PlayerStateMachine.Instance.BuffDisplayEventChannel.TransferBuff(this);
        PlayerStateMachine.Instance.PlayerBuffSkillEventChannel.RaiseEvent(this);
    }

    public override void UseSkill()
    {
        _remainingTurns--;
        
        if (_remainingTurns < 1)
        {
            PlayerStateMachine.Instance.BuffDisplayEventChannel.RemoveBuff(this);
            PlayerStateMachine.Instance.DeactivateParticles();
            return;
        }
        
        PlayerStateMachine.Instance.PlayerBuffSkillEventChannel.RaiseEvent(this);
        PlayerStateMachine.Instance.BuffDisplayEventChannel.RefreshBuffs();
    }

    private void DecreaseCooldown()
    {
        if(RemainingCooldown < 1) return;
        RemainingCooldown--;
    }
    
    public int RemainingTurns
    {
        get => _remainingTurns;
        set => _remainingTurns = value;
    }
}
