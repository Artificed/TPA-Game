using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillManager : MonoBehaviour
{
    public static SkillManager Instance { get; private set; }
    
    private List<Skill> _skills;
    private List<Skill> _usableSkills;

    [SerializeField] private List<SkillDataSO> skillDataSos;
    [SerializeField] private List<SkillContainer> skillContainers;

    [SerializeField] private PlayerBuffSkillEventChannel playerBuffSkillEventChannel;
    [SerializeField] private PlayerActiveSkillEventChannel playerActiveSkillEventChannel;
    private void Awake()
    {
        _skills = new List<Skill>();
        _usableSkills = new List<Skill>();
    }

    private void Start()
    {
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
        Debug.Log("test");
        skillContainers[activeSkill.GetSkillKey - 1].HandleActiveSkillToggle(activeSkill.IsActive);
    }

    private void UpdateBuffSkillUI(BuffSkill buffSkill)
    {
        
    }

    private void OnEnable()
    {
        playerActiveSkillEventChannel.activeSkillEvent.AddListener(UpdateActiveSkillUI);
        playerBuffSkillEventChannel.buffSkillEvent.AddListener(UpdateBuffSkillUI);
    }

    private void OnDisable()
    {
        playerActiveSkillEventChannel.activeSkillEvent.RemoveListener(UpdateActiveSkillUI);
        playerBuffSkillEventChannel.buffSkillEvent.RemoveListener(UpdateBuffSkillUI);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
