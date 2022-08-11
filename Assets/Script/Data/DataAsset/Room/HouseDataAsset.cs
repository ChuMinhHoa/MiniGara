using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "New HouseRoom Data Asset", menuName = "ScriptAbleObjects/NewHouseRoomDataAsset")]
public class HouseDataAsset : RoomDataAsset<HouseModelType> {
    public List<int> comfortable;
    public override int GetComforable(int level)
    {
        return comfortable[level-1];
    }
}
