using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseRoom : MonoBehaviour
{
    public List<Transform> modelPosition;
    public RoomType roomType;
    public List<WaitingPoint> waitingPoints;
    public virtual WaitingPoint GetWaitingPoint() {
        foreach (WaitingPoint point in waitingPoints)
        {
            if (point.able)
                return point;
        }
        return null;
    }
}
