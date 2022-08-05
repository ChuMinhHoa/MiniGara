using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StaffDataAsset<Enum> : ScriptableObject
{
    List<ModelStaffData<Enum>> datas;
    public ModelStaffData<Enum> GetModelByType(string type) {
        for (int i = 0; i < datas.Count; i++)
        {
            if (datas[i].type.ToString() == type)
                return datas[i];
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
