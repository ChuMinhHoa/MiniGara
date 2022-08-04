using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VehicleBroke : VehicleBase
{
    public LandingPad myLandingPad;
    public LanchPad myLanchPad;

    public Vector3 pointLanding;
    public Vector3 offset;
    public float speed;
    public float smooth;
    [SerializeField] Transform bootersTransform;
    [SerializeField] GameObject bootersVFX;
    Vector3 vel;
    public override void IdleEnter()
    {
        anim.Play("Idle");
    }
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
    }
    public override void ChangeTargetMove(Vector3 targetMove, Vector3 landingPointChange)
    {
        targetToMove = targetMove;
        pointLanding = landingPointChange;
        vehicleState = VehicleState.Move;
    }
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
        {
            vehicleState = VehicleState.OnLand;
        }
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
}
