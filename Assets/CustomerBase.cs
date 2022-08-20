using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;

public class CustomerBase : MonoBehaviour
{
    protected static StateMachine<CustomerBase> m_Statemachine;
    public static StateMachine<CustomerBase> stateMachine { get { return m_Statemachine; } }
    [HideInInspector]
    public Animator anim;
    public CustomerState state;
    CustomerState currentState = CustomerState.Idle;
    [HideInInspector]
    public Vector3 target;
    [HideInInspector]
    public NavMeshAgent agent;
    [HideInInspector]
    public LandingPad myLandingPad;
    UnityAction actionDone;
    UnityAction<CustomerState> actionChangeState;
    CustomerState stateChange;
    float timeRotage = 0f;
    Quaternion rotageFrom;
    Quaternion rotageTo;
    Vector3 targetRotageTo;
    public AnimationCurve rotageCurve;
    private void Awake()
    {
        anim = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();
        if (m_Statemachine == null)
        {
            m_Statemachine = new StateMachine<CustomerBase>(this);
            m_Statemachine.ChangeState(CustomerOnVehicle.instance);
            m_Statemachine.SetcurrentState(CustomerOnVehicle.instance);
        }
    }
    public virtual void Update() {
        stateMachine.Update();
        switch (state)
        {
            case CustomerState.OnVehicle:
                if (state!= currentState)
                {
                    currentState = state;
                    stateMachine.ChangeState(CustomerOnVehicle.instance);
                }
                break;
            case CustomerState.Idle:
                if (state != currentState)
                {
                    currentState = state;
                    stateMachine.ChangeState(CustomerIdle.instance);
                }
                break;
            case CustomerState.Move:
                if (state != currentState)
                {
                    currentState = state;
                    stateMachine.ChangeState(CustomerMove.instance);
                }
                break;
            case CustomerState.Talk:
                if (state != currentState)
                {
                    currentState = state;
                    stateMachine.ChangeState(CustomerTalk.instance);
                }
                break;
            case CustomerState.Rotage:
                if (state != currentState)
                {
                    currentState = state;
                    stateMachine.ChangeState(CustomerRotage.instance);
                }
                break;
            default:
                break;
        }
    }
    public virtual void OnVehicleEnter() {
        agent.enabled = false;
        anim.Play("Drive"); 
    }
    public virtual void OnVehicleExecute() { }
    public virtual void OnVehicleEnd() { }
    public virtual void CustomerIdleEnter() {
        agent.enabled = true;
        anim.Play("Idle"); }
    public virtual void CustomerIdleExecute() { }
    public virtual void CustomerIdleEnd() { }
    public virtual void CustomerMoveEnter() { 
        anim.Play("Walking");
        agent.enabled = true;
        agent.isStopped = false;
        agent.SetDestination(target);
        agent.speed = .3f;
    }
    public virtual void CustomerMoveExecute() {
        if (IsFinishMoveOnNavemesh())
        {
            agent.isStopped = true;
            ChangeState(CustomerState.Rotage);
        }
    }
    public virtual void CustomerMoveEnd() { }
    public virtual void CustomerTalkingEnter() { anim.Play("Talking"); }
    public virtual void CustomerTalkingExecute() { }
    public virtual void CustomerTalkingEnd() { }
    public virtual void CustomerRotageEnter() {
        timeRotage = 0;
        rotageFrom = transform.rotation;
        Vector3 transFormNow = transform.position;
        transFormNow.y = 0;
        Vector3 targetXZ = targetRotageTo;
        targetXZ.y = 0;
        rotageTo = Quaternion.LookRotation((targetXZ - transFormNow), Vector3.up);
    }
    public virtual void CustomerRotageExecute() {
        if (timeRotage <= rotageCurve.keys[rotageCurve.length - 1].time)
        {
            transform.rotation = Quaternion.Slerp(rotageFrom, rotageTo, rotageCurve.Evaluate(timeRotage));
            timeRotage += Time.deltaTime;
            return;
        }
        
        if (actionDone != null)
        {
            actionDone();
            return;
        }
        if (actionChangeState != null)
        {
            actionChangeState(stateChange);
            return;
        }
        ChangeState(CustomerState.Idle);
    }
    public virtual void CustomerRotageEnd() { }
    public void ChangeState(CustomerState stateChange) { state = stateChange; }
    public void ChangeTarget(Vector3 targetChange, 
        UnityAction actionDoneChange = null, 
        UnityAction<CustomerState> actionChangeStateChange = null, 
        CustomerState state = CustomerState.Idle) 
    { 
        target = targetChange; 
        actionDone = actionDoneChange;
        actionChangeState = actionChangeStateChange;
        stateChange = state;
        ChangeState(CustomerState.Move);
    }
    public void ChangeRotageTarget(Vector3 pointChange) { targetRotageTo = pointChange; }
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
}
public class CustomerOnVehicle : State<CustomerBase> {
    private static CustomerOnVehicle m_Instance;
    public static CustomerOnVehicle instance
    {
        get {
            if (m_Instance == null)
                m_Instance = new CustomerOnVehicle();
            return m_Instance;
        }
    }
    public override void Enter(CustomerBase go)
    {
        go.OnVehicleEnter();
    }
    public override void Execute(CustomerBase go)
    {
        go.OnVehicleExecute();
    }
    public override void End(CustomerBase go)
    {
        go.OnVehicleEnd();
    }
}
public class CustomerIdle : State<CustomerBase>
{
    private static CustomerIdle m_Instance;
    public static CustomerIdle instance
    {
        get
        {
            if (m_Instance == null)
                m_Instance = new CustomerIdle();
            return m_Instance;
        }
    }
    public override void Enter(CustomerBase go)
    {
        go.CustomerIdleEnter();
    }
    public override void Execute(CustomerBase go)
    {
        go.CustomerIdleExecute();
    }
    public override void End(CustomerBase go)
    {
        go.CustomerIdleEnd();
    }
}
public class CustomerMove : State<CustomerBase>
{
    private static CustomerMove m_Instance;
    public static CustomerMove instance
    {
        get
        {
            if (m_Instance == null)
                m_Instance = new CustomerMove();
            return m_Instance;
        }
    }
    public override void Enter(CustomerBase go)
    {
        go.CustomerMoveEnter();
    }
    public override void Execute(CustomerBase go)
    {
        go.CustomerMoveExecute();
    }
    public override void End(CustomerBase go)
    {
        go.CustomerMoveEnd();
    }
}
public class CustomerTalk : State<CustomerBase>
{
    private static CustomerTalk m_Instance;
    public static CustomerTalk instance
    {
        get
        {
            if (m_Instance == null)
                m_Instance = new CustomerTalk();
            return m_Instance;
        }
    }
    public override void Enter(CustomerBase go)
    {
        go.CustomerTalkingEnter();
    }
    public override void Execute(CustomerBase go)
    {
        go.CustomerTalkingExecute();
    }
    public override void End(CustomerBase go)
    {
        go.CustomerTalkingEnd();
    }
}
public class CustomerRotage : State<CustomerBase>
{
    private static CustomerRotage m_Instance;
    public static CustomerRotage instance
    {
        get
        {
            if (m_Instance == null)
                m_Instance = new CustomerRotage();
            return m_Instance;
        }
    }
    public override void Enter(CustomerBase go)
    {
        go.CustomerRotageEnter();
    }
    public override void Execute(CustomerBase go)
    {
        go.CustomerRotageExecute();
    }
    public override void End(CustomerBase go)
    {
        go.CustomerRotageEnd();
    }
}