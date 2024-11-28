using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBashSkillCommand : ICommand
{
    public void Execute()
    {
        ActiveSkill bashSkill = (ActiveSkill) SkillManager.Instance.GetSkill("Bash");
        if (Player.Instance.Level < bashSkill.GetUnlockLevel) return;
        bashSkill.ToggleActive();
    }
}
