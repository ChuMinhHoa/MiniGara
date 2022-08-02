using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.AI;

public class VehicleCarry : VehicleBase
{
    public Transform takePlace;
    public Transform vehicleTake;
    Animator anim;
    UnityAction actionDone;
    public WaitingPoint myWaitingPoint;
    public override void Awake()
    {
        base.Awake();
        anim = GetComponent<Animator>();
    }
    public override void IdleEnter()
    {
    }
    public override void IdleEnd()
    {
    }
    public override void MoveEnter()
    {
        agent.enabled = true;
        agent.isStopped = false;
    }
    public override void MoveExecute()
    {
        if (!agent.enabled)
            return;
        agent.destination = targetToMove;
        if (IsFinishMoveOnNavemesh())
            vehicleState = VehicleState.Rotage;
    }
    public override void MoveEnd()
    {
        agent.isStopped = true;
    }
    public override void ChangeTargetMove(Vector3 targetMove, UnityAction actionDoneChange = null)
    {
        if (transform.position == targetMove)
        {
            return;
        }
        targetToMove = targetMove;
        //targetToMove.y = transform.position.y;
        vehicleState = VehicleState.Move;
        actionDone = actionDoneChange;
        able = false;
    }
    public override void RotageEnter()
    {
        timeRotage = 0;
        rotageFrom = transform.rotation;
    }
    public override void RotageExecute()
    {
        if (timeRotage <= rotageToRightWay.keys[rotageToRightWay.length - 1].time)
        {
            transform.rotation = Quaternion.Slerp(rotageFrom, rotageTo, rotageToRightWay.Evaluate(timeRotage));
            timeRotage += Time.deltaTime;
            return;
        }
        vehicleState = VehicleState.Idle;
    }
    public override void RotageEnd() {
        if (actionDone != null)
        {
            actionDone();
            actionDone = null;
        }
    }
    public void ChangeRotageTo(Quaternion rotageToChange) {
        rotageTo = rotageToChange;
    }
    public void ChangeVehicle(Transform vechicleTakeChange) {
        anim.Play("TakeVehicle");
        vehicleTake = vechicleTakeChange;
        StopAllCoroutines();
        StartCoroutine(TakeVehicle());
    }
    IEnumerator TakeVehicle() {
        yield return new WaitForSeconds(1f);
        vehicleTake.parent = takePlace;
        anim.Play("TakeVehicleDone");
        GameManager.instance.carryVehicleCarryManager.AddVehicleProgress(this);
    }
    public void ResetAble() {
        able = true;
    }
    public void ChangeWaitingPoint(WaitingPoint waitingPoint = null) {
        if (myWaitingPoint != null)
            myWaitingPoint.able = true;
        if (waitingPoint == null)
        {
            myWaitingPoint = null;
            return;
        }
        myWaitingPoint = waitingPoint;
        myWaitingPoint.able = false;
    }
}
public class VehicleCarryRotageAffterMove : State<VehicleBase> {
    private static VehicleCarryRotageAffterMove m_Instance;
    public static VehicleCarryRotageAffterMove instance
    {
        get
        {
            if (m_Instance == null)
                m_Instance = new VehicleCarryRotageAffterMove();
            return m_Instance;
        }
    }
    public override void Enter(VehicleBase go)
    {
        go.RotageEnter();
    }
    public override void Execute(VehicleBase go)
    {
        go.RotageExecute();
    }
    public override void End(VehicleBase go)
    {
        go.RotageEnd();
    }
}

