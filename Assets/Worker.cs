using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;

public class Worker : MonoBehaviour
{
    protected StateMachine<Worker> m_Statemachine;
    public StateMachine<Worker> stateMachine { get { return m_Statemachine; } }

    private void Awake()
    {
        m_Statemachine = new StateMachine<Worker>(this);
        m_Statemachine.SetcurrentState(WorkerIdle.instance);
        m_Statemachine.SetcurrentState(WorkerIdle.instance);
        anim = GetComponent<Animator>();
    }
    public WorkerState state;
    public float workTime;
    Animator anim;
    WorkerState currentState;
    public bool able;
    public bool work;
    public NavMeshAgent agent;
    public Vector3 target;
    WaitingPoint waitingPoint;
    House myHouse;
    UnityAction actionMoveDone;
    UnityAction actionWorkDone;
    public float timeAbleDefault = .25f;
    float timeAble = .25f;
    private void Update()
    {
        stateMachine.Update();
        switch (state)
        {
            case WorkerState.Idle:
                if (currentState != state)
                {
                    currentState = state;
                    stateMachine.ChangeState(WorkerIdle.instance);
                }
                break;
            case WorkerState.Move:
                if (currentState != state)
                {
                    currentState = state;
                    stateMachine.ChangeState(WorkerMove.instance);
                }
                break;
            case WorkerState.Rotage:
                if (currentState != state)
                {
                    currentState = state;
                    stateMachine.ChangeState(WorkerRotage.instance);
                }
                break;
            case WorkerState.Work:
                if (currentState != state)
                {
                    currentState = state;
                    stateMachine.ChangeState(WorkerWork.instance);
                }
                break;
            default:
                break;
        }
    }
    public void IdleEnter() {
        anim.Play("Idle");
    }
    public void IdleExecute() { }
    public void IdleEnd() {
    }
    public void MoveEnter() {
        
        agent.isStopped = false;
        agent.SetDestination(target);
        if (work)
        {
            anim.Play("Running");
            agent.speed = 1f;
        }
        else { 
            agent.speed = .5f;
            anim.Play("Walking");
        }
    }
    public void MoveExecute() {
        if (IsFinishMoveOnNavemesh())
            state = WorkerState.Rotage;
    }
    public void MoveEnd() { }
    public AnimationCurve rotageCurve;
    public Transform targetRotage;
    float timeRotage;
    Quaternion rotageFrom;
    Quaternion rotageTo;
    public void RotageEnter() {
        timeRotage = 0;
        rotageFrom = transform.rotation;
        Vector3 transFormNow = transform.position;
        transFormNow.y = 0;
        Vector3 targetXZ = targetRotage.position;
        targetXZ.y = 0;
        rotageTo = Quaternion.LookRotation((targetXZ - transFormNow), Vector3.up);
    }
    public void RotageExecute() {
        if (timeRotage <= rotageCurve.keys[rotageCurve.length - 1].time)
        {
            transform.rotation = Quaternion.Slerp(rotageFrom, rotageTo, rotageCurve.Evaluate(timeRotage));
            timeRotage += Time.deltaTime;
            return;
        }
        state = WorkerState.Idle;
    }
    public void RotageEnd() {
        agent.isStopped = true;
        if (actionMoveDone != null)
        {
            actionMoveDone();
            actionMoveDone = null;
        }
    }
    public void ChangeRotage(Quaternion rotageToChange) { }
    public void WorkEnter() {
        anim.Play("Work");
        StopAllCoroutines();
        StartCoroutine(IE_WaitWorkEnd());
    }
    IEnumerator IE_WaitWorkEnd() {
        yield return new WaitForSeconds(workTime);
        state = WorkerState.Idle;
    }
    public void WorkExecute() { }
    public void WorkEnd() {
        if (actionWorkDone != null)
        {
            actionWorkDone();
            actionWorkDone = null;
        }
    }
    public void ChangeState(WorkerState stateChange) {
        state = stateChange;
    }
    public void ChangeTarget(Vector3 targetChange, Transform monitorTransform, UnityAction actionMoveDoneChange = null, UnityAction actionWorkDoneChange = null) {
        target = targetChange;
        targetRotage = monitorTransform;
        actionMoveDone = actionMoveDoneChange;
        actionWorkDone = actionWorkDoneChange;
        ChangeState(WorkerState.Move);
    }
    public void ChangeWaitingPoint(WaitingPoint waitingPointChange = null) {
        if (waitingPointChange == null)
        {
            if (waitingPoint != null) waitingPoint.able = true;
            return;
        }
        waitingPoint = waitingPointChange;
    }
    public void ChangeHouse(House house) {
        myHouse = house;
    }
    public House GetHouse() {
        return myHouse;
    }
    public void ResetWorker() {
        actionWorkDone = null;
        actionMoveDone = null;
        StartCoroutine(IE_ResetAble(timeAble));
    }
    IEnumerator IE_ResetAble(float timeAble) {
        yield return new WaitForSeconds(timeAble);
        able = true;
    }
    public void ChangeTimeAble(float timeAbleChange = 0f) {
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
public class WorkerIdle : State<Worker> {
    private static WorkerIdle m_Instance;
    public static WorkerIdle instance
    {
        get {
            if (m_Instance == null)
                m_Instance = new WorkerIdle();
            return m_Instance;
        }
    }
    public override void Enter(Worker go)
    {
        go.IdleEnter();
    }
    public override void End(Worker go)
    {
        go.IdleEnd();
    }
    public override void Execute(Worker go) {
        go.IdleExecute();
    }
}
public class WorkerMove : State<Worker>
{
    private static WorkerMove m_Instance;
    public static WorkerMove instance
    {
        get
        {
            if (m_Instance == null)
                m_Instance = new WorkerMove();
            return m_Instance;
        }
    }
    public override void Enter(Worker go)
    {
        go.MoveEnter();
    }
    public override void End(Worker go)
    {
        go.MoveEnd();
    }
    public override void Execute(Worker go)
    {
        go.MoveExecute();
    }
}
public class WorkerRotage : State<Worker>
{
    private static WorkerRotage m_Instance;
    public static WorkerRotage instance
    {
        get
        {
            if (m_Instance == null)
                m_Instance = new WorkerRotage();
            return m_Instance;
        }
    }
    public override void Enter(Worker go)
    {
        go.RotageEnter();
    }
    public override void End(Worker go)
    {
        go.RotageEnd();
    }
    public override void Execute(Worker go)
    {
        go.RotageExecute();
    }
}
public class WorkerWork : State<Worker>
{
    private static WorkerWork m_Instance;
    public static WorkerWork instance
    {
        get
        {
            if (m_Instance == null)
                m_Instance = new WorkerWork();
            return m_Instance;
        }
    }
    public override void Enter(Worker go)
    {
        go.WorkEnter();
    }
    public override void End(Worker go)
    {
        go.WorkEnd();
    }
    public override void Execute(Worker go)
    {
        go.WorkExecute();
    }
}
