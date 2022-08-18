using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bed : MonoBehaviour
{
    public List<Transform> sleepPoints;
    public List<bool> activateSleepPoint;
    public Vector3 GetSleepPoint() {
        for (int i = 0; i < sleepPoints.Count; i++)
        {
            if (activateSleepPoint[i])
            {
                activateSleepPoint[i] = false;
                return sleepPoints[i].position;
            }
        }
        return Vector3.zero;
    }
    public void ResetBed() {
        for (int i = 0; i < activateSleepPoint.Count; i++)
            activateSleepPoint[i] = true;
    }
}
