using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIUpgrade : UIPanelBase
{
    public UITabUpgrade tabUpgrade;
    public UITabStaff tabStaff;
    public override void Awake()
    {
        uiPanelType = UIPanelType.UpgradePanel;
        base.Awake();
    }
    public void InitData(IRoomControler roomControler) {
        ActivateTabUpgrade(roomControler);
    }
    void ActivateTabUpgrade(IRoomControler roomControler) {
        tabUpgrade.gameObject.SetActive(true);
        tabStaff.gameObject.SetActive(false);
        tabUpgrade.InitData(roomControler);
    }
    void ActivateTabStaff() {
        tabUpgrade.gameObject.SetActive(true);
        tabStaff.gameObject.SetActive(false);
    }
}
