using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLifeStealSkillCommand : ICommand
{
    public void Execute()
    {
        Skill skill = SkillManager.Instance.GetSkill("Life Steal");
        
    }
}
