using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarryManager : RoomManagerBase
{
    public List<VehicleCarry> vehicleProgress;
    public List<CarryRoom> carryRooms;
    public void AddVehicleProgress(VehicleCarry vehicle) { vehicleProgress.Add(vehicle); }
    public override void SpawnRoom()
    {
        for (int i = 0; i < countRoom; i++)
        {
            AddCarryRoom(spawnRoomPoints[i]);
        }
    }
    public override void AddCarryRoom(Transform instancePoint) {
        CarryRoom newCarryRoom = Instantiate(roomPrefab, instancePoint.position, Quaternion.identity, roomParent).GetComponent<CarryRoom>();
        carryRooms.Add(newCarryRoom);
        RotageAffterSpawn(newCarryRoom.transform);
    }
    private void Update()
    {
        if (vehicleProgress.Count > 0)
        {
            FixRoom fixRoom = GameManager.instance.fixRoomManager.GetFixRoom();
            if (fixRoom != null)
            {
                vehicleProgress[0].ChangeWaitingPoint();
                fixRoom.AddProgress(vehicleProgress[0]);
                vehicleProgress.Remove(vehicleProgress[0]);
                return;
            }
            else {
                for (int i = 0; i < vehicleProgress.Count; i++)
                {
                    if (vehicleProgress[i].myWaitingPoint == null)
                    {
                        GetCarryRoom().MoveVehicleToDefaultPoint(vehicleProgress[i]);
                    }
                }
            }
        }
    }
    public VehicleCarry GetAbleVehicle() {
        foreach (CarryRoom carryRoom in carryRooms)
        {
            VehicleCarry vehicleCarry = carryRoom.GetAbleVehicle();
            if (vehicleCarry != null)
            {
                return vehicleCarry;
            }
        }
        return null;
    }
    public override CarryRoom GetCarryRoom() {
        foreach (CarryRoom carryRoom in carryRooms)
        {
            if (carryRoom.GetWaitingPoint() != null)
            {
                return carryRoom;
            }
        }
        return null;
    }
    public int CountCarryVehicle() {
        int Count = 0;
        foreach (CarryRoom carryRoom in carryRooms)
        {
            Count += carryRoom.CountAbleVehicle();
        }
        return Count;
    }
}
