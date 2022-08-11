using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public interface IRoomControler {
    public int GetTotalModel();
    public string GetModelName(int index);
    public Sprite GetSprite(int index);
    public int GetLevelItem(int index);
    public int GetLevelMaxItem(int index);
    public float GetUpgradePriceItem(int index);
    public int GetCurrentMoneyEarn(int index);
    public int GetNextMoneyEarn(int index);
    public int GetCurrentEnery(int index);
    public int GetNextEnergy(int index);
    public void UpgradeItem(int index);
    public Vector3 GetRootPosition(int index);
    public Vector3 GetCamPos(int index);
}
public class BaseRoom<T> : MonoBehaviour, IRoomControler
{
    public BaseRoomSetting<T> roomSetting;
    public RoomDataAsset<T> roomDataAsset;
    public List<WaitingPoint> waitingPoints;
    public virtual WaitingPoint GetWaitingPoint() {
        foreach (WaitingPoint point in waitingPoints)
        {
            if (point.able)
                return point;
        }
        return null;
    }
    public void OnLoadRoom() {
        LoadFromSaveData(ProfileManager.instance.playerData.GetRoomData<T>(roomSetting.roomID, roomSetting.roomType));
    }
    void LoadFromSaveData(BaseRoomSetting<T> saveRoom) {
        for (int i = 0; i < roomSetting.modelPositions.Count; i++)
        {
            ModelPosition<T> model = roomSetting.modelPositions[i];
            if (model.rootObject.childCount > 0)
                Destroy(model.rootObject.GetChild(0).gameObject);
            if (saveRoom != null && saveRoom.modelPositions.Count > i)
                model.level = saveRoom.modelPositions[i].level;
            if (model.level > 0)
            {
                Transform newModelTransform = Instantiate(roomDataAsset.GetModelByType(model.type.ToString(), model.level));
                newModelTransform.SetParent(model.rootObject);
                newModelTransform.SetAsFirstSibling();
                newModelTransform.localPosition = Vector3.zero;
                newModelTransform.localEulerAngles = Vector3.zero;
                newModelTransform.localScale = new Vector3(1, 1, 1);
                model.currentModel = newModelTransform;
            }
        }
    }
    public void LoadAffterUpgrade(Transform modelUPgrade, Transform rootTransform, int modelIndex) {
        if (rootTransform.childCount > 0)
            Destroy(rootTransform.GetChild(0).gameObject);
        Transform newModelTransform = Instantiate(modelUPgrade);
        newModelTransform.SetParent(rootTransform);
        newModelTransform.SetAsFirstSibling();
        newModelTransform.localPosition = Vector3.zero;
        newModelTransform.localEulerAngles = Vector3.zero;
        newModelTransform.localScale = new Vector3(1, 1, 1);
    }
    public int GetTotalModel() {
        return roomSetting.modelPositions.Count;
    }
    public string GetModelName(int index) {
        T type = roomSetting.modelPositions[index].type;
        return roomDataAsset.GetNameByType(type.ToString());
    }
    public Sprite GetSprite(int index) {
        T type = roomSetting.modelPositions[index].type;
        return roomDataAsset.GetSpriteModelByType(type.ToString());
    }
    public int GetLevelItem(int index) {
        return roomSetting.modelPositions[index].level;
    }
    public int GetLevelMaxItem(int index) {
        T type = roomSetting.modelPositions[index].type;
        return roomDataAsset.GetLevelMaxByType(type.ToString());
    }
    public float GetUpgradePriceItem(int index) {
        T type = roomSetting.modelPositions[index].type;
        int level = roomSetting.modelPositions[index].level;
        return roomDataAsset.GetUpgradePriceByType(type.ToString(), level);
    }
    public int GetCurrentMoneyEarn(int index) {
        T type = roomSetting.modelPositions[index].type;
        int level = roomSetting.modelPositions[index].level;
        return roomDataAsset.GetMoneyEarn(type.ToString(), level);
    }
    public int GetNextMoneyEarn(int index) {
        T type = roomSetting.modelPositions[index].type;
        int level = roomSetting.modelPositions[index].level;
        return roomDataAsset.GetMoneyEarn(type.ToString(), level+1);
    }
    public int GetCurrentEnery(int index) {
        T type = roomSetting.modelPositions[index].type;
        int level = roomSetting.modelPositions[index].level;
        return roomDataAsset.GetEnergy(type.ToString(), level);
    }
    public int GetNextEnergy(int index) {
        T type = roomSetting.modelPositions[index].type;
        int level = roomSetting.modelPositions[index].level;
        return roomDataAsset.GetEnergy(type.ToString(), level + 1);
    }
    public Vector3 GetRootPosition(int index) {
        return roomSetting.modelPositions[index].rootObject.position;
    }
    public Vector3 GetCamPos(int index) {
        return roomSetting.modelPositions[index].camPos.position;
    }
    public void UpgradeItem(int index) {
        int level = 0;
        Debug.Log("Item ID: "+index);
        roomSetting.modelPositions[index].level++;
        level = roomSetting.modelPositions[index].level;
        roomSetting.modelPositions[index].currentModel = roomDataAsset.GetModelByType(roomSetting.modelPositions[index].type.ToString(), level);
        ProfileManager.instance.playerData.SaveRoomData<T>(roomSetting);
        Transform modelUpgrade = roomSetting.modelPositions[index].currentModel;
        Transform rootTrs = roomSetting.modelPositions[index].rootObject;
        LoadAffterUpgrade(modelUpgrade, rootTrs, index);
    }
}
