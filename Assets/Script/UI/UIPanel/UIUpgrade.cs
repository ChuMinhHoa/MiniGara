using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIUpgrade : UIPanelBase
{
    public override void Awake()
    {
        uiPanelType = UIPanelType.UpgradePanel;
        base.Awake();
    }
}
