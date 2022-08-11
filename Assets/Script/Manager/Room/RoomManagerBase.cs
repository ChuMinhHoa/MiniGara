using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomManagerBase : MonoBehaviour
{
    public List<Transform> spawnRoomPoints;
    public Transform roomParent;
    public Quaternion rotageRoomSpawn;
    public GameObject roomPrefab;
    public int countRoom;
    public virtual void SpawnRoom() {}
    public virtual void RotageAffterSpawn(Transform newObjTransform) {
        newObjTransform.rotation = rotageRoomSpawn;
    }
    public virtual void RotageAffterSpawn(Transform newObjTransform, Quaternion rotageTo)
    {
        newObjTransform.rotation = rotageTo;
    }
    public virtual void AddFixRoom(Transform spawnPoint, int indexRoom) { }
    public virtual void AddLandingRoom(Transform spawnLandingPoint, Transform spawnLanchPoint, int indexRoom) { }
    public virtual void AddCarryRoom(Transform spawnPoint, int indexRoom) { }
    public virtual void AddTakeOffRoom(Transform spawnPoint, int indexRoom) { }
    public virtual void AddHouseRoom(Transform spawnPoint, int indexRoom) { }
    public virtual FixRoom GetFixRoom() {
        return null;
    }
    public virtual LandingPad GetLandingRoom() {
        return null;
    }
    public virtual CarryRoom GetCarryRoom()
    {
        return null;
    }
    public virtual LanchPad GetLanchRoom()
    {
        return null;
    }
    public virtual TakeOffRoom GetTakeOffRoom()
    {
        return null;
    }
}