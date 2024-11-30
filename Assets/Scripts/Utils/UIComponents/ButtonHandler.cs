using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class ButtonHandler : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    [SerializeField] private Button button;
    [SerializeField] private Sprite buttonNormal; 
    [SerializeField] private Sprite buttonHovered;
    
    [SerializeField] private AudioClip buttonHoverSound;
    [SerializeField] private AudioClip normalClickSound;
    [SerializeField] private AudioClip buttonPurchaseSound;

    [SerializeField] private AudioClip currentClickSound;
    
    [SerializeField] private AudioSource audioSource;

    private void Start()
    {
        button = GetComponent<Button>();
        audioSource = GetComponent<AudioSource>();
        currentClickSound = normalClickSound;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        button.image.sprite = buttonHovered;
        audioSource.PlayOneShot(buttonHoverSound);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        button.image.sprite = buttonNormal;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        audioSource.PlayOneShot(currentClickSound);
        currentClickSound = normalClickSound;
    }

    public void UsePurchaseSound()
    {
        currentClickSound = buttonPurchaseSound;
    }
}