using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLifeStealSkillCommand : ICommand
{
    public void Execute()
    {
        BuffSkill lifeStealSkill = (BuffSkill)SkillManager.Instance.GetSkill("Life Steal");

        if (Player.Instance.Level < lifeStealSkill.GetUnlockLevel) return;

        bool playSoundClip = CheckPlaySoundClip(lifeStealSkill);

        lifeStealSkill.ToggleSkill();

        if(playSoundClip)
        {
            SoundFXManager.Instance.PlayLifeStealClip(Player.Instance.transform);
        }
    }

    private bool CheckPlaySoundClip(BuffSkill lifeStealSkill)
    {
        return lifeStealSkill.GetRemainingCooldown < 1;
    }

}
