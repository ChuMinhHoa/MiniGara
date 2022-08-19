using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;
using System;

public class Worker : StaffBase<WorkerModelType>
{
    House myHouse;
    [Header("==============Free Time===================")]
    public FreeBehavior currentBehavior;
    public Queue<FreeBehavior> freetimeBehaviors = new Queue<FreeBehavior>();
    int countTalkTime;
    Action freeTimeModeDone;
    Vector3 behaviorPoint;
    #region ====================Sleep=================
    public override void StaffSleepEnter()
    {
        agent.enabled = true;
        target = myHouse.pointCallSleep.position;
        agent.isStopped = false;
        agent.SetDestination(target);
        agent.speed = 1f;
        anim.Play("Running");
    }
    public override void StaffSleepExecute()
    {
        if (sleepTime)
            return;
        if (IsFinishMoveOnNavemesh() && !sleepTime)
            SleepModeSetting();
    }
    public override void StaffSleepEnd()
    {
        myHouse.ResetBed();
        transform.position = myHouse.pointCallSleep.position;
        sleepTime = false;
    }
    #endregion
    #region==================FreeTime================
    public override void StaffFreeTimeEnter()
    {
        FreeTimeModeSetting();
        agent.speed = 0.5f;
    }
    public override void StaffFreeTimeExecute()
    {
        if (!freeTime)
        {
            if (freetimeBehaviors.Count > 0)
                currentBehavior = freetimeBehaviors.Dequeue();
            else
            {
                FreeTimeModeSetting();
                return;
            }
            
            switch (currentBehavior)
            {
                case FreeBehavior.Talking:
                    countTalkTime = 0;
                    myHouse.SetCurrentPointIndex(0);
                    behaviorPoint = myHouse.GetPointTalking(myHouse.GetCurrentPointIndex());
                    actionMoveDone = Talking;
                    myHouse.IncreaseCurrentPointIndex();
                    agent.enabled = true;
                    agent.isStopped = false;
                    agent.SetDestination(behaviorPoint);
                    anim.Play("Walking");
                    break;
                case FreeBehavior.WatchTV:
                    if (CheckAbleFreeTimeBehaviour(FreeBehavior.WatchTV))
                    {
                        behaviorPoint = myHouse.GetPointSofar();
                        myHouse.SetSofaPoint(false);
                        actionMoveDone = WatchingTV;
                        agent.enabled = true;
                        agent.isStopped = false;
                        agent.SetDestination(behaviorPoint);
                        anim.Play("Walking");
                    }
                    else
                    {
                        freeTime = false;
                        return;
                    }
                    break;
                default:
                    break;
            }
            freeTime = true;
        }
        if (agent.enabled == false)
            return;
        if (IsFinishMoveOnNavemesh())
        {
            agent.isStopped = true;
            actionMoveDone();
        }
    }
    bool CheckAbleFreeTimeBehaviour(FreeBehavior behaviorCheck) {
        switch (behaviorCheck)
        {
            case FreeBehavior.WatchTV:
                return myHouse.CheckBehaviorWatchTV();
            default:
                break;
        }
        return false;
    }
    public override void StaffFreeTimeEnd(){ freeTime = false; }
    void Talking()
    {
        Vector3 pointRotageTo = myHouse.GetOtherPoint();
        pointRotageTo.y = transform.position.y;
        Quaternion rotage = Quaternion.LookRotation(pointRotageTo - transform.position, Vector3.up);
        transform.rotation = rotage;
        int talkAnim = UnityEngine.Random.Range(1, 3);
        anim.Play("Talking_" + talkAnim.ToString());
    }
    void WatchingTV() {
        agent.enabled = false;
        transform.position = behaviorPoint;
        anim.SetTrigger("WatchTV");
        Vector3 pointRotageTo = myHouse.GetRootTransform(HouseModelType.House_TV);
        pointRotageTo.y = transform.position.y;
        Quaternion rotage = Quaternion.LookRotation(pointRotageTo - transform.position, Vector3.up);
        transform.rotation = rotage;
        
    }
    #endregion
    #region =============Anim function==============
    public void WatchTVDone()
    {
        freeTime = false;
        myHouse.SetSofaPoint(true);
    }
    public void TalkingDone()
    {
        countTalkTime++;
        if (countTalkTime == 2)
            freeTime = false;
    }
    #endregion
    #region ===========State Setting=============
    public void SleepModeSetting() {
        agent.isStopped = true;
        agent.enabled = false;
        Vector3 point = myHouse.GetSleepPoint();
        Debug.Log(point);
        transform.position = point;
        transform.rotation = sleepRotage;
        anim.Play("Sleep");
        sleepTime = true;
    }
    public void FreeTimeModeSetting() { 
        able = false;
        agent.enabled = true;
        int maxBehaviour = Enum.GetNames(typeof(FreeBehavior)).Length;
        
        for (int i = 0; i < 10; i++)
        {
            int randomBeha = UnityEngine.Random.Range(0, maxBehaviour);
            switch (randomBeha)
            {
                case 0:
                    freetimeBehaviors.Enqueue(FreeBehavior.Talking);
                    break;
                case 1:
                    freetimeBehaviors.Enqueue(FreeBehavior.WatchTV);
                    break;
                default:
                    break;
            }
        }
    }
    public void WorkModeSetting() { 
        able = true;
        agent.enabled = true;
        ChangeState(StaffState.Idle);
    }
    #endregion
    public void ChangeHouse(House house)
    {
        myHouse = house;
    }
    public House GetHouse()
    {
        return myHouse;
    }
}

