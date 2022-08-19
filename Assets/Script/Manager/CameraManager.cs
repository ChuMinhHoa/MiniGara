using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
public class CameraManager : MonoBehaviour
{
    [SerializeField] Transform cameraRig;
    public Camera myMainCamera;
    public AnimationCurve cameraCurve;
    public Quaternion defaultRotage;
    public float speed;
    Quaternion targetRotage, startRotage;
    Vector3 targetPos, startPos;
    Vector3 lastPosBeforeTouchHouse;
    Vector3 pointStarDrag;
    Vector3 mouseDrag;
    float timeCurve;
    float fovDefault, fovTarget, fovStart;
    bool touchItem;
    bool defaultSet;
    bool setLastPos = true;
    private void Start()
    {
        fovDefault = myMainCamera.fieldOfView;
    }
    public void ChangeTarget(Vector3 targetChange, Vector3 targetRotageChange) {
        if (setLastPos)
        {
            lastPosBeforeTouchHouse = myMainCamera.transform.position;
            setLastPos = false;
            fovTarget = 60;
        }
        startPos = cameraRig.position;
        startRotage = myMainCamera.transform.rotation;
        targetPos = targetChange;
        targetRotage = Quaternion.LookRotation(targetRotageChange - targetChange, Vector3.up);
        touchItem = true;
        timeCurve = 0;
        fovStart = myMainCamera.fieldOfView;
    }
    public void BackToDefault() {
        setLastPos = defaultSet = true;
        startPos = cameraRig.position;
        startRotage = myMainCamera.transform.rotation;
        targetPos = lastPosBeforeTouchHouse;
        targetRotage = defaultRotage;
        fovTarget = fovDefault;
        timeCurve = 0;
    }
    private void Update()
    {
        if (touchItem || defaultSet)
        {
            if (timeCurve <= cameraCurve.keys[cameraCurve.length - 1].time)
            {
                cameraRig.position = Vector3.Lerp(startPos, targetPos, cameraCurve.Evaluate(timeCurve));
                myMainCamera.transform.rotation = Quaternion.Slerp(startRotage, targetRotage, cameraCurve.Evaluate(timeCurve));
                myMainCamera.fieldOfView = Mathf.Lerp(fovStart, fovTarget, cameraCurve.Evaluate(timeCurve));
                timeCurve += Time.deltaTime;
            }
            else {
                cameraRig.position = targetPos;
                myMainCamera.transform.rotation = targetRotage;
                myMainCamera.fieldOfView = fovTarget;
                touchItem = defaultSet = false;
            }
        }
        if (UIManager.instance.isPopup)
            return;
        if (Input.GetMouseButtonDown(0))
        {
            pointStarDrag = Input.mousePosition;
        }
        if (Input.GetMouseButton(0))
        {
            mouseDrag = Camera.main.ScreenToViewportPoint(Input.mousePosition - pointStarDrag) * speed;
            pointStarDrag = Input.mousePosition;
            mouseDrag.z = mouseDrag.y;
            mouseDrag.y = 0;
            cameraRig.Translate(mouseDrag);
        }
    }
}
