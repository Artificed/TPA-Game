using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

[CreateAssetMenu(menuName = "EventChannels/BuffDisplayEventChannel", fileName = "BuffDisplayEventChannel")]
public class BuffDisplayEventChannel : ScriptableObject
{
    public UnityAction<BuffSkill> OnBuffLoaded;
    public UnityAction<BuffSkill> OnBuffRemoved;
    public UnityAction OnBuffRefreshed;

    public void TransferBuff(BuffSkill buffSkill)
    {
        OnBuffLoaded?.Invoke(buffSkill);
    }

    public void RemoveBuff(BuffSkill buffSkill)
    {
        OnBuffRemoved?.Invoke(buffSkill);
    }

    public void RefreshBuffs()
    {
        OnBuffRefreshed?.Invoke();
    }
}
