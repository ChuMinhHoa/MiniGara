using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class ModelPosition<Enum>
{
    public Enum type;
    public int level;
    public Transform rootObject;
    [HideInInspector]
    public Transform currentModel;
}
