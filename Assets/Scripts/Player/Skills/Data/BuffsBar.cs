using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BuffsBar : MonoBehaviour
{
    [SerializeField] private GameObject container;
    [SerializeField] private GameObject buffPrefab;
    [SerializeField] private BuffDisplayEventChannel buffDisplayEventChannel;
    
    private List<BuffContainer> _buffContainers = new List<BuffContainer>();
    private List<BuffSkill> _buffSkills = new List<BuffSkill>();

    private void RepositionContainer()
    {
        float totalWidth = 0f;
        foreach (BuffContainer buffContainer in _buffContainers)
        {
            RectTransform rectTransform = buffContainer.GetComponent<RectTransform>();
            totalWidth += rectTransform.rect.width;
        }
        
        float startX = -totalWidth / 2 + 25;
        
        for (int i = 0; i < _buffContainers.Count; i++)
        {
            RectTransform rectTransform = _buffContainers[i].GetComponent<RectTransform>();
            rectTransform.anchoredPosition = new Vector2(startX + (rectTransform.rect.width * i), 0);
        }
    }
    
    private void RefreshContainer()
    {
        foreach (BuffContainer buffContainer in _buffContainers)
        {
            Destroy(buffContainer.gameObject);
        }

        _buffContainers.Clear();
        _buffSkills.Clear();

        List<BuffSkill> activeBuffs = SkillManager.Instance.Skills
            .Where(skill => skill is BuffSkill && ((BuffSkill)skill).RemainingTurns > 0)
            .Cast<BuffSkill>()
            .ToList();
        
        foreach (BuffSkill buffSkill in activeBuffs)
        {
            GameObject buffObject = Instantiate(buffPrefab, container.transform);
        
            BuffContainer buffContainer = buffObject.GetComponent<BuffContainer>();
            buffContainer.Initialize(buffSkill);

            _buffContainers.Add(buffContainer);
            _buffSkills.Add(buffSkill);
        }

        RepositionContainer();
    }

    private void OnEnable()
    {
        buffDisplayEventChannel.OnBuffRefreshed += RefreshContainer;
    }

    private void OnDisable()
    {
        buffDisplayEventChannel.OnBuffRefreshed -= RefreshContainer;
    }
}
