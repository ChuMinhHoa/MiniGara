using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class BaseRoomSetting<T>
{
    public RoomType roomType;
    public int roomID;
    public List<ModelPosition<T>> modelPositions;
    public int GetLevelOfModelPosition(string modelType) {
        for (int i = 0; i < modelPositions.Count; i++)
        {
            if (modelPositions[i].type.ToString() == modelType)
                return modelPositions[i].level;
        }
        return -1;
    }
}
