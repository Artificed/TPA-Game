using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(menuName = "EventChannels/PlayerHealthEventChannel", fileName = "PlayerHealthEventChannel")]
public class PlayerHealthEventChannel : ScriptableObject
{
    public UnityAction<int, int> OnHealthChanged;

    public void RaiseEvent(int currentHealth, int maxHealth)
    {
        OnHealthChanged.Invoke(currentHealth, maxHealth);
    }
}
