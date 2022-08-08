using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class House : BaseRoom<HouseModelType>
{
    protected StateMachine<House> m_Statemachine;
    public StateMachine<House> stateMachine;
    [Header("WORKER")]
    [Header("================HOUSE=================")]
    public List<Worker> workers;
    public int maxAmountWorker = 2;
    int currentAmountWorker = 1;
    [Header("TRANSFORM")]
    public Transform pointCallSleep;
    public Transform pointOpenDoor;
    public Transform window;
    public Transform sofarPoint;
    public Transform doorRotage;
    [Header("QUATERNION")]
    public Quaternion rotageDoor;
    public Quaternion rotageWindow;
    Quaternion defaultRotageDoor;
    Quaternion defaultRotageWindow;
    [Header("CURVE")]
    public AnimationCurve rotageCurve;
    float timeRotage;
    public GameObject workerPrefab;
    

    private void Awake()
    {
        m_Statemachine = new StateMachine<House>(this);
    }
    private void Start()
    {
        defaultRotageDoor = doorRotage.localRotation;
        defaultRotageWindow = window.localRotation;
        InstanceWorker();
    }
    public void IdleEnter() {
        timeRotage = 0f;
    }
    public void IdleExecute() {
        if (timeRotage <= rotageCurve.keys[rotageCurve.length].time)
        {
            doorRotage.rotation = Quaternion.Slerp(doorRotage.rotation, defaultRotageDoor, rotageCurve.Evaluate(timeRotage));
            window.rotation = Quaternion.Slerp(window.rotation, defaultRotageWindow, rotageCurve.Evaluate(timeRotage));
            timeRotage += Time.deltaTime;
        }
    }
    public void IdleEnd() { }
    public void SleepEnter() { }
    public void SleepExecute() { }
    public void SleepEnd() { }
    public void BreakTimeEnter() { }
    public void BreakTimeExecute() { }
    public void BreakTimeEnd() { }
    void InstanceWorker() {
        for (int i = 0; i < currentAmountWorker; i++) { 
            Worker workerNew = Instantiate(workerPrefab, pointOpenDoor.position, Quaternion.identity).GetComponent<Worker>();
            workers.Add(workerNew);
            workerNew.ChangeHouse(this);
            int staffID = ProfileManager.instance.playerData.GetStaffID(i, StaffType.Worker);
            if (staffID != -1)
            {
                workerNew.staffSetting.staffID = staffID;
                workerNew.OnLoadStaff();
                Debug.Log("Load Worker ID:" + staffID + " Data");
            }
            else
            {
                ProfileManager.instance.playerData.AddStaffID(StaffType.Worker);
                workerNew.staffSetting.staffID = GameManager.instance.staffCount;
                ProfileManager.instance.playerData.SaveStaffData<WorkerModelType>(workerNew.staffSetting);
                workerNew.OnLoadStaff();
                Debug.Log("Create Worker ID:" + workerNew.staffSetting.staffID + " Data");
            }
            GameManager.instance.staffCount++;
        }
        ProfileManager.instance.playerData.SaveData();
    }
    public Worker GetWorker() {
        foreach (Worker worker in workers)
        {
            if (worker.able)
            {
                worker.able = false;
                return worker;
            }
        }
        return null;
    }
}
public class HouseIdle : State<House> {
    private static HouseIdle m_Instance;
    public static HouseIdle instance
    {
        get {
            if (m_Instance == null)
                m_Instance = new HouseIdle();
            return m_Instance;
        }
    }
    public override void Enter(House go)
    {
        go.IdleEnter();
    }
    public override void Execute(House go)
    {
        go.IdleExecute();
    }
    public override void End(House go)
    {
        go.IdleEnd();
    }
}
public class HouseSleep : State<House>
{
    private static HouseSleep m_Instance;
    public static HouseSleep instance
    {
        get
        {
            if (m_Instance == null)
                m_Instance = new HouseSleep();
            return m_Instance;
        }
    }
    public override void Enter(House go)
    {
        go.SleepEnter();
    }
    public override void Execute(House go)
    {
        go.SleepExecute();
    }
    public override void End(House go)
    {
        go.SleepEnd();
    }
}
public class HouseBreakTime : State<House>
{
    private static HouseBreakTime m_Instance;
    public static HouseBreakTime instance
    {
        get
        {
            if (m_Instance == null)
                m_Instance = new HouseBreakTime();
            return m_Instance;
        }
    }
    public override void Enter(House go)
    {
        go.BreakTimeEnter();
    }
    public override void Execute(House go)
    {
        go.BreakTimeExecute();
    }
    public override void End(House go)
    {
        go.BreakTimeEnd();
    }
}