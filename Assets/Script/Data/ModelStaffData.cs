using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class ModelStaffData<Enum>
{
    public string modelName;
    public Enum type;
    public List<GameObject> model3Ds;
    public Sprite sprIcon;
}
