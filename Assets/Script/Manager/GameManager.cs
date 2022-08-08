using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : GenericSingleton<GameManager>
{
    public CarryManager carryVehicleCarryManager;
    public LandingPadManager landingPadManager;
    public FixRoomManager fixRoomManager;
    public WorkerManager workerManager;
    public TakeOffManager takeOffManager;
    public int roomCount;
    public int staffCount;
    public void SpawnRoom() {
        carryVehicleCarryManager.SpawnRoom();
        landingPadManager.SpawnRoom();
        fixRoomManager.SpawnRoom();
        workerManager.SpawnRoom();
        takeOffManager.SpawnRoom();
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.N))
        {
            Time.timeScale += 2f;
        }
        if (Input.GetKeyDown(KeyCode.B))
        {
            Time.timeScale -= 2f;
        }
    }
    public bool CheckToCallVehicleBroke() {
        LandingPad landingPad = landingPadManager.GetLandingRoom();
        if (landingPad == null)
            return false;
        int countAbleVehicle = carryVehicleCarryManager.CountCarryVehicle();
        if (countAbleVehicle <= 1)
            return false;
        return true;
    }
    public int GetBuffAmountSpawnVehicleBroke() {
        return 0;
    }
}
