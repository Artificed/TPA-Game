using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class SkillManager : MonoBehaviour
{
    public static SkillManager Instance { get; private set; }
    
    private List<Skill> _skills;

    [SerializeField] private List<SkillDataSO> skillDataSos;
    [SerializeField] private List<SkillContainer> skillContainers;

    [SerializeField] private PlayerBuffSkillEventChannel playerBuffSkillEventChannel;
    [SerializeField] private PlayerActiveSkillEventChannel playerActiveSkillEventChannel;

    [SerializeField] private PlayerTurnEventChannel playerTurnEventChannel;

    [SerializeField] private PlayerLevelEventChannel playerLevelEventChannel;
    
    private void Start()
    {
        _skills = new List<Skill>();
        
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        
        InitializeSkills();
        InitializeSkillBar();
    }

    private void InitializeSkills()
    {
        foreach (var skillDataSo in skillDataSos)
        {
            if (skillDataSo.activeTime == -1)
            {
                Skill skill = new ActiveSkill();
                skill.Initialize(skillDataSo);
                _skills.Add(skill);
            }
            else
            {
                Skill skill = new BuffSkill();
                skill.Initialize(skillDataSo);
                _skills.Add(skill);
            }
        }
    }

    private void InitializeSkillBar()
    {
        foreach (var skill in _skills)
        {
            skillContainers[skill.GetSkillKey - 1].Initialize(skill);
        }
    }

    public Skill GetSkill(string skillName)
    {
        foreach (var skill in _skills)
        {
            if (skill.GetSkillName.Equals(skillName))
            {
                return skill;
            }
        }
        return null;
    }

    private void UpdateActiveSkillUI(ActiveSkill activeSkill)
    {
        skillContainers[activeSkill.GetSkillKey - 1].HandleActiveSkillToggle();
    }

    private void UpdateBuffSkillUI(BuffSkill buffSkill)
    {
        skillContainers[buffSkill.GetSkillKey - 1].HandleBuffSkillUsage();
    }

    public void HandlePlayerTurn()
    {
        foreach (var skill in _skills)
        {
            skill.HandlePlayerTurn();
        }
    }

    private void CheckUnlocked(int level)
    {
        foreach (var skill in _skills)
        {
            skill.Unlocked = level >= skill.GetUnlockLevel;
            skillContainers[skill.GetSkillKey - 1].CheckSkillUnlocked(skill);
        }
    }

    private void OnEnable()
    {
        playerActiveSkillEventChannel.activeSkillEvent.AddListener(UpdateActiveSkillUI);
        playerBuffSkillEventChannel.buffSkillEvent.AddListener(UpdateBuffSkillUI);
        playerTurnEventChannel.playerTurnEvent.AddListener(HandlePlayerTurn);
        
        playerLevelEventChannel.OnLevelChanged += CheckUnlocked;
    }

    private void OnDisable()
    {
        playerActiveSkillEventChannel.activeSkillEvent.RemoveListener(UpdateActiveSkillUI);
        playerBuffSkillEventChannel.buffSkillEvent.RemoveListener(UpdateBuffSkillUI);
        playerTurnEventChannel.playerTurnEvent.RemoveListener(HandlePlayerTurn);
        
        playerLevelEventChannel.OnLevelChanged -= CheckUnlocked;
    }

    public List<Skill> Skills => _skills;
}
