using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum selfRotage {x,y,z}
public class SelfRotage : MonoBehaviour
{
    public float rotageSpeed;
    float currentAngle;
    public selfRotage selfRotage;
    void Update() {
        currentAngle += rotageSpeed * Time.deltaTime;
        switch (selfRotage)
        {
            case selfRotage.x:
                transform.eulerAngles = new Vector3(currentAngle, 0, 0);
                break;
            case selfRotage.y:
                transform.eulerAngles = new Vector3(0, currentAngle, 0);
                break;
            case selfRotage.z:
                transform.eulerAngles = new Vector3(0, 0, currentAngle);
                break;
            default:
                break;
        }
    }
}
