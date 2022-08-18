using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
[System.Serializable]
public class TimeLineManager
{
    public bool workerTime;
    //public bool otherTime;
    float timeSpeed = 90;
    public float currentTime;
    public List<TimePlan> timePlans;
    public void Update()
    {
        currentTime += Time.deltaTime * timeSpeed;
        UIManager.instance.timePanel.ChangeTimeText(FormatToHoursAndMinitue(TimeSpan.FromSeconds(currentTime)));
        for (int i = 0; i < timePlans.Count; i++)
        {
            if (currentTime >= timePlans[i].timeStart)
                SettingPlan(timePlans[i].behaviorType, timePlans[i]);
        }
        if (currentTime >= 86400f)
            ResetDay();
    }
    void ResetDay() {
        currentTime = 0;
        for (int i = 0; i < timePlans.Count; i++)
            timePlans[i].activate = false;
        workerTime = false;
    }
    void SettingPlan(BehaviorType behaviorType, TimePlan timePlan) {
        if (!timePlan.activate)
        {
            switch (behaviorType)
            {
                case BehaviorType.Eat:
                    workerTime = false;
                    Debug.Log("Eat now");
                    GameManager.instance.workerManager.CommandToHouse(HouseRoomState.EatTime);
                    break;
                case BehaviorType.Work:
                    workerTime = true;
                    Debug.Log("Work now");
                    GameManager.instance.workerManager.CommandToHouse(HouseRoomState.WorkTime);
                    break;
                case BehaviorType.FreeTime:
                    workerTime = false;
                    Debug.Log("Free now");
                    GameManager.instance.workerManager.CommandToHouse(HouseRoomState.FreeTime);
                    break;
                case BehaviorType.Sleep:
                    workerTime = false;
                    Debug.Log("Sleep now");
                    GameManager.instance.workerManager.CommandToHouse(HouseRoomState.SleepTime);
                    break;
                default:
                    break;
            }
            timePlan.activate = true;
        }
    }
    string FormatToHoursAndMinitue(TimeSpan timeSpan) {
        string timeStr = string.Format("{0:D2} {1:D2}", timeSpan.Hours, timeSpan.Minutes);
        return timeStr;
    }
}
