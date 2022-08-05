using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TakeOffRoom : BaseRoom<TakeOffModelType>
{
    protected StateMachine<TakeOffRoom> m_StateMachine;
    public StateMachine<TakeOffRoom> stateMachine { get { return m_StateMachine; } }
    [Header("VEHICLECARRY")]
    [Header("==============TAKEOFF ROOM============")]
    public List<VehicleCarry> vehicleCarries;
    VehicleCarry vehicleOnProgrees;
    [Header("STATE")]
    public TakeOffRoomState state;
    TakeOffRoomState currentState;
    [Header("TRANSFORM")]
    public Transform elevatorTransform;
    public Transform pointVehicleCarryDrop;
    public Transform pointElevatorTake;
    public Transform pointElevatorDrop;
    public Transform pointStartFly;
    Transform vehicleBroke;
    [Header("CURVE")]
    public AnimationCurve animCurve;
    float timeCurve;
    Vector3 startPoint;
    Vector3 endPoint;
    public bool able;
    private void Awake()
    {
        m_StateMachine = new StateMachine<TakeOffRoom>(this);
        m_StateMachine.SetcurrentState(Idle.instance);
        m_StateMachine.ChangeState(Idle.instance);
    }
    private void Update()
    {
        stateMachine.Update();
        switch (state)
        {
            case TakeOffRoomState.Idle:
                if (currentState != state)
                {
                    currentState = state;
                    stateMachine.ChangeState(Idle.instance);
                }
                break;
            case TakeOffRoomState.GetBrokeVehicle:
                if (currentState != state)
                {
                    currentState = state;
                    stateMachine.ChangeState(ChangeBrokeVehicle.instance);
                }
                break;
            case TakeOffRoomState.MoveVehicle:
                if (currentState != state)
                {
                    currentState = state;
                    stateMachine.ChangeState(MoveVehicle.instance);
                }
                break;
            case TakeOffRoomState.Drop:
                if (currentState != state)
                {
                    currentState = state;
                    stateMachine.ChangeState(DropVehicle.instance);
                }
                break;
            case TakeOffRoomState.TakeOff:
                if (currentState != state)
                {
                    currentState = state;
                    stateMachine.ChangeState(Idle.instance);
                }
                break;
            default:
                break;
        }
        if (vehicleCarries.Count > 0)
        {
            if (able)
            {
                vehicleCarries[0].ChangeRotageTo(Quaternion.LookRotation(-Vector3.right, Vector3.up));
                vehicleCarries[0].ChangeTargetMove(pointVehicleCarryDrop.position, GetBrokeVehicle);
                vehicleOnProgrees = vehicleCarries[0];
                RemoveVehicleCarry(vehicleCarries[0]);
                vehicleOnProgrees.ChangeWaitingPoint();
                able = false;
                return;
            }

            for (int i = 0; i < vehicleCarries.Count; i++) { 
                if (vehicleCarries[i].myWaitingPoint == null)
                {
                    vehicleCarries[i].ChangeRotageTo(Quaternion.LookRotation(-Vector3.right, Vector3.up));
                    vehicleCarries[i].ChangeTargetMove(waitingPoints[i].point.position);
                    vehicleCarries[i].ChangeWaitingPoint(waitingPoints[i]);
                }
            }
        }
    }
    void GetBrokeVehicle() { 
        state = TakeOffRoomState.GetBrokeVehicle;
    }
    public void AddVehicleCarry(VehicleCarry vehicleCarry) { vehicleCarries.Add(vehicleCarry); }
    void RemoveVehicleCarry(VehicleCarry vehicleCarry) { vehicleCarries.Remove(vehicleCarry); }
    public bool CheckAbleWaittingPoint() {
        foreach (WaitingPoint waitingPoint in waitingPoints)
        {
            if (waitingPoint.able)
                return true;
        }
        return false;
    }
    public bool CheckAble() {
        return able;
    }
    public void IdleEnter() { }
    public void IdleExecute() { }
    public void IdleEnd() { }
    
    public void ChangeBrokeEnter() {
        timeCurve = 0;
        startPoint = elevatorTransform.position;
        endPoint = pointElevatorTake.position;
    }
    public void ChangeBrokeExecute() {
        if (timeCurve <= animCurve.keys[animCurve.length - 1].time)
        {
            elevatorTransform.position = Vector3.Lerp(startPoint, endPoint, animCurve.Evaluate(timeCurve));
            timeCurve += Time.deltaTime;
            return;
        }
        vehicleBroke = vehicleOnProgrees.vehicleTake;
        vehicleBroke.parent = elevatorTransform;
        state = TakeOffRoomState.MoveVehicle;
    }
    public void ChangeBrokeEnd() { }

    public void MoveVehicleEnter() {
        timeCurve = 0;
        startPoint = vehicleBroke.position;
        endPoint = elevatorTransform.position;
    }
    public void MoveVehicleExecute() {
        if (timeCurve <= animCurve.keys[animCurve.length - 1].time)
        {
            vehicleBroke.position = Vector3.Lerp(startPoint, endPoint, animCurve.Evaluate(timeCurve));
            timeCurve += Time.deltaTime;
            return;
        }
        state = TakeOffRoomState.Drop;
    }
    public void MoveVehicleEnd() {
        
    }

    public void DropEnter() {
        timeCurve = 0;
        startPoint = elevatorTransform.position;
        endPoint = pointElevatorDrop.position;
        WaitingPoint waitingPoint = GameManager.instance.carryVehicleCarryManager.GetCarryRoom().GetWaitingPoint();
        vehicleOnProgrees.ChangeTargetMove(waitingPoint.point.position, vehicleOnProgrees.ResetAble);
        vehicleOnProgrees.ChangeRotageTo(Quaternion.LookRotation(Vector3.right, Vector3.up));
        vehicleOnProgrees.ChangeWaitingPoint(waitingPoint);
        vehicleOnProgrees = null;
    }
    public void DropExecute() {
        if (timeCurve <= animCurve.keys[animCurve.length - 1].time)
        {
            elevatorTransform.position = Vector3.Lerp(startPoint, endPoint, animCurve.Evaluate(timeCurve));
            timeCurve += Time.deltaTime * 0.5f;
            return;
        }
        vehicleBroke.parent = null;
        VehicleBroke vehicleBrokeCompo = vehicleBroke.GetComponent<VehicleBroke>();
        vehicleBrokeCompo.myTakeOffRoom = this;
        vehicleBrokeCompo.vehicleState = VehicleState.TakeOff;
        state = TakeOffRoomState.Idle;
    }
    public void DropEnd() {
        able = true;
        vehicleBroke = null;
    }
}
public class Idle : State<TakeOffRoom> {
    private static Idle m_Instance;
    public static Idle instance
    {
        get {
            if (m_Instance == null)
                m_Instance = new Idle();
            return m_Instance;
        }
    }
    public override void Enter(TakeOffRoom go)
    {
        go.IdleEnter();
    }
    public override void Execute(TakeOffRoom go)
    {
        go.IdleExecute();
    }
    public override void End(TakeOffRoom go)
    {
        go.IdleEnd();
    }
}
public class ChangeBrokeVehicle : State<TakeOffRoom>
{
    private static ChangeBrokeVehicle m_Instance;
    public static ChangeBrokeVehicle instance
    {
        get
        {
            if (m_Instance == null)
                m_Instance = new ChangeBrokeVehicle();
            return m_Instance;
        }
    }
    public override void Enter(TakeOffRoom go)
    {
        go.ChangeBrokeEnter();
    }
    public override void Execute(TakeOffRoom go)
    {
        go.ChangeBrokeExecute();
    }
    public override void End(TakeOffRoom go)
    {
        go.ChangeBrokeEnd();
    }
}
public class MoveVehicle : State<TakeOffRoom>
{
    private static MoveVehicle m_Instance;
    public static MoveVehicle instance
    {
        get
        {
            if (m_Instance == null)
                m_Instance = new MoveVehicle();
            return m_Instance;
        }
    }
    public override void Enter(TakeOffRoom go)
    {
        go.MoveVehicleEnter();
    }
    public override void Execute(TakeOffRoom go)
    {
        go.MoveVehicleExecute();
    }
    public override void End(TakeOffRoom go)
    {
        go.MoveVehicleEnd();
    }
}
public class DropVehicle : State<TakeOffRoom>
{
    private static DropVehicle m_Instance;
    public static DropVehicle instance
    {
        get
        {
            if (m_Instance == null)
                m_Instance = new DropVehicle();
            return m_Instance;
        }
    }
    public override void Enter(TakeOffRoom go)
    {
        go.DropEnter();
    }
    public override void Execute(TakeOffRoom go)
    {
        go.DropExecute();
    }
    public override void End(TakeOffRoom go)
    {
        go.DropEnd();
    }
}
