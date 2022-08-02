using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TakeOffManager : RoomManagerBase
{
    public List<TakeOffRoom> takeOffRooms;
    public List<VehicleCarry> vehicleCarries;
    public override void SpawnRoom()
    {
        for (int i = 0; i < countRoom; i++)
        {
            AddTakeOffRoom(spawnRoomPoints[i]);
        }
    }
    public override void AddTakeOffRoom(Transform spawnPoint) {

        TakeOffRoom newTakeOffRoom = Instantiate(roomPrefab, spawnPoint.position, Quaternion.identity, roomParent).GetComponent<TakeOffRoom>();
        takeOffRooms.Add(newTakeOffRoom);
        RotageAffterSpawn(newTakeOffRoom.transform);

    }
    public void AddVehicleCarry(VehicleCarry vehicleCarry) { vehicleCarries.Add(vehicleCarry); }
    void RemoveVehicleCarry(VehicleCarry vehicleCarry) { vehicleCarries.Remove(vehicleCarry); }
    private void Update()
    {
        if (vehicleCarries.Count > 0)
        {
            TakeOffRoom takeOffRoom = GetTakeOffRoom();
            if (takeOffRoom != null)
            {
                takeOffRoom.AddVehicleCarry(vehicleCarries[0]);
                RemoveVehicleCarry(vehicleCarries[0]);
            }
        }
    }
    public override TakeOffRoom GetTakeOffRoom()
    {
        foreach (TakeOffRoom takeOffRoom in takeOffRooms)
        {
            if (takeOffRoom.CheckAble())
                return takeOffRoom;
        }
        foreach (TakeOffRoom takeOffRoom in takeOffRooms)
        {
            if (takeOffRoom.CheckAbleWaittingPoint())
                return takeOffRoom;
        }
        return null;
    }
}
