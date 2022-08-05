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
            AddHouseRoom(spawnRoomPoints[i]);
        }
    }
    public override void AddHouseRoom(Transform spawnPoint)
    {
        House newHouse = Instantiate(roomPrefab, spawnPoint.position, Quaternion.identity, roomParent).GetComponent<House>();
        houses.Add(newHouse);
        RotageAffterSpawn(newHouse.transform);
    }
}
