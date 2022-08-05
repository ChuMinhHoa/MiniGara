using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseRoom<T> : MonoBehaviour
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
                newModelTransform.localPosition = Vector3.zero;
                newModelTransform.localEulerAngles = Vector3.zero;
                newModelTransform.localScale = new Vector3(1, 1, 1);
                model.currentModel = newModelTransform;
            }
        }
    }
}
