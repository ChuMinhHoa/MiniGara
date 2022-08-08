using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
[Serializable]
public class StaffSaveData {
    public List<StaffSetting<WorkerModelType>> workerStaffs = new List<StaffSetting<WorkerModelType>>();
    public List<StaffSetting<PlantCareModelType>> planCareStaffs = new List<StaffSetting<PlantCareModelType>>();
    public List<int> workerID;
    public List<int> plantCareID;
    const string WorkerModelType = "WorkerModelType";
    const string PlantCareModelType = "PlantCareModelType";
    public int totalStaff;
    #region StaffSetting
    public StaffSetting<T> GetStaffSetting<T>(int staffID, StaffType staffType) {
        switch (staffType)
        {
            case StaffType.Worker:
                for (int i = 0; i < workerStaffs.Count; i++)
                {
                    if (staffID == workerStaffs[i].staffID)
                        return workerStaffs[i] as StaffSetting<T>;
                }
                break;
            case StaffType.Planter:
                for (int i = 0; i < planCareStaffs.Count; i++)
                {
                    if (staffID == planCareStaffs[i].staffID)
                        return planCareStaffs[i] as StaffSetting<T>;
                }
                break;
            default:
                break;
        }
        return null;
    }
    public void SaveStaffSetting<T>(StaffSetting<T> staffSetting, bool isOverride = true) {
        bool isHasData = false;
        switch (typeof(T).ToString())
        {
            case WorkerModelType:
                for (int i = 0; i < workerStaffs.Count; i++)
                {
                    if (staffSetting.staffID == workerStaffs[i].staffID)
                    {
                        if (isOverride) workerStaffs[i] = staffSetting as StaffSetting<WorkerModelType>;
                        isHasData = true;
                    }
                }
                if (!isHasData) workerStaffs.Add(staffSetting as StaffSetting<WorkerModelType>);
                break;
            case PlantCareModelType:
                for (int i = 0; i < planCareStaffs.Count; i++)
                {
                    if (staffSetting.staffID == planCareStaffs[i].staffID)
                    {
                        if (isOverride) planCareStaffs[i] = staffSetting as StaffSetting<PlantCareModelType>;
                        isHasData = true;
                    }
                }
                if (!isHasData) planCareStaffs.Add(staffSetting as StaffSetting<PlantCareModelType>);
                break;
            default:
                break;
        }
    }
    #endregion
    #region ID
    public int GetWorkerID(int index) { return workerID[index]; }
    public int GetPlantCareID(int index) { return workerID[index]; }
    public void AddWorkerID(int ID) { workerID.Add(ID); }
    public void AddPlantCareID(int ID) { workerID.Add(ID); }
    #endregion
    #region totalStaff
    public int GetTotalStaffs() { return totalStaff; }
    public void AddTotalStaffs() { totalStaff++; }
    #endregion
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
    public int totalRooms;
    #region ID
    public int GetCarryRoomID(int index) {
        if (index == -1)
            return -1;
        return roomCarryID[index];
    }
    public int GetLandingRoomID(int index) {
        if (index == -1)
            return -1; 
        return roomLandingID[index]; 
    }
    public int GetTakeOffRoomID(int index) {
        if (index == -1)
            return -1;
        return roomTakeOffID[index]; 
    }
    public int GetFixRoomID(int index) {
        if (index == -1)
            return -1;
        return roomFixID[index];
    }
    public int GetHouseRoomID(int index) {
        if (index == -1)
            return -1;
        return roomHouseID[index]; }
    public void AddCarryRoomID(int value) { 
        roomCarryID.Add(value); 
    }
    public void AddLandingRoomID(int value) {
        roomLandingID.Add(value); }
    public void AddTakeOffRoomID(int value) {
        roomTakeOffID.Add(value);
    }
    public void AddFixRoomID(int value) {
        roomFixID.Add(value); 
    }
    public void AddHouseRoomID(int value) { 
        roomHouseID.Add(value); 
    }
    #endregion
    #region TotalRoom
    public void AddTotalRooms() { totalRooms++; }
    public int GetTotalRooms() { return totalRooms; }
    #endregion
    #region RoomSetting
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
    #endregion
}
[Serializable]
public class PlayerData
{
    public RoomSaveData roomSaveData;
    public StaffSaveData staffSaveData;
    public void InitData() {
        Debug.Log("Init Data!");
    }
    public void LoadData() {
        string jsonData = GetData();
        if (!string.IsNullOrEmpty(jsonData))
        {
            PlayerData dataSave = JsonUtility.FromJson<PlayerData>(jsonData);
            roomSaveData = dataSave.roomSaveData;
            staffSaveData = dataSave.staffSaveData; 
            Debug.Log("Load Data");
        }
        GameManager.instance.SpawnRoom();
        SaveData();
    }
    #region Room
    public void SaveRoomData<T>(BaseRoomSetting<T> room, bool isOverride = true) {
        roomSaveData.SaveRoomSettingData(room, isOverride);
    }
    public BaseRoomSetting<T> GetRoomData<T>(int roomID, RoomType roomType)
    {
        return roomSaveData.GetRoomSettingData<T>(roomID, roomType);
    }
    public int GetRoomID(int index, RoomType roomType) {
        switch (roomType)
        {
            case RoomType.LandingRoom:
                if (index < roomSaveData.roomLandingID.Count) return roomSaveData.GetLandingRoomID(index);
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
    #endregion
    #region Staff
    public void SaveStaffData<T>(StaffSetting<T> staffSetting, bool isOverride = true)
    {
        staffSaveData.SaveStaffSetting<T>(staffSetting, isOverride);
    }
    public StaffSetting<T> GetStaffData<T>(int roomID, StaffType staffType)
    {
        return staffSaveData.GetStaffSetting<T>(roomID, staffType);
    }
    public int GetStaffID(int index, StaffType staffType)
    {
        switch (staffType)
        {
            case StaffType.Worker:
                if (index < staffSaveData.workerID.Count) return staffSaveData.GetWorkerID(index);
                else return -1;
            case StaffType.Planter:
                if (index < staffSaveData.plantCareID.Count) return staffSaveData.GetPlantCareID(index);
                else return -1;
            default:
                break;
        }
        return -1;
    }
    public void AddStaffID(StaffType staffType)
    {
        switch (staffType)
        {
            case StaffType.Worker:
                staffSaveData.AddWorkerID(staffSaveData.GetTotalStaffs());
                break;
            case StaffType.Planter:
                staffSaveData.AddPlantCareID(staffSaveData.GetTotalStaffs());
                break;
            default:
                break;
        }
        staffSaveData.AddTotalStaffs();
    }
    #endregion
    public void SaveData() {
        Debug.Log("Save Data");
        PlayerPrefs.SetString("PlayerData", JsonUtility.ToJson(this).ToString());
    }
    public string GetData() { return PlayerPrefs.GetString("PlayerData"); }
}
