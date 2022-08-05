using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LanchPad : BaseRoom<LanchPadModelType>
{
    protected StateMachine<LanchPad> m_Statemachine;
    public StateMachine<LanchPad> stateMachine { get { return m_Statemachine; } }
    private void Awake()
    {
        m_Statemachine = new StateMachine<LanchPad>(this);
        m_Statemachine.SetcurrentState(LanchPadIdle.instance);
        m_Statemachine.ChangeState(LanchPadIdle.instance);
    }   
    public LanchPadState state;
    LanchPadState currentState;
    public LandingPad currentLanding;
    private void Update()
    {
        stateMachine.Update();
        switch (state)
        {
            case LanchPadState.Idle:
                if (currentState!=state)
                {
                    stateMachine.ChangeState(LanchPadIdle.instance);
                    currentState = state;
                }
                break;
            case LanchPadState.PickUp:
                if (currentState != state)
                {
                    stateMachine.ChangeState(LanchPadPickUp.instance);
                    currentState = state;
                }
                break;
            case LanchPadState.CallCarry:
                if (currentState != state)
                {
                    stateMachine.ChangeState(LanchPadCallCarryCar.instance);
                    currentState = state;
                }
                break;
            case LanchPadState.DropVehicle:
                if (currentState != state)
                {
                    stateMachine.ChangeState(LanchPadDrop.instance);
                    currentState = state;
                }
                break;
            default:
                break;
        }
    }
   
    public void LanchPadIdleEnter() { }
    public void LanchPadIdleExecute() { }
    public void LanchPadIdleEnd() { }
    public Transform loadingBayAim;
    public Transform vehicleParent;
    public Transform vehicleDropPoint;
    public AnimationCurve rotageCurve;
    public Transform vehiclePickUp;
    VehicleCarry myVehicleCarry;
    float rotageTime;
    Quaternion rotageTo;
    Quaternion rotageFrom;
    public void LanchPadPickUpEnter() {
        rotageTo = Quaternion.LookRotation(Vector3.right, Vector3.up);
        rotageFrom = loadingBayAim.rotation;
        rotageTime = 0;
    }
    public void LanchPadPickUpExecute() {
        if (rotageTime <= rotageCurve.keys[rotageCurve.length - 1].time)
        {
            loadingBayAim.rotation = Quaternion.Slerp(rotageFrom, rotageTo, rotageCurve.Evaluate(rotageTime));
            rotageTime += Time.deltaTime;
            return;
        }
        vehiclePickUp.parent = vehicleParent;
        currentLanding.ChangeState(LandingPadState.ElevatorDown);
    }
    public void LanchPadPickUpEnd() { }
    public void LanchPadCallCarryEnter() { }
    public void LanchPadCallCarryExecute() {
        myVehicleCarry = GameManager.instance.carryVehicleCarryManager.GetAbleVehicle();
        if (myVehicleCarry == null)
            return;
        myVehicleCarry.ChangeWaitingPoint();
        myVehicleCarry.ChangeRotageTo(Quaternion.LookRotation(-Vector3.right, Vector3.up));
        myVehicleCarry.ChangeTargetMove(vehicleDropPoint.position, RotageToDrop);
        state = LanchPadState.Idle;
    }
    public void LanchPadCallCarryEnd() { }
    public void LanchPadDropEnter() {
        rotageTo = Quaternion.LookRotation(-Vector3.forward, Vector3.up);
        rotageFrom = loadingBayAim.rotation;
        rotageTime = 0;
    }
    public void LanchPadDropExecute() {
        if (rotageTime <= rotageCurve.keys[rotageCurve.length - 1].time)
        {
            loadingBayAim.rotation = Quaternion.Slerp(rotageFrom, rotageTo, rotageCurve.Evaluate(rotageTime));
            rotageTime += Time.deltaTime;
            return;
        }
        state = LanchPadState.Idle;
    }
    public void LanchPadDropEnd() {
        myVehicleCarry.ChangeVehicle(vehiclePickUp);
        currentLanding.able = true;
        myVehicleCarry = null;
    }
    public void ChangeState(LanchPadState stateChange) {
        state = stateChange;
    }
    public void ChangePickUp(Transform pickUpChange) {
        vehiclePickUp = pickUpChange;
    }
    public void RotageToDrop() {
        ChangeState(LanchPadState.DropVehicle);
    }
}
public class LanchPadIdle : State<LanchPad>
{
    private static LanchPadIdle Instance;
    public static LanchPadIdle instance
    {
        get
        {
            if (Instance == null)
                Instance = new LanchPadIdle();
            return Instance;
        }
    }
    public override void Enter(LanchPad go)
    {
        go.LanchPadIdleEnter();
    }
    public override void Execute(LanchPad go)
    {
        go.LanchPadIdleExecute();
    }
    public override void End(LanchPad go)
    {
        go.LanchPadIdleEnd();
    }
}
public class LanchPadPickUp : State<LanchPad>
{
    private static LanchPadPickUp Instance;
    public static LanchPadPickUp instance
    {
        get
        {
            if (Instance == null)
                Instance = new LanchPadPickUp();
            return Instance;
        }
    }
    public override void Enter(LanchPad go)
    {
        go.LanchPadPickUpEnter();
    }
    public override void Execute(LanchPad go)
    {
        go.LanchPadPickUpExecute();
    }
    public override void End(LanchPad go)
    {
        go.LanchPadPickUpEnd();
    }
}
public class LanchPadCallCarryCar : State<LanchPad>
{
    private static LanchPadCallCarryCar Instance;
    public static LanchPadCallCarryCar instance
    {
        get
        {
            if (Instance == null)
                Instance = new LanchPadCallCarryCar();
            return Instance;
        }
    }
    public override void Enter(LanchPad go)
    {
        go.LanchPadCallCarryEnter();
    }
    public override void Execute(LanchPad go)
    {
        go.LanchPadCallCarryExecute();
    }
    public override void End(LanchPad go)
    {
        go.LanchPadCallCarryEnd();
    }
}
public class LanchPadDrop : State<LanchPad>
{
    private static LanchPadDrop Instance;
    public static LanchPadDrop instance
    {
        get
        {
            if (Instance == null)
                Instance = new LanchPadDrop();
            return Instance;
        }
    }
    public override void Enter(LanchPad go)
    {
        go.LanchPadDropEnter();
    }
    public override void Execute(LanchPad go)
    {
        go.LanchPadDropExecute();
    }
    public override void End(LanchPad go)
    {
        go.LanchPadDropEnd();
    }
}
