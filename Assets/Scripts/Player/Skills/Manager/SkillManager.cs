using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillManager : MonoBehaviour
{
    public static SkillManager Instance { get; private set; }
    
    private List<Skill> _skills;
    private List<Skill> _activeSkills;

    [SerializeField] private List<SkillDataSO> skillDataSos;
    [SerializeField] private List<SkillContainer> skillContainers;

    private void Awake()
    {
        _skills = new List<Skill>();
        _activeSkills = new List<Skill>();
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
                Skill skill = new BuffSkill();
                skill.Initialize(skillDataSo);
                _skills.Add(skill);
            }
            else
            {
                Skill skill = new ActiveSkill();
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

    // Update is called once per frame
    void Update()
    {
        
    }
}
