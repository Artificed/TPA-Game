using System.Collections;
using System.Collections.Generic;
using Microsoft.Unity.VisualStudio.Editor;
using UnityEngine;

[CreateAssetMenu(fileName = "SkillData", menuName = "ScriptableObjects/SkillData")]
public class SkillDataSO : ScriptableObject
{
    public string skillName;
    public string description;
    public int unlockLevel;
    public int cooldownTime;
    public int activeTime;
    public Texture2D imageIcon;
    public int skillKey;
}
