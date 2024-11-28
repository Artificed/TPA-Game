using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuffsBar : MonoBehaviour
{
    [SerializeField] private GameObject container;
    [SerializeField] private GameObject buffPrefab;
        
    private List<BuffContainer> _buffContainers = new List<BuffContainer>();

    [SerializeField] private BuffDisplayEventChannel buffDisplayEventChannel;
    
    private void RefreshContainer()
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
    
    private void AddBuff(BuffSkill buffSkill)
    {
        GameObject buffObject = Instantiate(buffPrefab, container.transform);

        BuffContainer buffContainer = buffObject.GetComponent<BuffContainer>();
        buffContainer.Initialize(buffSkill);

        _buffContainers.Add(buffContainer);

        RefreshContainer();
    }

    private void RemoveBuff(BuffSkill buffSkill)
    {
        BuffContainer buffToRemove = null;

        foreach (BuffContainer buffContainer in _buffContainers)
        {
            if (buffSkill == buffContainer.BuffSkill)
            {
                buffToRemove = buffContainer;
                break;
            }
        }

        if (buffToRemove != null)
        {
            _buffContainers.Remove(buffToRemove);
            Destroy(buffToRemove);
        }

        RefreshContainer();
    }
    
    private void OnEnable()
    {
        buffDisplayEventChannel.OnBuffLoaded += AddBuff;
        buffDisplayEventChannel.OnBuffLoaded += RemoveBuff;
        buffDisplayEventChannel.OnBuffRefreshed += RefreshContainer;
    }

    private void OnDisable()
    {
        buffDisplayEventChannel.OnBuffLoaded -= AddBuff;
        buffDisplayEventChannel.OnBuffLoaded -= RemoveBuff;
        buffDisplayEventChannel.OnBuffRefreshed -= RefreshContainer;
    }
}
