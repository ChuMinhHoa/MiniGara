using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class House : BaseRoom<HouseModelType>
{
    protected StateMachine<House> m_Statemachine;
    public StateMachine<House> stateMachine { get { return m_Statemachine; } }
    [Header("WORKER")]
    [Header("================HOUSE=================")]
    public List<Worker> workers;
    public int maxAmountWorker = 2;
    int currentAmountWorker = 1;
    int currentPointTalkIndex = 0;
    [Header("TRANSFORM")]
    public Transform pointCallSleep;
    public Transform pointOpenDoor;
    public Transform window;
    public Transform doorRotage;
    public List<Transform> pointTalkings = new List<Transform>();
    [Header("QUATERNION")]
    public Quaternion rotageDoor;
    public Quaternion rotageWindow;
    Quaternion defaultRotageDoor;
    Quaternion defaultRotageWindow;
    [Header("CURVE")]
    public AnimationCurve rotageCurve;
    float timeRotage;
    public GameObject workerPrefab;
    [Header("State")]
    HouseRoomState currentState;
    public HouseRoomState state;
    [Header("Worker In Free Time")]
    public Bed bedInRoom;
    public Sofa sofaInRoom;
    private void Awake()
    {
        m_Statemachine = new StateMachine<House>(this);
        m_Statemachine.SetcurrentState(HouseIdle.instance);
        m_Statemachine.ChangeState(HouseIdle.instance);
    }
    private void Start()
    {
        defaultRotageDoor = doorRotage.localRotation;
        defaultRotageWindow = window.localRotation;
        InstanceWorker();
    }
    private void Update()
    {
        stateMachine.Update();
        switch (state)
        {
            case HouseRoomState.Idle:
                if (currentState!= state)
                {
                    currentState = state;
                    stateMachine.ChangeState(HouseIdle.instance);
                }
                break;
            case HouseRoomState.SleepTime:
                if (currentState != state)
                {
                    currentState = state;
                    stateMachine.ChangeState(HouseSleep.instance);
                }
                break;
            case HouseRoomState.FreeTime:
                if (currentState != state)
                {
                    currentState = state;
                    stateMachine.ChangeState(HouseBreakTime.instance);
                }
                break;
            case HouseRoomState.WorkTime:
                if (currentState != state)
                {
                    currentState = state;
                    stateMachine.ChangeState(HouseWorkTime.instance);
                }
                break;
            default:
                break;
        }
    }
    #region =======================Data Control=============================
    public override void LoadFromSaveData(BaseRoomSetting<HouseModelType> saveRoom)
    {
        base.LoadFromSaveData(saveRoom);
        for (int i = 0; i < roomSetting.modelPositions.Count; i++)
        {
            ModelPosition<HouseModelType> model = roomSetting.modelPositions[i];
            if (model.type == HouseModelType.House_Bed)
                bedInRoom = roomSetting.modelPositions[i].rootObject.GetChild(0).GetComponent<Bed>();
            if (model.type == HouseModelType.House_Sofa)
            {
                if(roomSetting.modelPositions[i].rootObject.childCount>0) 
                    sofaInRoom = roomSetting.modelPositions[i].rootObject.GetChild(0).GetComponent<Sofa>();
            }
        }
    }
    public void ChangeState(HouseRoomState houseRoomState) { state = houseRoomState; }
    public Vector3 GetSleepPoint()
    {
        return bedInRoom.GetSleepPoint();
    }
    public Worker GetWorker()
    {
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
    public void ResetBed() { bedInRoom.ResetBed(); }
    void InstanceWorker()
    {
        for (int i = 0; i < currentAmountWorker; i++)
        {
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
    #endregion
    #region Command
    void CommandSleep()
    {
        for (int i = 0; i < workers.Count; i++)
            workers[i].ChangeState(StaffState.Sleep);
    }
    void CommandSleepDone() {
        for (int i = 0; i < workers.Count; i++)
        {
            workers[i].transform.position = pointCallSleep.position;
            workers[i].WorkModeSetting();
        }
    }
    void CommandFreeTime() {
        for (int i = 0; i < workers.Count; i++)
            workers[i].ChangeState(StaffState.FreeTime);
    }
    #endregion
    #region ====================STATE=======================
    public void IdleEnter()
    {
        timeRotage = 0f;
    }
    public void IdleExecute()
    {
        if (timeRotage <= rotageCurve.keys[rotageCurve.length - 1].time)
        {
            doorRotage.rotation = Quaternion.Slerp(doorRotage.rotation, defaultRotageDoor, rotageCurve.Evaluate(timeRotage));
            window.rotation = Quaternion.Slerp(window.rotation, defaultRotageWindow, rotageCurve.Evaluate(timeRotage));
            timeRotage += Time.deltaTime;
        }
    }
    public void IdleEnd() { }
    public void SleepEnter()
    {
        CommandSleep();
        timeRotage = 0f;
    }
    public void SleepExecute()
    {
        if (timeRotage <= rotageCurve.keys[rotageCurve.length - 1].time)
        {
            doorRotage.rotation = Quaternion.Slerp(doorRotage.rotation, defaultRotageDoor, rotageCurve.Evaluate(timeRotage));
            window.rotation = Quaternion.Slerp(window.rotation, defaultRotageWindow, rotageCurve.Evaluate(timeRotage));
            timeRotage += Time.deltaTime;
        }
    }
    public void SleepEnd() { }
    public void FreeTimeEnter()
    {
        CommandFreeTime();
        timeRotage = 0f;
    }
    public void FreeTimeExecute()
    {
        if (timeRotage <= rotageCurve.keys[rotageCurve.length - 1].time)
        {
            doorRotage.rotation = Quaternion.Slerp(doorRotage.rotation, rotageDoor, rotageCurve.Evaluate(timeRotage));
            window.rotation = Quaternion.Slerp(window.rotation, rotageWindow, rotageCurve.Evaluate(timeRotage));
            timeRotage += Time.deltaTime;
        }
    }
    public void FreeTimeEnd() { }
    public void WorkTimeEnter() {
        CommandSleepDone();
        currentPointTalkIndex = 0;
    }
    public void WorkTimeExecute() { }
    public void WorkTimeEnd() { }
    #endregion
    int GetLevelOfTV() {
        return roomSetting.GetLevelOfModelPosition(HouseModelType.House_Bed.ToString());
    }
    public bool CheckBehaviorWatchTV() {
        return GetLevelOfTV() > 1 && sofaInRoom.GetSofaAble();
    }
    public Vector3 GetPointTalking(int index) {
        return pointTalkings[index].position;
    }
    public int GetCurrentPointIndex() { return currentPointTalkIndex; }
    public void IncreaseCurrentPointIndex() { currentPointTalkIndex++; }
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
        go.FreeTimeEnter();
    }
    public override void Execute(House go)
    {
        go.FreeTimeExecute();
    }
    public override void End(House go)
    {
        go.FreeTimeEnd();
    }
}
public class HouseWorkTime : State<House>
{
    private static HouseWorkTime m_Instance;
    public static HouseWorkTime instance
    {
        get
        {
            if (m_Instance == null)
                m_Instance = new HouseWorkTime();
            return m_Instance;
        }
    }
    public override void Enter(House go)
    {
        go.WorkTimeEnter();
    }
    public override void Execute(House go)
    {
        go.WorkTimeExecute();
    }
    public override void End(House go)
    {
        go.WorkTimeEnd();
    }
}