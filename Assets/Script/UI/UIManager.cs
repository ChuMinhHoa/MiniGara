using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : GenericSingleton<UIManager>
{
    Dictionary<UIPanelType, GameObject> uiPanels = new Dictionary<UIPanelType, GameObject>();
    [SerializeField] Transform canvasMainTransform;
    public void ShowUpgradePanel() { }
    public void RegisterPanel(UIPanelType uiPanelType, GameObject obj) {
        GameObject go = null;
        if (!uiPanels.TryGetValue(uiPanelType, out go))
        {
            Debug.Log("Register panel: " + uiPanelType.ToString());
            uiPanels.Add(uiPanelType, obj);
        }
        obj.SetActive(false);
    }
    public GameObject GetUIPanel(UIPanelType type) {
        GameObject panelReturn = null;
        if (!uiPanels.TryGetValue(type, out panelReturn))
        {
            Resources.LoadAll("UI/");
            switch (type)
            {
                case UIPanelType.UpgradePanel:
                    panelReturn = Instantiate(Resources.FindObjectsOfTypeAll<UIUpgrade>()[0].gameObject, canvasMainTransform);
                    break;
                default:
                    break;
            }
            if (panelReturn) panelReturn.SetActive(true);
            return panelReturn;
        }
        return null;
    }
}
