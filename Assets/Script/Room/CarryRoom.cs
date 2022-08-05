using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class CarryRoom : BaseRoom<CarryRoomModelType>
{
    public List<VehicleCarry> vehiclesCarry;
    public GameObject vehicleCarryPreb;
    public List<Transform> spawnPoints;
    private void Start()
    {
        InitVehicle();
    }
    void InitVehicle() {
        for (int i = 0; i < 2; i++)
        {
            VehicleCarry newVehicleCarry = Instantiate(vehicleCarryPreb, waitingPoints[i].point.position, Quaternion.identity).GetComponent<VehicleCarry>();
            newVehicleCarry.ChangeRotageTo(Quaternion.LookRotation(Vector3.right, Vector3.up));
            newVehicleCarry.ChangeTargetMove(waitingPoints[i].point.position, newVehicleCarry.ResetAble);
            newVehicleCarry.ChangeWaitingPoint(waitingPoints[i]);
            vehiclesCarry.Add(newVehicleCarry);
        }
    }
    public VehicleCarry GetAbleVehicle()
    {
        foreach (VehicleCarry vehicle in vehiclesCarry)
        {
            if (vehicle.able)
                return vehicle;
        }
        return null;
    }
    public void MoveVehicleToDefaultPoint(VehicleCarry vehicleCarry, UnityAction actionDone = null)
    {
        WaitingPoint waitingPoint = GetWaitingPoint();
        if (waitingPoint != null)
        {
            vehicleCarry.ChangeWaitingPoint(waitingPoint);
            vehicleCarry.ChangeRotageTo(Quaternion.LookRotation(Vector3.right, Vector3.up));
            vehicleCarry.ChangeTargetMove(waitingPoint.point.position, actionDone);
        }
    }
    public int CountAbleVehicle() {
        int count = 0;
        foreach (VehicleCarry vehicleAble in vehiclesCarry)
        {
            if (vehicleAble.able)
                count++;
        }
        return count;
    }
}
