using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LandingPad : BaseRoom<LandingPadModelType>
{
    protected StateMachine<LandingPad> m_Statemachine;
    public StateMachine<LandingPad> stateMachine { get { return m_Statemachine; } }
    private void Awake()
    {
        m_Statemachine = new StateMachine<LandingPad>(this);
        m_Statemachine.SetcurrentState(LPadIdle.instance);
        m_Statemachine.ChangeState(LPadIdle.instance);
    }
   
    [Header("TRANSFORM & GAME OBJ")]
    [Header("===========================LANDING ROOM==================================")]
    public Transform landingStartPosition;
    public Transform landingEndPosition;
    public Transform playerHandlePoint;
    public Transform monitorTransform;
    public GameObject elevator;
    [Header("STATE")]
    public LandingPadState state;
    LandingPadState currentState;
    [Header("OTHER VARIABLE")]
    public float elevatorSpeed;
    public bool able = true;
    [SerializeField] Vector3 elevatorOffset;
    VehicleCarry vehicleCarry;
    VehicleBroke vehicleBroke;
    Worker workerAble;
    private void Update()
    {
        stateMachine.Update();
        switch (state)
        {
            case LandingPadState.Idle:
                if (currentState != state)
                {
                    currentState = state;
                    stateMachine.ChangeState(LPadIdle.instance);
                }
                break;
            case LandingPadState.LaunchPadCall:
                if (currentState != state)
                {
                    currentState = state;
                    stateMachine.ChangeState(LaunchPadCall.instance);
                }
                break;
            case LandingPadState.CallWorker:
                if (currentState != state)
                {
                    currentState = state;
                    stateMachine.ChangeState(CallWorker.instance);
                }
                break;
            case LandingPadState.ElevatorDown:
                if (currentState != state)
                {
                    currentState = state;
                    stateMachine.ChangeState(LaunchPadElevatorDown.instance);
                }
                break;
            default:
                break;
        }
    }
    public void LPIdleEnter() { }
    public void LPIdleExecute() { }
    public void LPIdleEnd() { }
    
    public void CallWorkerEnter() { }
    public void CallWorkerExecute() {
        if (workerAble == null) {
            workerAble = GameManager.instance.workerManager.GetWorker();           
            return;
        }
        workerAble.work = true;
        workerAble.ChangeTarget(playerHandlePoint.position, monitorTransform, WorkerWork, CallLanchPad);
        workerAble.ChangeWaitingPoint();
        state = LandingPadState.Idle;
    }
    public void CallWorkerEnd() { }
    void WorkerWork() {
        workerAble.ChangeState(StaffState.Work);
    }
    void CallLanchPad() {
        state = LandingPadState.LaunchPadCall;
    }
    Vector3 elevatorStartPoint;
    public void LaunchPadCallEnter()
    {
        elevatorStartPoint = elevator.transform.position;
    }
    
    public void LaunchPadCallExecute()
    {

        if (vehicleCarry == null)
        {
            vehicleCarry = GameManager.instance.carryVehicleCarryManager.GetAbleVehicle();
            return;
        }
        elevator.transform.Translate((elevatorStartPoint + elevatorOffset - elevator.transform.position) * elevatorSpeed * Time.deltaTime);
        if (Vector3.Distance(elevator.transform.position, elevatorStartPoint + elevatorOffset) <= 0.01f)
        {
            state = LandingPadState.Idle;
            vehicleBroke.myLanchPad.currentLanding = this;
            vehicleBroke.myLanchPad.ChangeState(LanchPadState.PickUp);
        }
    }
    public void ElevatorDownEnter()
    {
        vehicleCarry = null;
        elevatorStartPoint = elevator.transform.position;
        StopAllCoroutines();
        StartCoroutine(IEWaitToCallCarry());
        WaitingPoint waitingPoint = GameManager.instance.workerManager.GetWaitingPoint(workerAble.GetHouse());
        workerAble.ChangeWaitingPoint(waitingPoint);
        workerAble.work = false;
        workerAble.ChangeTimeAble(0);
        workerAble.ChangeTarget(waitingPoint.point.position - new Vector3(0.01f, 0, 0), waitingPoint.point, workerAble.ResetWorker);
        
        workerAble = null;
    }
    IEnumerator IEWaitToCallCarry()
    {
        yield return new WaitForSeconds(.25f);
        vehicleBroke.myLanchPad.ChangeState(LanchPadState.CallCarry);
    }
    public void ElevatorDownExecute()
    {
        elevator.transform.Translate((elevatorStartPoint - elevatorOffset - elevator.transform.position) * elevatorSpeed * Time.deltaTime);
        if (Vector3.Distance(elevator.transform.position, elevatorStartPoint - elevatorOffset) <= 0.01f)
        {
            state = LandingPadState.Idle;
        }
    }
    public void ElevatorDownEnd() { }
    public void LaunchPadCallEnd() { }
    public void ChangeState(LandingPadState stateChange)
    {
        state = stateChange;
    }
    public void ChangeVehicle(VehicleBroke vehicleBrokeChange)
    {
        vehicleBroke = vehicleBrokeChange;
    }
}
public class LPadIdle : State<LandingPad>
{
    private static LPadIdle Instance;
    public static LPadIdle instance
    {
        get
        {
            if (Instance == null)
                Instance = new LPadIdle();
            return Instance;
        }
    }
    public override void Enter(LandingPad go)
    {
        go.LPIdleEnter();
    }
    public override void Execute(LandingPad go)
    {
        go.LPIdleExecute();
    }
    public override void End(LandingPad go)
    {
        go.LPIdleEnd();
    }

}
public class CallWorker : State<LandingPad>
{
    private static CallWorker Instance;
    public static CallWorker instance
    {
        get
        {
            if (Instance == null)
                Instance = new CallWorker();
            return Instance;
        }
    }
    public override void Enter(LandingPad go)
    {
        go.CallWorkerEnter();
    }
    public override void Execute(LandingPad go)
    {
        go.CallWorkerExecute();
    }
    public override void End(LandingPad go)
    {
        go.CallWorkerEnd();
    }
}
public class LaunchPadCall : State<LandingPad>
{
    private static LaunchPadCall Instance;
    public static LaunchPadCall instance
    {
        get
        {
            if (Instance == null)
                Instance = new LaunchPadCall();
            return Instance;
        }
    }
    public override void Enter(LandingPad go)
    {
        go.LaunchPadCallEnter();
    }
    public override void Execute(LandingPad go)
    {
        go.LaunchPadCallExecute();
    }
    public override void End(LandingPad go)
    {
        go.LaunchPadCallEnd();
    }
}
public class LaunchPadElevatorDown : State<LandingPad>
{
    private static LaunchPadElevatorDown Instance;
    public static LaunchPadElevatorDown instance
    {
        get
        {
            if (Instance == null)
                Instance = new LaunchPadElevatorDown();
            return Instance;
        }
    }
    public override void Enter(LandingPad go)
    {
        go.ElevatorDownEnter();
    }
    public override void Execute(LandingPad go)
    {
        go.ElevatorDownExecute();
    }
    public override void End(LandingPad go)
    {
        go.ElevatorDownEnd();
    }
}
