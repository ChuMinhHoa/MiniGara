using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.AI;

public class VehicleBase : MonoBehaviour
{
    public StateMachine<VehicleBase> stateMachine { get { return m_stateMachine; } }
    protected StateMachine<VehicleBase> m_stateMachine;
    public Animator anim;
    public virtual void Awake()
    {
        m_stateMachine = new StateMachine<VehicleBase>(this);
        m_stateMachine.ChangeState(VehicleIdle.instance);
        m_stateMachine.SetcurrentState(VehicleIdle.instance);
        anim = GetComponent<Animator>();
    }
    public VehicleState vehicleState;
    VehicleState currentState;
    public NavMeshAgent agent;
    public NavMeshObstacle obstacle;
    public virtual void Update() {
        stateMachine.Update();
        switch (vehicleState)
        {
            case VehicleState.Idle:
                if (currentState != vehicleState)
                {
                    currentState = vehicleState;
                    stateMachine.ChangeState(VehicleIdle.instance);
                }
                break;
            case VehicleState.Move:
                if (currentState != vehicleState)
                {
                    currentState = vehicleState;
                    stateMachine.ChangeState(VehicleMove.instance);
                }
                break;
            case VehicleState.Landing:
                if (currentState != vehicleState)
                {
                    currentState = vehicleState;
                    stateMachine.ChangeState(VehicleLanding.instance);
                }
                break;
            case VehicleState.Rotage:
                if (currentState != vehicleState)
                {
                    currentState = vehicleState;
                    stateMachine.ChangeState(VehicleCarryRotageAffterMove.instance);
                }
                break;
            case VehicleState.OnLand:
                if (currentState != vehicleState)
                {
                    currentState = vehicleState;
                    stateMachine.ChangeState(VehicleOnland.instance);
                }
                break;
            case VehicleState.TakeOff:
                if (currentState != vehicleState)
                {
                    currentState = vehicleState;
                    stateMachine.ChangeState(VehicleTakeOff.instance);
                }
                break;
            default:
                break;
        }
    }

    public Vector3 targetToMove;
    public CharacterController myCharactor;
    public bool able = true;

    public AnimationCurve rotageToRightWay;
    public float timeRotage = 0f;
    public Quaternion rotageTo;
    public Quaternion rotageFrom;
    public virtual void IdleEnter() {
       
    }
    public virtual void IdleExecute() { }
    public virtual void IdleEnd() {
        
    }
    
    public virtual void MoveEnter() { }
    public virtual void MoveExecute() { }
    public virtual void MoveEnd() { }

    #region Broken vehicle
    public virtual void LandingEnter() { }
    public virtual void LandingExecute() { }
    public virtual void LandingEnd() { }
    public virtual void ChangeLandingPad(LandingPad landingPadChange, LanchPad lanchPad) { }
    public virtual void OnlandEnter() { }
    public virtual void OnlandExecute() { }
    public virtual void OnlandEnd() { }
    public virtual void TakeOffEnter() { }
    public virtual void TakeOffExecute() { }
    public virtual void TakeOffEnd() { }
    #endregion

    #region CarryVehicle
    public virtual void RotageEnter() { }
    public virtual void RotageExecute() { }
    public virtual void RotageEnd() { }
    #endregion
    public virtual void ChangeTargetMove(Vector3 targetMove, Vector3 landingPointChange) { }
    public virtual void ChangeTargetMove(Vector3 targetMove, UnityAction moveDoneAction = null) {}
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
public class VehicleIdle : State<VehicleBase> {
    private static VehicleIdle m_Instance;
    public static VehicleIdle instance
    {
        get {
            if (m_Instance == null)
                m_Instance = new VehicleIdle();
            return m_Instance;
        }
    }
    public override void Enter(VehicleBase go)
    {
        go.IdleEnter();
    }
    public override void Execute(VehicleBase go)
    {
        go.IdleExecute();
    }
    public override void End(VehicleBase go)
    {
        go.IdleEnd();
    }
}
public class VehicleMove : State<VehicleBase>
{
    private static VehicleMove m_Instance;
    public static VehicleMove instance
    {
        get
        {
            if (m_Instance == null)
                m_Instance = new VehicleMove();
            return m_Instance;
        }
    }
    public override void Enter(VehicleBase go)
    {
        go.MoveEnter();
    }
    public override void Execute(VehicleBase go)
    {
        go.MoveExecute();
    }
    public override void End(VehicleBase go)
    {
        go.MoveEnd();
    }
}
public class VehicleLanding : State<VehicleBase>
{
    private static VehicleLanding m_Instance;
    public static VehicleLanding instance
    {
        get
        {
            if (m_Instance == null)
                m_Instance = new VehicleLanding();
            return m_Instance;
        }
    }
    public override void Enter(VehicleBase go)
    {
        go.LandingEnter();
    }
    public override void Execute(VehicleBase go)
    {
        go.LandingExecute();
    }
    public override void End(VehicleBase go)
    {
        go.LandingEnd();
    }
}
public class VehicleOnland : State<VehicleBase>
{
    private static VehicleOnland m_Instance;
    public static VehicleOnland instance
    {
        get
        {
            if (m_Instance == null)
                m_Instance = new VehicleOnland();
            return m_Instance;
        }
    }
    public override void Enter(VehicleBase go)
    {
        go.OnlandEnter();
    }
    public override void Execute(VehicleBase go)
    {
        go.OnlandExecute();
    }
    public override void End(VehicleBase go)
    {
        go.OnlandEnd();
    }
}
public class VehicleTakeOff : State<VehicleBase> {
    private static VehicleTakeOff m_Instance;
    public static VehicleTakeOff instance
    {
        get {
            if (m_Instance == null)
                m_Instance = new VehicleTakeOff();
            return m_Instance;
        }
    }
    public override void Enter(VehicleBase go)
    {
        go.TakeOffEnter();
    }
    public override void Execute(VehicleBase go)
    {
        go.TakeOffExecute();
    }
    public override void End(VehicleBase go)
    {
        go.TakeOffEnd();
    }
}
