using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SkillContainer : MonoBehaviour
{
    [SerializeField] private Image skillDisplay;
    [SerializeField] private TextMeshProUGUI skillKey;

    private Skill _skill;

    public void Initialize(Skill skill)
    {
        _skill = skill;

        skillDisplay.sprite = Sprite.Create(_skill.GetImageIcon, 
            new Rect(0, 0, _skill.GetImageIcon.width, _skill.GetImageIcon.height), 
            new Vector2(0.5f, 0.5f));
        
        skillKey.text = _skill.GetSkillKey.ToString();
    }
    
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
