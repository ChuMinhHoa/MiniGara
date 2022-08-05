using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.Events;

public class VehicleBroke : VehicleBase
{
    [Header("Room")]
    [Header("===========VEHICLE BROKE==============")]
    public LandingPad myLandingPad;
    public LanchPad myLanchPad;
    public TakeOffRoom myTakeOffRoom;
    public BrokenRoom myBrokenRoom;
    [Header("Vector")]
    public Vector3 pointLanding;
    public Vector3 offset;
    Vector3 vel;
    Vector3 startPoint;
    [Header("Transform And GameObject")]
    [SerializeField] Transform bootersTransform;
    [SerializeField] GameObject bootersVFX;
    [Header("Other")]
    public float speed;
    public float smooth;
    public AnimationCurve curveTakeOff;
    bool canTakeOffNow;
    float timeCurve;
    UnityAction actionDone;
    public override void IdleEnter()
    {
        anim.Play("Idle");
    }
    #region Move
    public override void MoveEnter()
    {
        vel = Vector3.zero;
        Vector3 dir = transform.position - targetToMove;
        timeRotage = 0;
        rotageTo = Quaternion.LookRotation(-dir, Vector3.up);
        rotageFrom = transform.rotation;
        bootersVFX.SetActive(true);
        anim.Play("Move");
    }
    public override void MoveExecute()
    {
        if (timeRotage <= rotageToRightWay.keys[rotageToRightWay.length - 1].time)
        {
            transform.rotation = Quaternion.Slerp(rotageFrom, rotageTo, rotageToRightWay.Evaluate(timeRotage));
            timeRotage += Time.deltaTime;
            return;
        }
        transform.position = Vector3.SmoothDamp(transform.position, targetToMove, ref vel, smooth / speed);
        if (Vector3.Distance(transform.position, targetToMove) <= 0.1f)
        {
            vehicleState = VehicleState.Landing;
        }
    }
    public override void MoveEnd()
    {
        base.MoveEnd();
        if (actionDone != null)
            actionDone();
    }
    public override void ChangeTargetMove(Vector3 targetMove, Vector3 landingPointChange)
    {
        targetToMove = targetMove;
        pointLanding = landingPointChange;
        vehicleState = VehicleState.Move;
    }
    public override void ChangeTargetMove(Vector3 targetMove, UnityAction moveDoneAction = null)
    {
        targetToMove = targetMove;
        vehicleState = VehicleState.Move;
        actionDone = moveDoneAction;
    }
    #endregion
    #region Landing
    public override void LandingEnter()
    {
        vel = Vector3.zero;
        timeRotage = 0;
        rotageTo = Quaternion.LookRotation(-Vector3.forward, Vector3.up);
        rotageFrom = transform.rotation;
        anim.Play("Landing");
    }
    public override void LandingExecute()
    {
        if (timeRotage <= rotageToRightWay.keys[rotageToRightWay.length - 1].time)
        {
            transform.rotation = Quaternion.Slerp(rotageFrom, rotageTo, rotageToRightWay.Evaluate(timeRotage));
            timeRotage += Time.deltaTime;
            return;
        }
        transform.position = Vector3.SmoothDamp(transform.position, pointLanding + offset, ref vel, smooth);
        if (Vector3.Distance(transform.position, pointLanding + offset) <= 0.1f)
            vehicleState = VehicleState.OnLand;
    }
    public override void LandingEnd()
    {
        //call Player
        transform.parent = myLandingPad.elevator.transform;
        myLandingPad.ChangeState(LandingPadState.CallWorker);
        bootersVFX.SetActive(false);
        anim.Play("OnLand");
    }
    public override void ChangeLandingPad(LandingPad landingPadChange, LanchPad lanchPad)
    {
        myLandingPad = landingPadChange;
        myLanchPad = lanchPad;
    }
    #endregion
    #region TakeOff
    public override void TakeOffEnter()
    {
        anim.Play("PopUpPrepare");
        canTakeOffNow = false;
        StartCoroutine(IE_WaitingForCanPopUp());
        targetToMove = myTakeOffRoom.pointStartFly.position;
        timeCurve = 0f;
        startPoint = transform.position;
    }
    IEnumerator IE_WaitingForCanPopUp() {
        yield return new WaitForSeconds(1f);
        canTakeOffNow = true;
        anim.Play("PopUp");
    }
    public override void TakeOffExecute()
    {
        if (canTakeOffNow)
        {
            if (timeCurve <= curveTakeOff.keys[curveTakeOff.length - 1].time)
            {
                transform.position = Vector3.Lerp(startPoint, targetToMove, curveTakeOff.Evaluate(timeCurve));
                timeCurve += Time.deltaTime;
            }
            else {
                canTakeOffNow = false;
                ChangeTargetMove(myBrokenRoom.vehicleFixedGo.position, DestroySelf);
            }
        }
    }
    #endregion
    void DestroySelf() {
        //myBrokenRoom.RemoveVehicleBroke(this);
        Destroy(gameObject); 
    }
}