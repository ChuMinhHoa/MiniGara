using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class RoomDataAsset<Enum> : ScriptableObject
{
    public List<ModelData<Enum>> datas;
    public Transform GetModelByType(string type, int level) {
        for (int i = 0; i < datas.Count; i++)
        {
            if (datas[i].type.ToString() == type) return datas[i].models3D[level - 1].transform;
        }
        return null;
    }
    public Sprite GetSpriteModelByType(string type) {
        for (int i = 0; i < datas.Count; i++)
        {
            if (datas[i].type.ToString() == type) return datas[i].sprUI;
        }
        return null;
    }
    public int GetLevelMaxByType(string type) {
        for (int i = 0; i < datas.Count; i++)
        {
            if (datas[i].type.ToString() == type) return datas[i].models3D.Length;
        }
        return 0;
    }
    public string GetNameByType(string type) {
        for (int i = 0; i < datas.Count; i++)
        {
            if (datas[i].type.ToString() == type) return datas[i].name;
        }
        return "";
    }
    public int GetUpgradePriceByType(string type, int level) {
        for (int i = 0; i < datas.Count; i++)
        {
            if (datas[i].type.ToString() == type) return datas[i].upgradePrices[level - 1];
        }
        return 0;
    }
}
