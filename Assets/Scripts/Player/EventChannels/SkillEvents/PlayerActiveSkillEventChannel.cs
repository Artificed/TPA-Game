using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(fileName = "PlayerActiveSkillEventChannel", menuName = "EventChannels/PlayerActiveSkillEventChannel")]
public class PlayerActiveSkillEventChannel : ScriptableObject
{
    public UnityEvent<ActiveSkill> activeSkillEvent;

    public void RaiseEvent(ActiveSkill activeSkill)
    {
        activeSkillEvent?.Invoke(activeSkill);
    }
}
