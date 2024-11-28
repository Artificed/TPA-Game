using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BuffContainer: MonoBehaviour
{
    [SerializeField] private Image buffImage;
    [SerializeField] private TextMeshProUGUI buffActiveTime;

    private BuffSkill _buffSkill;
    public BuffSkill BuffSkill => _buffSkill;
    
    public void Initialize(BuffSkill skill)
    {
        _buffSkill = skill;

        buffImage.sprite = Sprite.Create(skill.GetImageIcon, 
            new Rect(0, 0, skill.GetImageIcon.width, skill.GetImageIcon.height), 
            new Vector2(0.5f, 0.5f));

        buffActiveTime.text = skill.RemainingTurns.ToString();
    }
}
