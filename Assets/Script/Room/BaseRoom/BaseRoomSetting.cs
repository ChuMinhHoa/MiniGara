using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class BaseRoomSetting<T>
{
    public RoomType roomType;
    public int roomID;
    public List<ModelPosition<T>> modelPositions;
}
