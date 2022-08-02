using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BrokenRoom : RoomManagerBase
{
    public List<VehicleBroke> vehicleNeedRepair;
    public float timeRespawnDefault;
    public int amountSpawnDefault;
    float timeRemaining;
    int amountSpawn;
    float timeCoolDown;
    int amountSpawned = 0;
    bool timeToSpawn;
    bool spawnedDone;
    public void SpawnBrokeVehicle() {
        if (timeCoolDown <= 0 && !spawnedDone)
        {
            VehicleBroke newVehicleBroke = Instantiate(roomPrefab, spawnRoomPoints[amountSpawned].position, Quaternion.identity, roomParent).GetComponent<VehicleBroke>();
            vehicleNeedRepair.Add(newVehicleBroke);
            amountSpawned += 1;
            timeCoolDown = .25f;
        }
        else if (timeCoolDown > 0){
            timeCoolDown -= Time.deltaTime;
        }
        if(amountSpawned == amountSpawn && !spawnedDone)
        {
            timeCoolDown = 0;
            timeToSpawn = false;
            timeRemaining = timeRespawnDefault;
            spawnedDone = true;
        }
    }
    private void Update()
    {
        if (vehicleNeedRepair.Count > 0 && GameManager.instance.CheckToCallVehicleBroke())
        {
            VehicleBroke vehicleBroke = vehicleNeedRepair[0];
            LandingPad freeLanding = GameManager.instance.landingPadManager.GetLandingRoom();
            if (freeLanding != null)
            {
                freeLanding.able = false;
                freeLanding.ChangeVehicle(vehicleBroke);
                int indexOfLanchPad = GameManager.instance.landingPadManager.landingPads.IndexOf(freeLanding);
                GameManager.instance.landingPadManager.lanchPads[indexOfLanchPad].ChangePickUp(vehicleBroke.transform);
                vehicleBroke.ChangeLandingPad(freeLanding, GameManager.instance.landingPadManager.lanchPads[indexOfLanchPad]);
                vehicleBroke.ChangeTargetMove(freeLanding.landingStartPosition.position, freeLanding.landingEndPosition.position);
                vehicleNeedRepair.Remove(vehicleBroke);
            }
        }
        if (timeToSpawn)
        {
            SpawnBrokeVehicle();
            return; 
        }
        if (timeRemaining > 0)
            timeRemaining -= Time.deltaTime;
        else {
            amountSpawn = amountSpawnDefault + GameManager.instance.GetBuffAmountSpawnVehicleBroke();
            timeToSpawn = true;
        }
    }
    public void RemoveVehicleBroke(VehicleBroke vehicleRemove) {
        vehicleNeedRepair.Remove(vehicleRemove);
        if (vehicleNeedRepair.Count == 0)
            spawnedDone = false;
    }
}
