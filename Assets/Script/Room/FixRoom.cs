using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FixRoom : BaseRoom<FixRoomModelType>
{
    protected StateMachine<FixRoom> m_Statemachine;
    public StateMachine<FixRoom> stateMachine { get { return m_Statemachine; } }
    Animator anim;
    public FixRoomState state;
    FixRoomState currentState;
    public Transform fixPoint;
    public bool able = true;
    public bool workerOnPosition;
    public List<VehicleCarry> vehiclesProgress;
    public Transform elevator;
    public Transform workerPoint;
    Transform vehicleBroke;
    VehicleCarry vehicleOnProgress;
    Worker myWorker;
    float timeRepair;
    public float timeRepairSetting;
    private void Awake()
    {
        anim = GetComponent<Animator>();
        m_Statemachine = new StateMachine<FixRoom>(this);
        m_Statemachine.ChangeState(FixRoomIdle.instance);
        m_Statemachine.SetcurrentState(FixRoomIdle.instance);
    }
    public void AddProgress(VehicleCarry vehicleCarry) {
        vehiclesProgress.Add(vehicleCarry);
    }
    private void Update()
    {
        stateMachine.Update();

        if (vehiclesProgress.Count > 0)
        {
            if (able && vehiclesProgress[0].vehicleState == VehicleState.Idle) {
                able = false;
                vehicleOnProgress = vehiclesProgress[0];
                vehiclesProgress.Remove(vehiclesProgress[0]); 
                MoveVehicleToFixPoint(vehicleOnProgress);
            }   
            else {
                foreach (VehicleCarry vehicleCarry in vehiclesProgress)
                {
                    if (GetWaitingPoint() != null && vehicleCarry.myWaitingPoint == null)
                        MoveVehicleToWaitingPoint(vehicleCarry, GetWaitingPoint());
                }
            }   
        }
        
        switch (state)
        {
            case FixRoomState.Idle:
                if (currentState != state)
                {
                    currentState = state;
                    stateMachine.ChangeState(FixRoomIdle.instance);
                }
                break;
            case FixRoomState.PickUp:
                if (currentState != state)
                {
                    currentState = state;
                    stateMachine.ChangeState(FixRoomPickUp.instance);
                }
                break;
            case FixRoomState.Drop:
                if (currentState != state)
                {
                    currentState = state;
                    stateMachine.ChangeState(FixRoomDrop.instance);
                }
                break;
            case FixRoomState.BackAndSp:
                if (currentState != state)
                {
                    currentState = state;
                    stateMachine.ChangeState(FixRoomBackAndSp.instance);
                }
                break;
            case FixRoomState.FixedDone:
                if (currentState != state)
                {
                    currentState = state;
                    stateMachine.ChangeState(FixedDone.instance);
                }
                break;
            default:
                break;
        }
    }
    public void IdleEnter() {
        if (anim.GetCurrentAnimatorStateInfo(0).IsName("FixedDone"))
        {
            anim.Play("BackAndSp");
        }
        else anim.Play("Idle");
    }
    public void IdleExecute() { }
    public void IdleEnd() { }
    public void PickUpEnter() {
        anim.Play("PickUp");
        StopAllCoroutines();
        StartCoroutine(IE_WaitPickUp());
    }
    IEnumerator IE_WaitPickUp() {
        yield return new WaitForSeconds(1f);
        vehicleBroke.parent = elevator;
        state = FixRoomState.BackAndSp;
        WaitingPoint waitingPoint = GameManager.instance.carryVehicleCarryManager.GetCarryRoom().GetWaitingPoint();
        vehicleOnProgress.myWaitingPoint = waitingPoint;
        waitingPoint.able = false;
        vehicleOnProgress.ChangeTargetMove(waitingPoint.point.position, vehicleOnProgress.ResetAble);
        vehicleOnProgress.ChangeRotageTo(Quaternion.LookRotation(Vector3.right, Vector3.up));
        vehicleOnProgress = null;
    }
    
    public void PickUpExecute() { }
    public void PickUpEnd() { }
    public void BackAndSpEnter()
    {
        anim.Play("BackAndSp");
        StopAllCoroutines();
        StartCoroutine(IE_WaitBack());
    }
    IEnumerator IE_WaitBack()
    {
        yield return new WaitForSeconds(3f);
        vehicleBroke.parent = elevator;
        state = FixRoomState.Drop;
    }
    public void BackAndSpExecute() { }
    public void BackAndSpEnd() { }
    public void DropEnter() {
        anim.Play("Drop");
        timeRepair = timeRepairSetting;
        myWorker = null;
        workerOnPosition = false;
    }
    public void DropExecute() {
        if (myWorker == null)
        {
            myWorker = GameManager.instance.workerManager.GetWorker();
            if (myWorker != null)
            {
                myWorker.work = true;
                myWorker.ChangeTarget(workerPoint.position, elevator, () => { workerOnPosition = true; });
                myWorker.ChangeWaitingPoint();
            }
            return;
        }
        if (!workerOnPosition)
            return;
        if (timeRepair >= 0f)
        {
            timeRepair -= Time.deltaTime;
            return;
        }
        state = FixRoomState.FixedDone;
    }
    public void DropEnd() {
        
    }
    void MoveVehicleToFixPoint(VehicleCarry vehicleCarry) {
        vehicleBroke = vehicleCarry.vehicleTake;
        vehicleCarry.ChangeRotageTo(Quaternion.LookRotation(Vector3.forward, Vector3.up));
        vehicleCarry.ChangeTargetMove(fixPoint.position, StartRepairVehicle);
        vehicleCarry.ChangeWaitingPoint();
    }
    void MoveVehicleToWaitingPoint(VehicleCarry vehicleCarry, WaitingPoint waitingPoint) {
        vehicleCarry.ChangeRotageTo(Quaternion.LookRotation(Vector3.forward, Vector3.up));
        vehicleCarry.ChangeTargetMove(waitingPoint.point.position);
        vehicleCarry.ChangeWaitingPoint(waitingPoint);
    }
    public bool CheckAble() {
        for (int i = 0; i < waitingPoints.Count; i++)
        {
            if (waitingPoints[i].able)
                return true;
        }
        return false;
    }
    void StartRepairVehicle() {
        state = FixRoomState.PickUp;
    }
    public void FixedDoneEnter()
    {
        anim.Play("FixedDone");
        animTime = 0;
        WaitingPoint waitingPoint = GameManager.instance.workerManager.GetWaitingPoint(myWorker.GetHouse());
        myWorker.ChangeWaitingPoint(waitingPoint);
        myWorker.ChangeTarget(waitingPoint.point.position - new Vector3(0.01f,0,0), waitingPoint.point, myWorker.ResetWorker);
        myWorker.ChangeTimeAble(-1);
        myWorker.work = false;
        myWorker = null;
    }
    float animTime;
    public void FixedDoneExecute() {
        if (animTime <= 2f)
        {
            animTime += Time.deltaTime;
            return;
        }
        if (vehicleOnProgress == null && !able && vehicleBroke != null)
        {
            vehicleOnProgress = GameManager.instance.carryVehicleCarryManager.GetAbleVehicle();
            if (vehicleOnProgress != null)
            {
                vehicleOnProgress.ChangeRotageTo(Quaternion.LookRotation(Vector3.forward, Vector3.up));
                vehicleOnProgress.ChangeTargetMove(fixPoint.position, ReturnVehicleBroke);
                vehicleOnProgress.ChangeWaitingPoint();
            }
            return;
        }    
    }
    public void FixedDoneEnd() {
        vehicleOnProgress = null;
        able = true;
        vehicleBroke = null;
    }
    void ReturnVehicleBroke() {
        vehicleBroke.transform.parent = vehicleOnProgress.takePlace;
        vehicleOnProgress.vehicleTake = vehicleBroke;
        state = FixRoomState.Idle;
        GameManager.instance.takeOffManager.AddVehicleCarry(vehicleOnProgress);
    }
}
public class FixRoomIdle : State<FixRoom> {
    private static FixRoomIdle m_Instance;
    public static FixRoomIdle instance
    {
        get {
            if (m_Instance == null)
                m_Instance = new FixRoomIdle();
            return m_Instance;
        }
    }
    public override void Enter(FixRoom go)
    {
        go.IdleEnter();
    }
    public override void Execute(FixRoom go)
    {
        go.IdleExecute();
    }
    public override void End(FixRoom go)
    {
        go.IdleEnd();
    }
}
public class FixRoomPickUp : State<FixRoom>
{
    private static FixRoomPickUp m_Instance;
    public static FixRoomPickUp instance
    {
        get
        {
            if (m_Instance == null)
                m_Instance = new FixRoomPickUp();
            return m_Instance;
        }
    }
    public override void Enter(FixRoom go)
    {
        go.PickUpEnter();
    }
    public override void Execute(FixRoom go)
    {
        go.PickUpExecute();
    }
    public override void End(FixRoom go)
    {
        go.PickUpEnd();
    }
}
public class FixRoomBackAndSp : State<FixRoom>
{
    private static FixRoomBackAndSp m_Instance;
    public static FixRoomBackAndSp instance
    {
        get
        {
            if (m_Instance == null)
                m_Instance = new FixRoomBackAndSp();
            return m_Instance;
        }
    }
    public override void Enter(FixRoom go)
    {
        go.BackAndSpEnter();
    }
    public override void Execute(FixRoom go)
    {
        go.BackAndSpExecute();
    }
    public override void End(FixRoom go)
    {
        go.BackAndSpEnd();
    }
}
public class FixRoomDrop : State<FixRoom>
{
    private static FixRoomDrop m_Instance;
    public static FixRoomDrop instance
    {
        get
        {
            if (m_Instance == null)
                m_Instance = new FixRoomDrop();
            return m_Instance;
        }
    }
    public override void Enter(FixRoom go)
    {
        go.DropEnter();
    }
    public override void Execute(FixRoom go)
    {
        go.DropExecute();
    }
    public override void End(FixRoom go)
    {
        go.DropEnd();
    }
}
public class FixedDone : State<FixRoom> {
    private static FixedDone m_Instance;
    public static FixedDone instance
    {
        get
        {
            if (m_Instance == null)
                m_Instance = new FixedDone();
            return m_Instance;
        }
    }
    public override void Enter(FixRoom go)
    {
        go.FixedDoneEnter();
    }
    public override void Execute(FixRoom go)
    {
        go.FixedDoneExecute();
    }
    public override void End(FixRoom go)
    {
        go.FixedDoneEnd();
    }
}
