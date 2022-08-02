using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FixRoomManager : RoomManagerBase
{
    public List<FixRoom> fixRooms;
    public override void SpawnRoom() {
        for (int i = 0; i < countRoom; i++)
        {
            AddFixRoom(spawnRoomPoints[i]);
        }
    }
    public override void AddFixRoom(Transform spawnPoint)
    {
        FixRoom newFixRoom = Instantiate(roomPrefab, spawnPoint.position, Quaternion.identity, roomParent).GetComponent<FixRoom>();
        fixRooms.Add(newFixRoom);
        RotageAffterSpawn(newFixRoom.transform);
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
