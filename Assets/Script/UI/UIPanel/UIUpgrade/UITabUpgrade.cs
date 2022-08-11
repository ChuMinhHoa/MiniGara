using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UITabUpgrade : MonoBehaviour
{
    [SerializeField] Transform prefabItemUI;
    [SerializeField] Transform rootTransform;
    [SerializeField] Text textTitle;
    [SerializeField] Text textLevel;
    [SerializeField] Text textPriceUpgrade;
    [SerializeField] Image iconItem;
    [SerializeField] Button btnUpgrade;
    [SerializeField] Button btnClose;
    [SerializeField] GameObject maxUpgrade;
    [SerializeField] UITabProfit uiTabProfit;
    [SerializeField] RectTransform rectTransform;
    public List<UIUpgradeSlot> slotsUpgrade;
    public int selectedItemID = 0;
    IRoomControler currentRoom;
    bool rebuildLayout;
    public Sprite backGroundNormal, backGroundEnter;
    private void Start()
    {
        btnClose.onClick.AddListener(CloseUpgradePanel);
        btnUpgrade.onClick.AddListener(OnUpgradeItem);
    }
    public void InitData(IRoomControler currentRoomValue) {
        currentRoom = currentRoomValue;
        int totalModel = currentRoomValue.GetTotalModel();
        while (rootTransform.childCount < totalModel) {
            Instantiate(prefabItemUI, rootTransform);
        }
        slotsUpgrade.Clear();
        for (int i = 0; i < rootTransform.childCount; i++)
        {
            Transform t = rootTransform.GetChild(i);
            if (i < totalModel)
            {
                t.gameObject.SetActive(true);
                UIUpgradeSlot slot = t.GetComponent<UIUpgradeSlot>();
                slot.uiTabUpgrade = this;
                slot.InitData(currentRoom.GetSprite(i));
                slotsUpgrade.Add(slot);
            }
            else t.gameObject.SetActive(false);
        }
        OnSelect(0);
        SetUpInfor();
    }
    public void OnSelect(int index) {
        if (index == selectedItemID)
            return;
        selectedItemID = index;
        ResetSlot();
        SetUpInfor();
        slotsUpgrade[selectedItemID].backGround.sprite = backGroundEnter;
    }
    private void Update()
    {
        if (rebuildLayout)
            LayoutRebuilder.MarkLayoutForRebuild(rectTransform);
        btnUpgrade.interactable = currentRoom.GetUpgradePriceItem(selectedItemID) <= ProfileManager.instance.playerData.money;
    }
    void SetUpInfor() {
        textTitle.text = currentRoom.GetModelName(selectedItemID);
        iconItem.sprite = currentRoom.GetSprite(selectedItemID);
        int level = currentRoom.GetLevelItem(selectedItemID);
        int levelMax = currentRoom.GetLevelMaxItem(selectedItemID);
        btnUpgrade.gameObject.SetActive(level != levelMax);
        maxUpgrade.gameObject.SetActive(level == levelMax);
        textLevel.text = "Level: " + level.ToString();
        float price = currentRoom.GetUpgradePriceItem(selectedItemID);
        textPriceUpgrade.text = price.ToString("0.00");
        uiTabProfit.InitData(currentRoom, selectedItemID);
        Vector3 targetPos = currentRoom.GetCamPos(selectedItemID);
        Vector3 targetRotage = currentRoom.GetRootPosition(selectedItemID);
        GameManager.instance.cameraManager.ChangeTarget(targetPos, targetRotage);
    }
    public void OnDeselect() {
        ResetSlot();
    }
    void ResetSlot() {
        for (int i = 0; i < slotsUpgrade.Count; i++)
        {
            if (selectedItemID == i)
                continue;
            slotsUpgrade[i].DeSelect();
            slotsUpgrade[i].backGround.sprite = backGroundNormal;
        }
    }
    void OnUpgradeItem() {
        ProfileManager.instance.playerData.ConbineMoney((int)currentRoom.GetUpgradePriceItem(selectedItemID));
        currentRoom.UpgradeItem(selectedItemID);
        SetUpInfor();
    }
    void CloseUpgradePanel() {
        UIManager.instance.CloseUpgradePanel();
        GameManager.instance.cameraManager.BackToDefault();
    }
}
