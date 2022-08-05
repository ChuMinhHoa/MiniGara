using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
[Serializable]
public class StaffData { 

}
[Serializable]
public class RoomSaveData {
    [Header("ROOM MODEL DATA")]
    public List<BaseRoomSetting<FixRoomModelType>> fixRoomModelDatas = new List<BaseRoomSetting<FixRoomModelType>>();
    public List<BaseRoomSetting<CarryRoomModelType>> carryRoomModelDatas = new List<BaseRoomSetting<CarryRoomModelType>>();
    public List<BaseRoomSetting<LandingPadModelType>> landingPadModelDatas = new List<BaseRoomSetting<LandingPadModelType>>();
    public List<BaseRoomSetting<HouseModelType>> houseModelDatas = new List<BaseRoomSetting<HouseModelType>>();
    public List<BaseRoomSetting<TakeOffModelType>> takeOffModelDatas = new List<BaseRoomSetting<TakeOffModelType>>();

    const string FixRoomModelType = "FixRoomModelType";
    const string CarryRoomModelType = "CarryRoomModelType";
    const string LandingPadModelType = "LandingPadModelType";
    const string HouseModelType = "HouseModelType";
    const string TakeOffModelType = "TakeOffModelType";

    [Header("ROOM ID")]
    public List<int> roomCarryID;
    public List<int> roomLandingID;
    public List<int> roomTakeOffID;
    public List<int> roomFixID;
    public List<int> roomHouseID;
    int totalRooms;
    public int GetCarryRoomID(int index) { return roomCarryID[index]; }
    public int GetLandingRoomID(int index) { return roomLandingID[index]; }
    public int GetTakeOffRoomID(int index) { return roomTakeOffID[index]; }
    public int GetFixRoomID(int index) { return roomFixID[index]; }
    public int GetHouseRoomID(int index) { return roomHouseID[index]; }
    public void AddCarryRoomID(int value) { roomCarryID.Add(value); }
    public void AddLandingRoomID(int value) { roomLandingID.Add(value); }
    public void AddTakeOffRoomID(int value) { roomTakeOffID.Add(value); }
    public void AddFixRoomID(int value) { roomFixID.Add(value); }
    public void AddHouseRoomID(int value) { roomHouseID.Add(value); }
    public void AddTotalRooms() { totalRooms++; }
    public int GetTotalRooms() { return totalRooms; }
    public BaseRoomSetting<T> GetRoomSettingData<T>(int roomID, RoomType roomType) {
        switch (roomType)
        {
            case RoomType.LandingRoom:
                for (int i = 0; i < landingPadModelDatas.Count; i++)
                {
                    if (landingPadModelDatas[i].roomID == roomID)
                        return landingPadModelDatas[i] as BaseRoomSetting<T>;
                }
                break;
            case RoomType.FixRoom:
                for (int i = 0; i < fixRoomModelDatas.Count; i++)
                {
                    if (fixRoomModelDatas[i].roomID == roomID)
                        return fixRoomModelDatas[i] as BaseRoomSetting<T>;
                }
                break;
            case RoomType.CarryRoom:
                for (int i = 0; i < carryRoomModelDatas.Count; i++)
                {
                    if (carryRoomModelDatas[i].roomID == roomID)
                        return carryRoomModelDatas[i] as BaseRoomSetting<T>;
                }
                break;
            case RoomType.House:
                for (int i = 0; i < houseModelDatas.Count; i++)
                {
                    if (houseModelDatas[i].roomID == roomID)
                        return houseModelDatas[i] as BaseRoomSetting<T>;
                }
                break;
            case RoomType.TakeOffRoom:
                for (int i = 0; i < takeOffModelDatas.Count; i++)
                {
                    if (takeOffModelDatas[i].roomID == roomID)
                        return takeOffModelDatas[i] as BaseRoomSetting<T>;
                }
                break;
            default:
                break;
        }
        return null;
    }
    public void SaveRoomSettingData<T>(BaseRoomSetting<T> room, bool isOverride = true) {
        bool isHasData = false;
        switch (typeof(T).ToString())
        {
            case FixRoomModelType:
                for (int i = 0; i < fixRoomModelDatas.Count; i++)
                {
                    if (fixRoomModelDatas[i].roomID == room.roomID)
                    {
                        if (isOverride) fixRoomModelDatas[i] = room as BaseRoomSetting<FixRoomModelType>;
                        isHasData = true;
                    }
                }
                if (!isHasData) fixRoomModelDatas.Add(room as BaseRoomSetting<FixRoomModelType>);
                break;
            case CarryRoomModelType:
                for (int i = 0; i < carryRoomModelDatas.Count; i++)
                {
                    if (carryRoomModelDatas[i].roomID == room.roomID)
                    {
                        if (isOverride) carryRoomModelDatas[i] = room as BaseRoomSetting<CarryRoomModelType>;
                        isHasData = true;
                    }
                }
                if (!isHasData) carryRoomModelDatas.Add(room as BaseRoomSetting<CarryRoomModelType>);
                break;
            case LandingPadModelType:
                for (int i = 0; i < landingPadModelDatas.Count; i++)
                {
                    if (landingPadModelDatas[i].roomID == room.roomID)
                    {
                        if (isOverride) landingPadModelDatas[i] = room as BaseRoomSetting<LandingPadModelType>;
                        isHasData = true;
                    }
                }
                if (!isHasData) landingPadModelDatas.Add(room as BaseRoomSetting<LandingPadModelType>);
                break;
            case HouseModelType:
                for (int i = 0; i < houseModelDatas.Count; i++)
                {
                    if (houseModelDatas[i].roomID == room.roomID)
                    {
                        if (isOverride) houseModelDatas[i] = room as BaseRoomSetting<HouseModelType>;
                        isHasData = true;
                    }
                }
                if (!isHasData) houseModelDatas.Add(room as BaseRoomSetting<HouseModelType>);
                break;
            case TakeOffModelType:
                for (int i = 0; i < takeOffModelDatas.Count; i++)
                {
                    if (takeOffModelDatas[i].roomID == room.roomID)
                    {
                        if (isOverride) takeOffModelDatas[i] = room as BaseRoomSetting<TakeOffModelType>;
                        isHasData = true;
                    }
                }
                if (!isHasData) takeOffModelDatas.Add(room as BaseRoomSetting<TakeOffModelType>);
                break;
            default:
                break;
        }
    }
}
[Serializable]
public class PlayerData
{
    public RoomSaveData roomSaveData;
    public void InitData() {
        Debug.Log("Init Data!");

    }
    public void LoadData() {
        string jsonData = GetData();
        if (!string.IsNullOrEmpty(jsonData))
        {
            PlayerData dataSave = JsonUtility.FromJson<PlayerData>(jsonData);
            roomSaveData = dataSave.roomSaveData;
            Debug.Log("Load Data");
        }
        else {
            SaveData();
        }
    }
    public void SaveRoomData<T>(BaseRoomSetting<T> room, bool isOverride = true) {
        roomSaveData.SaveRoomSettingData(room, isOverride);
    }
    public BaseRoomSetting<T> GetRoomData<T>(int roomID, RoomType roomType) {
        return roomSaveData.GetRoomSettingData<T>(roomID, roomType);
    }
    public int GetRoomID(int index, RoomType roomType) {
        switch (roomType)
        {
            case RoomType.LandingRoom:
                if (index < roomSaveData.roomCarryID.Count) return roomSaveData.GetLandingRoomID(index);
                else return -1;
            case RoomType.FixRoom:
                if (index < roomSaveData.roomFixID.Count) return roomSaveData.GetFixRoomID(index);
                else return -1;
            case RoomType.CarryRoom:
                if (index < roomSaveData.roomCarryID.Count) return roomSaveData.GetCarryRoomID(index);
                else return -1;
            case RoomType.House:
                if (index < roomSaveData.roomHouseID.Count) return roomSaveData.GetHouseRoomID(index);
                else return -1;
            case RoomType.TakeOffRoom:
                if (index < roomSaveData.roomTakeOffID.Count) return roomSaveData.GetTakeOffRoomID(index);
                else return -1;
            default:
                break;
        }
        return -1;
    }
    public void AddRoomID(RoomType roomType) {
        switch (roomType)
        {
            case RoomType.LandingRoom:
                roomSaveData.AddLandingRoomID(roomSaveData.GetTotalRooms());
                break;
            case RoomType.FixRoom:
                roomSaveData.AddFixRoomID(roomSaveData.GetTotalRooms());
                break;
            case RoomType.CarryRoom:
                roomSaveData.AddCarryRoomID(roomSaveData.GetTotalRooms());
                break;
            case RoomType.House:
                roomSaveData.AddHouseRoomID(roomSaveData.GetTotalRooms());
                break;
            case RoomType.TakeOffRoom:
                roomSaveData.AddTakeOffRoomID(roomSaveData.GetTotalRooms());
                break;
            default:
                break;
        }
        roomSaveData.AddTotalRooms();
    }
    public void SaveData() {
        Debug.Log("Save Data");
        PlayerPrefs.SetString("PlayerData", JsonUtility.ToJson(this).ToString());
    }
    public string GetData() { return PlayerPrefs.GetString("PlayerData"); }
}
