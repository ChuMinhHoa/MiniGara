using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class MaterialClass {
    public Material materialFixed;
    public Material materialBroken;
    public VehicleType vehicleType;
}
public class MaterialController : MonoBehaviour
{
    public MaterialData materialData;
    public MaterialClass GetMaterial(VehicleType vehicleType) {
        foreach (MaterialClass data in materialData.materials)
        {
            if (data.vehicleType == vehicleType)
                return data;
        }
        return null;
    }
}
