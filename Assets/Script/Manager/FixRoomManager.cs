using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FixRoomManager : RoomManagerBase
{
    public List<FixRoom> fixRooms;
    public override void SpawnRoom() {
        for (int i = 0; i < countRoom; i++)
        {
            AddFixRoom(spawnRoomPoints[i], i);
        }
    }
    public override void AddFixRoom(Transform spawnPoint, int indexRoom)
    {
        FixRoom newFixRoom = Instantiate(roomPrefab, spawnPoint.position, Quaternion.identity, roomParent).GetComponent<FixRoom>();
        fixRooms.Add(newFixRoom);
        int roomID = ProfileManager.instance.playerData.GetRoomID(indexRoom, RoomType.FixRoom);
        if (roomID != -1)
        {
            newFixRoom.roomSetting.roomID = roomID;
            newFixRoom.OnLoadRoom();
            Debug.Log("Load Fix Room ID:" + roomID + " Data");
        }
        else
        {
            ProfileManager.instance.playerData.AddRoomID(RoomType.FixRoom);
            newFixRoom.roomSetting.roomID = GameManager.instance.roomCount;
            ProfileManager.instance.playerData.SaveRoomData<FixRoomModelType>(newFixRoom.roomSetting);
            newFixRoom.OnLoadRoom();
            Debug.Log("Create Fix Room ID:" + newFixRoom.roomSetting.roomID + " Data");
        }
        RotageAffterSpawn(newFixRoom.transform);
        GameManager.instance.roomCount++;
    }
    public override FixRoom GetFixRoom() {
        foreach (FixRoom fixRoom in fixRooms)
        {
            if (fixRoom.able)
                return fixRoom;
        }
        foreach (FixRoom fixRoom in fixRooms)
        {
            if (fixRoom.CheckAble())
                return fixRoom;
        }
        return null;
    }
}
