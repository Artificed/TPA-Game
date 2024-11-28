using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Skill
{
    protected string SkillName;
    protected string Description;
    protected int UnlockLevel;
    protected int CooldownTime;
    protected Texture2D ImageIcon;

    protected bool IsUnlocked;
    
    protected int RemainingCooldown;
    protected SkillDataSO SkillDataSo;
    protected int SkillKey;

    protected bool ImmediateAttackBuffer;
    
    public abstract void Initialize(SkillDataSO data);
    public abstract void HandlePlayerTurn();
    public abstract void UseSkill();

    public Texture2D GetImageIcon => ImageIcon;
    public int GetSkillKey => SkillKey;
    public bool GetUnlocked => IsUnlocked;
    public string GetDescription => Description;
    public int GetUnlockLevel => UnlockLevel;
    public string GetSkillName => SkillName;
    public int GetRemainingCooldown => RemainingCooldown;
}
