using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorkerManager : RoomManagerBase
{
    public List<House> houses;
    public WaitingPoint GetWaitingPoint(House houseFinding) {
        WaitingPoint waitingPoint = houseFinding.GetWaitingPoint();
        if (waitingPoint != null)
        {
            waitingPoint.able = false;
            return waitingPoint;
        }
        return null;
    }
    public Worker GetWorker() {
        foreach (House house in houses)
        {
            Worker workerGet = house.GetWorker();
            if (workerGet != null)
                return workerGet;
        }
        return null;
    }
    public override void SpawnRoom()
    {
        for (int i = 0; i < countRoom; i++)
        {
            AddHouseRoom(spawnRoomPoints[i], i);
        }
    }
    public override void AddHouseRoom(Transform spawnPoint, int indexRoom)
    {
        House newHouse = Instantiate(roomPrefab, spawnPoint.position, Quaternion.identity, roomParent).GetComponent<House>();
        houses.Add(newHouse);
        int roomID = ProfileManager.instance.playerData.GetRoomID(indexRoom, RoomType.House);
        if (roomID != -1)
        {
            newHouse.roomSetting.roomID = roomID;
            newHouse.OnLoadRoom();
            Debug.Log("Load House ID:" + roomID + " Data");
        }
        else
        {
            ProfileManager.instance.playerData.AddRoomID(RoomType.House);
            newHouse.roomSetting.roomID = GameManager.instance.roomCount;
            ProfileManager.instance.playerData.SaveRoomData<HouseModelType>(newHouse.roomSetting);
            newHouse.OnLoadRoom();
            Debug.Log("Create House ID:" + newHouse.roomSetting.roomID + " Data");
        }
        RotageAffterSpawn(newHouse.transform);
        GameManager.instance.roomCount++;
    }
    public void CommandToHouse(HouseRoomState houseState) {
        for (int i = 0; i < houses.Count; i++)
            houses[i].ChangeState(houseState);
    }
}
