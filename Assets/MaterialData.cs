using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "New MaterialData", menuName = "ScripableObject/New marterialData")]
public class MaterialData : ScriptableObject
{
    public List<MaterialClass> materials;
}
