using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SkillContainer : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private Image skillDisplay;
    [SerializeField] private TextMeshProUGUI skillKey;
    [SerializeField] private Image lockedDisplay;
    [SerializeField] private TextMeshProUGUI lockedText;
    [SerializeField] private TextMeshProUGUI descriptionText;
    [SerializeField] private GameObject descriptionContainer;

    [SerializeField] private BuffDisplayEventChannel buffDisplayEventChannel;
    
    private Color _darkOverlay = new Color(0f, 0f, 0f, 0.8f);
    private Color _orangeOverlay = new Color(1f, 0.6f, 0f, 0.4f);
    private Skill _skill;

    public void Initialize(Skill skill)
    {
        _skill = skill;

        skillDisplay.sprite = Sprite.Create(_skill.GetImageIcon, 
            new Rect(0, 0, _skill.GetImageIcon.width, _skill.GetImageIcon.height), 
            new Vector2(0.5f, 0.5f));
        
        if(!skill.GetUnlocked)
        {
            lockedText.text = "Locked";
            lockedDisplay.gameObject.SetActive(true);
        }
        
        skillKey.text = _skill.GetSkillKey.ToString();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if(_skill == null) return;
        
        descriptionContainer.SetActive(true);
        if (_skill.GetUnlocked)
        {
            descriptionText.text = _skill.GetDescription;
        }
        else
        {
            descriptionText.text = "Unlocked at level " + _skill.GetUnlockLevel;
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if(_skill == null) return;
        descriptionContainer.SetActive(false);
    }

    public void HandleActiveSkillToggle()
    {
        if (((ActiveSkill)_skill).IsActive)
        {
            lockedText.text = "";
            lockedDisplay.color = _orangeOverlay;
            lockedDisplay.gameObject.SetActive(true);
            return;
        }

        if (_skill.GetRemainingCooldown > 0)
        {
            lockedText.text = _skill.GetRemainingCooldown.ToString();
            lockedDisplay.color = _darkOverlay;
            lockedText.fontSize = 20;
            lockedDisplay.gameObject.SetActive(true);
            return;
        }
        
        lockedDisplay.gameObject.SetActive(false);
    }

    public void HandleBuffSkillUsage()
    {
        if (_skill.GetRemainingCooldown > 0)
        {
            buffDisplayEventChannel.OnBuffLoaded((BuffSkill) _skill);
            lockedText.text = _skill.GetRemainingCooldown.ToString();
            lockedDisplay.color = _darkOverlay;
            lockedText.fontSize = 20;
            lockedDisplay.gameObject.SetActive(true);
            return;
        }
        
        lockedDisplay.gameObject.SetActive(false);
    }
}
