using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.AI;

public class StaffBase<T> : MonoBehaviour
{
    [Header("ANIM AND STATE")]
    [Header("====================BASE STAFF========================")]
    public Animator anim;
    public StaffState state;
    public StaffState currentState;
    [Header("STAFF CHECK")]
    public bool able;
    public bool work;
    public float workTime;
    public float timeAble = .25f;
    float timeAbleDefault = .25f;
    [Header("NAVMESH")]
    public NavMeshAgent agent;
    public Vector3 target;
    public WaitingPoint waitingPoint;
    public UnityAction actionMoveDone;
    public UnityAction actionWorkDone;
    [Header("ROTAGE")]
    public AnimationCurve rotageCurve;
    public Transform targetRotage;
    public Quaternion sleepRotage;
    float timeRotage;
    Quaternion rotageFrom;
    Quaternion rotageTo;
    #region DATA
    [Header("DATA")]
    public StaffSetting<T> staffSetting;
    public StaffDataAsset<T> staffDataAsset;
    [Header("Behavior")]
    public bool sleepTime;
    public bool freeTime;
    public bool eatTime;
    protected StateMachine<StaffBase<T>> m_Statemachine;
    public StateMachine<StaffBase<T>> stateMachine { get { return m_Statemachine; } }
    private void Awake()
    {
        m_Statemachine = new StateMachine<StaffBase<T>>(this);
        m_Statemachine.SetcurrentState(StaffIdle<T>.instance);
        m_Statemachine.SetcurrentState(StaffIdle<T>.instance);
        anim = GetComponent<Animator>();
    }
    private void Update()
    {
        stateMachine.Update();
        switch (state)
        {
            case StaffState.Idle:
                if (currentState != state)
                {
                    currentState = state;
                    stateMachine.ChangeState(StaffIdle<T>.instance);
                }
                break;
            case StaffState.Move:
                if (currentState != state)
                {
                    currentState = state;
                    stateMachine.ChangeState(StaffMove<T>.instance);
                }
                break;
            case StaffState.Rotage:
                if (currentState != state)
                {
                    currentState = state;
                    stateMachine.ChangeState(StaffRotage<T>.instance);
                }
                break;
            case StaffState.Work:
                if (currentState != state)
                {
                    currentState = state;
                    stateMachine.ChangeState(StaffWork<T>.instance);
                }
                break;
            case StaffState.Sleep:
                if (currentState != state)
                {
                    currentState = state;
                    stateMachine.ChangeState(StaffSleep<T>.instance);
                }
                break;
            case StaffState.FreeTime:
                if (currentState != state)
                {
                    currentState = state;
                    stateMachine.ChangeState(StaffFreeTime<T>.instance);
                }
                break;
            default:
                break;
        }
    }
    public void OnLoadStaff() {
        LoadFromSaveData(ProfileManager.instance.playerData.GetStaffData<T>(staffSetting.staffID, staffSetting.staffType));
    }
    void LoadFromSaveData(StaffSetting<T> staffSave) {
        for (int i = 0; i < staffSetting.staffModelsPos.Count; i++)
        {
            StaffModelPos<T> model = staffSetting.staffModelsPos[i];
            if (model.rootObject.childCount > 0)
                Destroy(model.rootObject.GetChild(0).gameObject);
            if (staffSave != null && staffSave.staffModelsPos.Count > 0)
                model.level = staffSave.staffModelsPos[i].level;
            if (model.level > 0)
            {
                Transform rootTransform = model.rootObject;
                Transform newModelCreate = Instantiate(staffDataAsset.GetModelByType(model.type.ToString(), model.level));
                newModelCreate.SetParent(rootTransform);
                newModelCreate.localPosition = Vector3.zero;
                newModelCreate.localEulerAngles = Vector3.zero;
                newModelCreate.localScale = new Vector3(1, 1, 1);
                model.currentModel = newModelCreate;
            }
        }
    }
    #endregion
    public virtual void StaffIdleEnter() {
        anim.Play("Idle");
    }
    public virtual void StaffIdleExecute() { }
    public virtual void StaffIdleEnd() { }
    public virtual void StaffMoveEnter() {
        agent.enabled = true;
        agent.isStopped = false;
        agent.SetDestination(target);
        if (work)
        {
            anim.Play("Running");
            agent.speed = 1f;
        }
        else
        {
            agent.speed = .5f;
            anim.Play("Walking");
        }
    }
    public virtual void StaffMoveExecute() {
        if (IsFinishMoveOnNavemesh())
            state = StaffState.Rotage;
    }
    public virtual void StaffMoveEnd() { }
    public virtual void StaffRotageEnter() {
        timeRotage = 0;
        rotageFrom = transform.rotation;
        Vector3 transFormNow = transform.position;
        transFormNow.y = 0;
        Vector3 targetXZ = targetRotage.position;
        targetXZ.y = 0;
        rotageTo = Quaternion.LookRotation((targetXZ - transFormNow), Vector3.up);
    }
    public virtual void StaffRotageExecute() {
        if (timeRotage <= rotageCurve.keys[rotageCurve.length - 1].time)
        {
            transform.rotation = Quaternion.Slerp(rotageFrom, rotageTo, rotageCurve.Evaluate(timeRotage));
            timeRotage += Time.deltaTime;
            return;
        }
        state = StaffState.Idle;
    }
    public virtual void StaffRotageEnd() {
        agent.isStopped = true;
        if (actionMoveDone != null)
        {
            actionMoveDone();
            actionMoveDone = null;
        }
    }
    public virtual void StaffWorkEnter() {
        anim.Play("Work");
        StopAllCoroutines();
        StartCoroutine(IE_WaitWorkEnd());
    }
    IEnumerator IE_WaitWorkEnd()
    {
        yield return new WaitForSeconds(workTime);
        state = StaffState.Idle;
    }
    public virtual void StaffWorkExecute() { }
    public virtual void StaffWorkEnd() {
        if (actionWorkDone != null)
        {
            actionWorkDone();
            actionWorkDone = null;
        }
    }
    public void ChangeState(StaffState stateChange)
    {
        state = stateChange;
    }
    public void ChangeTarget(Vector3 targetChange, Transform monitorTransform, UnityAction actionMoveDoneChange = null, UnityAction actionWorkDoneChange = null)
    {
        target = targetChange;
        targetRotage = monitorTransform;
        actionMoveDone = actionMoveDoneChange;
        actionWorkDone = actionWorkDoneChange;
        ChangeState(StaffState.Move);
    }
    public void ChangeWaitingPoint(WaitingPoint waitingPointChange = null)
    {
        if (waitingPointChange == null)
        {
            if (waitingPoint != null) waitingPoint.able = true;
            return;
        }
        waitingPoint = waitingPointChange;
    }
    public void ResetWorker()
    {
        if (!GameManager.instance.timeLineManager.workerTime)
        {
            actionWorkDone = null;
            actionMoveDone = null;
            able = false;
            return;
        }
        actionWorkDone = null;
        actionMoveDone = null;
        StartCoroutine(IE_ResetAble(timeAble));
    }
    IEnumerator IE_ResetAble(float timeAble)
    {
        yield return new WaitForSeconds(timeAble);
        able = true;
    }
    public void ChangeTimeAble(float timeAbleChange = 0f)
    {
        if (timeAbleChange < 0f)
        {
            timeAble = timeAbleDefault;
            return;
        }
        timeAble = timeAbleChange;
    }
    public bool IsFinishMoveOnNavemesh()
    {
        if (!agent.isActiveAndEnabled)
            return true;
        if (!agent.pathPending)
        {
            if (agent.remainingDistance <= agent.stoppingDistance)
            {
                if (!agent.hasPath || agent.velocity.sqrMagnitude == 0f)
                {
                    return true;
                }
            }
        }
        return false;
    }

    #region Sleep
    public virtual void StaffSleepEnter() { }
    public virtual void StaffSleepExecute() { }
    public virtual void StaffSleepEnd() { }
    #endregion

    #region FreeTime
    public virtual void StaffFreeTimeEnter() { }
    public virtual void StaffFreeTimeExecute() { }
    public virtual void StaffFreeTimeEnd() { }
    #endregion
}
public class StaffIdle<T> : State<StaffBase<T>>
{
    private static StaffIdle<T> m_Instance;
    public static StaffIdle<T> instance
    {
        get
        {
            if (m_Instance == null)
                m_Instance = new StaffIdle<T>();
            return m_Instance;
        }
    }
    public override void Enter(StaffBase<T> go)
    {
        go.StaffIdleEnter();
    }
    public override void End(StaffBase<T> go)
    {
        go.StaffIdleEnd();
    }
    public override void Execute(StaffBase<T> go)
    {
        go.StaffIdleExecute();
    }
}
public class StaffMove<T> : State<StaffBase<T>>
{
    private static StaffMove<T> m_Instance;
    public static StaffMove<T> instance
    {
        get
        {
            if (m_Instance == null)
                m_Instance = new StaffMove<T>();
            return m_Instance;
        }
    }
    public override void Enter(StaffBase<T> go)
    {
        go.StaffMoveEnter();
    }
    public override void End(StaffBase<T> go)
    {
        go.StaffMoveEnd();
    }
    public override void Execute(StaffBase<T> go)
    {
        go.StaffMoveExecute();
    }
}
public class StaffRotage<T> : State<StaffBase<T>>
{
    private static StaffRotage<T> m_Instance;
    public static StaffRotage<T> instance
    {
        get
        {
            if (m_Instance == null)
                m_Instance = new StaffRotage<T>();
            return m_Instance;
        }
    }
    public override void Enter(StaffBase<T> go)
    {
        go.StaffRotageEnter();
    }
    public override void End(StaffBase<T> go)
    {
        go.StaffRotageEnd();
    }
    public override void Execute(StaffBase<T> go)
    {
        go.StaffRotageExecute();
    }
}
public class StaffWork<T> : State<StaffBase<T>>
{
    private static StaffWork<T> m_Instance;
    public static StaffWork<T> instance
    {
        get
        {
            if (m_Instance == null)
                m_Instance = new StaffWork<T>();
            return m_Instance;
        }
    }
    public override void Enter(StaffBase<T> go)
    {
        go.StaffWorkEnter();
    }
    public override void End(StaffBase<T> go)
    {
        go.StaffWorkEnd();
    }
    public override void Execute(StaffBase<T> go)
    {
        go.StaffWorkExecute();
    }
}
public class StaffSleep<T> : State<StaffBase<T>> {
    private static StaffSleep<T> m_Instance;
    public static StaffSleep<T> instance
    {
        get {
            if (m_Instance == null)
                m_Instance = new StaffSleep<T>();
            return m_Instance;
        }
    }
    public override void Enter(StaffBase<T> go)
    {
        go.StaffSleepEnter();
    }
    public override void Execute(StaffBase<T> go)
    {
        go.StaffSleepExecute();
    }
    public override void End(StaffBase<T> go)
    {
        go.StaffSleepEnd();
    }
}
public class StaffFreeTime<T> : State<StaffBase<T>>
{
    private static StaffFreeTime<T> m_Instance;
    public static StaffFreeTime<T> instance
    {
        get
        {
            if (m_Instance == null)
                m_Instance = new StaffFreeTime<T>();
            return m_Instance;
        }
    }
    public override void Enter(StaffBase<T> go)
    {
        go.StaffFreeTimeEnter();
    }
    public override void Execute(StaffBase<T> go)
    {
        go.StaffFreeTimeExecute();
    }
    public override void End(StaffBase<T> go)
    {
        go.StaffFreeTimeEnd();
    }
}
