using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sofa : MonoBehaviour
{
    [SerializeField] Transform pointSeatDown;
    [SerializeField] bool sofaAble = true;
    public Vector3 GetPointSeatDown() { return pointSeatDown.position; }
    public bool GetSofaAble() { return sofaAble; }
    public void SetSofaAble(bool able) { sofaAble = able; }
}
