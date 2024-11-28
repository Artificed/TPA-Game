using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLifeStealSkillCommand : ICommand
{
    public void Execute()
    {
        BuffSkill lifeStealSkill = (BuffSkill) SkillManager.Instance.GetSkill("Life Steal");
        if (!lifeStealSkill.GetUnlocked) return;
        lifeStealSkill.ToggleSkill();  
    }
}
