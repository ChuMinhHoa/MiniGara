using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class UIUpgradeSlot : MonoBehaviour, IPointerClickHandler, IPointerExitHandler
{
    [SerializeField] Image icon;
    public Image backGround;
    public UITabUpgrade uiTabUpgrade;
    public UnityEvent onSelect, onDeselect;
    public void InitData(Sprite iconSpr) {
        icon.sprite = iconSpr;
        if (transform.GetSiblingIndex() == 0)
            backGround.sprite = uiTabUpgrade.backGroundEnter;
        else backGround.sprite = uiTabUpgrade.backGroundNormal;
    }
    public void OnPointerClick(PointerEventData eventData)
    {
        uiTabUpgrade.OnSelect(transform.GetSiblingIndex());
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        uiTabUpgrade.OnDeselect();
    }
    public void Select()
    {
        if (onSelect != null)
            onSelect.Invoke();
    }
    public void DeSelect()
    {
        if (onDeselect != null)
            onDeselect.Invoke();
    }
}
