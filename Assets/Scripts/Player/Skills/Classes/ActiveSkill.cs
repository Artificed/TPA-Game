using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActiveSkill : Skill
{
    private bool isActive;
    
    public override void Initialize(SkillDataSO data)
    {
        SkillName = data.skillName;
        Description = data.description;
        UnlockLevel = data.unlockLevel;
        CooldownTime = data.cooldownTime;
        ImageIcon = data.imageIcon;
        SkillKey = data.skillKey;

        IsUnlocked = Player.Instance.Level >= UnlockLevel;

        isActive = false;
        RemainingCooldown = 0;
        ImmediateAttackBuffer = true;
    }

    public override void HandlePlayerTurn()
    {
        if (ImmediateAttackBuffer)
        {
            ImmediateAttackBuffer = false;
            return;
        }
        if(RemainingCooldown < 1) return;
        RemainingCooldown--;
        PlayerStateMachine.Instance.PlayerActiveSkillEventChannel.RaiseEvent(this);
    }

    public void ToggleActive()
    {
        if(RemainingCooldown > 0) return;
        ImmediateAttackBuffer = true;
        isActive = !isActive;
        PlayerStateMachine.Instance.PlayerActiveSkillEventChannel.RaiseEvent(this);
    }

    public override void UseSkill()
    {
        isActive = false;
        RemainingCooldown = CooldownTime;
        PlayerStateMachine.Instance.PlayerActiveSkillEventChannel.RaiseEvent(this);
    }
    
    public bool IsActive
    {
        get => isActive;
        set => isActive = value;
    }
}
