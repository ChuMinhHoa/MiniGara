using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LandingPadManager : RoomManagerBase
{
    public List<LandingPad> landingPads;
    public List<LanchPad> lanchPads;
    public GameObject lanchPadPrefab;
    public List<Transform> pointSpawnLanchs;
    public Quaternion rotageLanch;
    public override void SpawnRoom() {
        for (int i = 0; i < countRoom; i++)
        {
            AddLandingRoom(spawnRoomPoints[i], pointSpawnLanchs[i]);
        }
    }
    public override void AddLandingRoom(Transform spawnLandingPoint, Transform spawnLanchPoint)
    {
        LandingPad newLandingPad = Instantiate(roomPrefab, spawnLandingPoint.position, Quaternion.identity, roomParent).GetComponent<LandingPad>();
        LanchPad newLanchPad = Instantiate(lanchPadPrefab, spawnLanchPoint.position, Quaternion.identity, roomParent).GetComponent<LanchPad>();
        newLanchPad.currentLanding = newLandingPad;
        landingPads.Add(newLandingPad);
        int roomID = ProfileManager.instance.playerData.GetRoomID(GameManager.instance.roomCount, RoomType.CarryRoom);
        if (roomID != -1)
        {
            newLandingPad.roomSetting.roomID = roomID;
            newLandingPad.OnLoadRoom();
            Debug.Log("Load LandingPad ID:" + roomID + " Data");
        }
        else
        {
            ProfileManager.instance.playerData.AddRoomID(RoomType.LandingRoom);
            newLandingPad.roomSetting.roomID = GameManager.instance.roomCount;
            ProfileManager.instance.playerData.SaveRoomData<LandingPadModelType>(newLandingPad.roomSetting);
            newLandingPad.OnLoadRoom();
            Debug.Log("Create LandingPad ID:" + newLandingPad.roomSetting.roomID + " Data");
        }
        lanchPads.Add(newLanchPad);
        RotageAffterSpawn(newLandingPad.transform);
        RotageAffterSpawn(newLanchPad.transform, rotageLanch);
        GameManager.instance.roomCount++;
    }
    public override LandingPad GetLandingRoom() {
        for (int i = 0; i < landingPads.Count; i++)
        {
            if (landingPads[i].able)
                return landingPads[i];
        }
        return null;
    }
}
