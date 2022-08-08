using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIPanelBase : MonoBehaviour
{
    public bool isRegisterOnUI = true;
    protected UIPanelType uiPanelType;
    public virtual void Awake() {
        if (isRegisterOnUI) UIManager.instance.RegisterPanel(uiPanelType, gameObject);
    }
}
