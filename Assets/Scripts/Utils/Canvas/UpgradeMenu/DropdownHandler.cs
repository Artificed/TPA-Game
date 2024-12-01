using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DropdownHandler : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    [SerializeField] private PlayerDataSO playerDataSo;
    [SerializeField] private TMP_Dropdown dropdown;
    
    [SerializeField] private Sprite dropdownNormal; 
    [SerializeField] private Sprite dropdownHovered;
    
    [SerializeField] private AudioClip buttonHoverSound;
    [SerializeField] private AudioClip normalClickSound;
    
    [SerializeField] private AudioSource audioSource;

    [SerializeField] private PlayerFloorCountEventChannel playerFloorCountEventChannel;
    
    void Start()
    {
        UpdateDropdownOptions();
    }
    
    public void UpdateDropdownOptions()
    {
        dropdown.options.Clear();
        dropdown.options.Add(new TMP_Dropdown.OptionData("Boss"));
        
        for (int i = 1; i <= playerDataSo.floor; i++)
        {
            dropdown.options.Add(new TMP_Dropdown.OptionData("Floor " + i));
        }

        dropdown.value = 1;
    }


    public void OnPointerEnter(PointerEventData eventData)
    {
        dropdown.image.sprite = dropdownHovered;
        audioSource.PlayOneShot(buttonHoverSound);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        dropdown.image.sprite = dropdownNormal;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        audioSource.PlayOneShot(normalClickSound);
    }

    private void OnEnable()
    {
        playerFloorCountEventChannel.FloorChangedEvent += UpdateDropdownOptions;
    }

    private void OnDisable()
    {
        playerFloorCountEventChannel.FloorChangedEvent -= UpdateDropdownOptions;
    }
}
