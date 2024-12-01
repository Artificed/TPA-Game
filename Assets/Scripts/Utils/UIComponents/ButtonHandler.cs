using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class ButtonHandler : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    [SerializeField] protected Button button;
    [SerializeField] protected Sprite buttonNormal; 
    [SerializeField] protected Sprite buttonHovered;
    
    [SerializeField] protected AudioClip buttonHoverSound;
    [SerializeField] protected AudioClip normalClickSound;
    [SerializeField] protected AudioClip buttonPurchaseSound;

    [SerializeField] protected AudioClip currentClickSound;
    
    [SerializeField] protected AudioSource audioSource;

    private void Start()
    {
        button = GetComponent<Button>();
        audioSource = GetComponent<AudioSource>();
        currentClickSound = normalClickSound;
    }

    public virtual void OnPointerEnter(PointerEventData eventData)
    {
        button.image.sprite = buttonHovered;
        audioSource.PlayOneShot(buttonHoverSound);
    }

    public virtual void OnPointerExit(PointerEventData eventData)
    {
        button.image.sprite = buttonNormal;
    }

    public virtual void OnPointerClick(PointerEventData eventData)
    {
        audioSource.PlayOneShot(currentClickSound);
        currentClickSound = normalClickSound;
    }

    public void UsePurchaseSound()
    {
        currentClickSound = buttonPurchaseSound;
    }
    
    
}