using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.AI;

public class StaffBase : MonoBehaviour
{
    protected StateMachine<StaffBase> m_Statemachine;
    public StateMachine<StaffBase> stateMachine { get { return m_Statemachine; } }
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
    Vector3 target;
    public WaitingPoint waitingPoint;
    UnityAction actionMoveDone;
    UnityAction actionWorkDone;
    [Header("ROTAGE")]
    public AnimationCurve rotageCurve;
    public Transform targetRotage;
    float timeRotage;
    Quaternion rotageFrom;
    Quaternion rotageTo;
    private void Awake()
    {
        m_Statemachine = new StateMachine<StaffBase>(this);
        m_Statemachine.SetcurrentState(StaffIdle.instance);
        m_Statemachine.SetcurrentState(StaffIdle.instance);
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
                    stateMachine.ChangeState(StaffIdle.instance);
                }
                break;
            case StaffState.Move:
                if (currentState != state)
                {
                    currentState = state;
                    stateMachine.ChangeState(StaffMove.instance);
                }
                break;
            case StaffState.Rotage:
                if (currentState != state)
                {
                    currentState = state;
                    stateMachine.ChangeState(StaffRotage.instance);
                }
                break;
            case StaffState.Work:
                if (currentState != state)
                {
                    currentState = state;
                    stateMachine.ChangeState(StaffWork.instance);
                }
                break;
            default:
                break;
        }
    }
    public virtual void StaffIdleEnter() {
        anim.Play("Idle");
    }
    public virtual void StaffIdleExecute() { }
    public virtual void StaffIdleEnd() { }
    public virtual void StaffMoveEnter() {
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
}
public class StaffIdle : State<StaffBase>
{
    private static StaffIdle m_Instance;
    public static StaffIdle instance
    {
        get
        {
            if (m_Instance == null)
                m_Instance = new StaffIdle();
            return m_Instance;
        }
    }
    public override void Enter(StaffBase go)
    {
        go.StaffIdleEnter();
    }
    public override void End(StaffBase go)
    {
        go.StaffIdleExecute();
    }
    public override void Execute(StaffBase go)
    {
        go.StaffIdleEnd();
    }
}
public class StaffMove : State<StaffBase>
{
    private static StaffMove m_Instance;
    public static StaffMove instance
    {
        get
        {
            if (m_Instance == null)
                m_Instance = new StaffMove();
            return m_Instance;
        }
    }
    public override void Enter(StaffBase go)
    {
        go.StaffMoveEnter();
    }
    public override void End(StaffBase go)
    {
        go.StaffMoveEnd();
    }
    public override void Execute(StaffBase go)
    {
        go.StaffMoveExecute();
    }
}
public class StaffRotage : State<StaffBase>
{
    private static StaffRotage m_Instance;
    public static StaffRotage instance
    {
        get
        {
            if (m_Instance == null)
                m_Instance = new StaffRotage();
            return m_Instance;
        }
    }
    public override void Enter(StaffBase go)
    {
        go.StaffRotageEnter();
    }
    public override void End(StaffBase go)
    {
        go.StaffRotageEnd();
    }
    public override void Execute(StaffBase go)
    {
        go.StaffRotageExecute();
    }
}
public class StaffWork : State<StaffBase>
{
    private static StaffWork m_Instance;
    public static StaffWork instance
    {
        get
        {
            if (m_Instance == null)
                m_Instance = new StaffWork();
            return m_Instance;
        }
    }
    public override void Enter(StaffBase go)
    {
        go.StaffWorkEnter();
    }
    public override void End(StaffBase go)
    {
        go.StaffWorkEnd();
    }
    public override void Execute(StaffBase go)
    {
        go.StaffWorkExecute();
    }
}
