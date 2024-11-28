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
        DecreaseActiveTime();
        PlayerStateMachine.Instance.PlayerBuffSkillEventChannel.RaiseEvent(this);
    }

    public void ToggleSkill()
    {
        if(RemainingCooldown > 0) return;
        
        _remainingTurns = _activeTime;
        RemainingCooldown = CooldownTime;
        
        PlayerStateMachine.Instance.Animator.SetTrigger(PlayerStateMachine.Instance.IsBuffHash);
        PlayerStateMachine.Instance.PlayerBuffSkillEventChannel.RaiseEvent(this);
        PlayerStateMachine.Instance.BuffDisplayEventChannel.TransferBuff(this);
    }

    public override void UseSkill()
    {
        if(_activeTime < 1) return;
        _activeTime--;
        PlayerStateMachine.Instance.PlayerBuffSkillEventChannel.RaiseEvent(this);
    }

    private void DecreaseCooldown()
    {
        if(RemainingCooldown < 1) return;
        RemainingCooldown--;
    }

    private void DecreaseActiveTime()
    {
        if(_activeTime < 1) return;
        _activeTime--;
    }

    public int RemainingTurns
    {
        get => _remainingTurns;
        set => _remainingTurns = value;
    }
}
