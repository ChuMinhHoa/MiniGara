using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class StaffSetting<T>
{
    public StaffType staffType;
    public int staffID;
    public List<StaffModelPos<T>> staffModelsPos;
}
