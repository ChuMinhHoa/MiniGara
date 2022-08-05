using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class ModelData<Enum>
{
    public string name;
    public Enum type;
    public GameObject[] models3D;
    public List<int> upgradePrices;
    public Sprite sprUI;
}
