using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActiveSkill : Skill
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

    public int ActiveTime => _activeTime;

    public void DecreaseActiveTime()
    {
        _activeTime--;
    }

    public void DecreaseTurns()
    {
        _remainingTurns--;
    }
}
