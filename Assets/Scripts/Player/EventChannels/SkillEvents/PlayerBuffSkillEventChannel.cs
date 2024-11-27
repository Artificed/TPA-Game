using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(fileName = "PlayerBuffSkillEventChannel", menuName = "EventChannels/PlayerBuffSkillEventChannel")]
public class PlayerBuffSkillEventChannel : ScriptableObject
{
    public UnityEvent<BuffSkill> buffSkillEvent;

    public void RaiseEvent(BuffSkill buffSkill)
    {
        buffSkillEvent?.Invoke(buffSkill);
    }
}
