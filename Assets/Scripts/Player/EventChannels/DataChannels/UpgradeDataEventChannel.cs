using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(fileName = "UpgradeDataEventChannel", menuName = "EventChannels/UpgradeDataEventChannel")]
public class UpgradeDataEventChannel : ScriptableObject
{
    public UnityEvent<UpgradeDataSO> onIconSelected;

    public void RaiseEvent(UpgradeDataSO data)
    {
        onIconSelected.Invoke(data);
    }
}
