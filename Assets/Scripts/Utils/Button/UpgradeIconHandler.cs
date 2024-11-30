using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class UpgradeIconHandler : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler, IPointerUpHandler
{
    [SerializeField] private Button button;
    [SerializeField] private Image buttonImage;
    [SerializeField] private float transitionDuration = 0.02f;

    [SerializeField] private UpgradeDataSO upgradeDataSo;
    [SerializeField] private TextMeshProUGUI iconText;
    [SerializeField] private Image image;

    [SerializeField] private UpgradeDataEventChannel upgradeDataEventChannel;
    
    private Color normalColor = new Color(1f, 1f, 1f, 0f);  
    private Color highlightedColor = new Color(1f, 1f, 1f, 0.1f); 
    private Color pressedColor = new Color(1f, 1f, 1f, 0.02f);
    
    private Color currentTargetColor; 
    private bool isPointerOver = false;
    
    void Start()
    {
        buttonImage.color = normalColor;
        currentTargetColor = normalColor;

        upgradeDataSo.OnDataChanged += LoadUpgradeData;
        
        LoadUpgradeData();
    }

    void Update()
    {
        buttonImage.color = Color.Lerp(buttonImage.color, currentTargetColor, Time.deltaTime / transitionDuration);
    }
    
    private void LoadUpgradeData()
    {
        image.sprite = Sprite.Create(upgradeDataSo.imageIcon, 
            new Rect(0, 0, upgradeDataSo.imageIcon.width, upgradeDataSo.imageIcon.height), new Vector2(0.5f, 0.5f));
        iconText.text = "Lvl. " + upgradeDataSo.currentLevel + "/" + upgradeDataSo.maxLevel;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        currentTargetColor = pressedColor;
        upgradeDataEventChannel.RaiseEvent(upgradeDataSo);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        currentTargetColor = highlightedColor;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        currentTargetColor = highlightedColor;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        currentTargetColor = normalColor;
    }

    private void OnDestroy()
    {
        upgradeDataSo.OnDataChanged -= LoadUpgradeData;
    }
}