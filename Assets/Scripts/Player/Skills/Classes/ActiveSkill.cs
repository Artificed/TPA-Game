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
    }

    public bool IsActive
    {
        get => isActive;
        set => isActive = value;
    }

    public void ToggleActive()
    {
        isActive = !isActive;
        PlayerStateMachine.Instance.PlayerActiveSkillEventChannel.RaiseEvent(this);
    }
}
