using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "New FixRoom Data Asset", menuName = "ScriptAbleObjects/NewFixRoomDataAsset")]
public class FixRoomDataAsset : RoomDataAsset<FixRoomModelType> {
    public List<int> comfortable;
    public override int GetComforable(int level)
    {
        return comfortable[level];
    }
}
