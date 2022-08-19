using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.Events;

public class VehicleBroke : VehicleBase
{
    [Header("Room")]
    [Header("===========VEHICLE BROKE==============")]
    [HideInInspector]public TakeOffRoom myTakeOffRoom;
    [HideInInspector]public BrokenRoom myBrokenRoom;
    public LandingPad myLandingPad;
    [HideInInspector]public LanchPad myLanchPad;
    [Header("Vector")]
    public Vector3 pointLanding;
    public Vector3 offset;
    Vector3 vel;
    Vector3 startPoint;
    [Header("Transform And GameObject")]
    [SerializeField] Transform bootersTransform;
    [SerializeField] GameObject bootersVFX;
    [SerializeField] Transform pointCharactorControl;
    [SerializeField] CustomerBase customPreb;
    [Header("Other")]
    public AnimationCurve curveTakeOff;
    UnityAction actionDone;
    public float speed;
    public float smooth;
    bool canTakeOffNow;
    bool rotageWindownDone = false;
    float timeCurve;
    public override void Awake()
    {
        base.Awake();
        CustomerBase newCus = Instantiate(customPreb, transform.position, Quaternion.identity, pointCharactorControl);
        myCharactor = newCus;
        newCus.transform.localPosition = new Vector3(0, 0, 0);
        newCus.transform.localScale = new Vector3(1.2f, 1.2f, 1.2f);
    }
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
            vehicleState = VehicleState.OpenWindown;
    }
    public override void LandingEnd()
    {
        transform.parent = myLandingPad.elevator.transform;
        bootersVFX.SetActive(false);
        anim.Play("OnLand");
    }
    public override void ChangeLandingPad(LandingPad landingPadChange, LanchPad lanchPad)
    {
        myLandingPad = landingPadChange;
        myLanchPad = lanchPad;
    }
    #endregion
    #region ===============OpenWindown===============
    public override void OpenWindownEnter()
    {
        anim.Play("OpenWinDown");
        rotageWindownDone = false;
    }
    public override void OpenWindownExecute()
    {
        if (rotageWindownDone)
        {
            myLandingPad.ChangeState(LandingPadState.CallWorker);
            vehicleState = VehicleState.OnLand;
        }
    }
    public void RotageWindownDone() { rotageWindownDone = true;  }
    public void PushCharactorDown() {
        myCharactor.transform.parent = null;
        Vector3 point = myLandingPad.pointDropCustomer.position;
        myCharactor.transform.position = point;
        Debug.Log(point + " " + myCharactor.transform.position);
        //myCharactor.ChangeState(CustomerState.Idle);
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
    public void TakeCharactorOn() { }
    #endregion
    void DestroySelf() {
        //myBrokenRoom.RemoveVehicleBroke(this);
        Destroy(gameObject); 
    }
}
