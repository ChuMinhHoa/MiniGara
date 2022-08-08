using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class StaffDataAsset<Enum> : ScriptableObject
{
    public List<ModelStaffData<Enum>> datas;
    public Transform GetModelByType(string type, int level) {
        for (int i = 0; i < datas.Count; i++)
        {
            if (datas[i].type.ToString() == type)
                return datas[i].model3Ds[level-1].transform;
        }
        return null;
    }
    public string GetModelNameByType(string type) {
        for (int i = 0; i < datas.Count; i++)
        {
            if (datas[i].type.ToString() == type)
                return datas[i].modelName;
        }
        return "";
    }
    public Sprite GetSpriteByType(string type) {
        for (int i = 0; i < datas.Count; i++)
        {
            if (datas[i].type.ToString() == type)
                return datas[i].sprIcon;
        }
        return null;
    }
}
