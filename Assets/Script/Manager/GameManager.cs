using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class GameManager : GenericSingleton<GameManager>
{
    public CarryManager carryVehicleCarryManager;
    public LandingPadManager landingPadManager;
    public FixRoomManager fixRoomManager;
    public WorkerManager workerManager;
    public TakeOffManager takeOffManager;
    public CameraManager cameraManager;
    public int roomCount;
    public int staffCount;
    public IRoomControler currentRoomSelected;
    bool isTouching;
    Vector3 touchDown, touchUp;
    public void SpawnRoom() {
        carryVehicleCarryManager.SpawnRoom();
        landingPadManager.SpawnRoom();
        fixRoomManager.SpawnRoom();
        workerManager.SpawnRoom();
        takeOffManager.SpawnRoom();
    }
    private void Start()
    {
       
    }
    private void Update()
    {
        //if (Input.GetKeyDown(KeyCode.N))
        //{
        //    Time.timeScale += 2f;
        //}
        //if (Input.GetKeyDown(KeyCode.B))
        //{
        //    Time.timeScale -= 2f;
        //}
        if (Input.GetMouseButtonDown(0))
        {
            isTouching = !UIManager.instance.isPopup && !EventSystem.current.IsPointerOverGameObject();
            if (EventSystem.current.currentSelectedGameObject) isTouching = false;
            touchDown = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        }
        if (isTouching)
        {
            if (Input.GetMouseButtonUp(0))
            {
                isTouching = false;
                touchUp = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                if (Vector3.Distance(touchDown, touchUp) >= 0.02f) { 
                    //move camera
                    return; 
                }
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;
                Physics.Raycast(ray, out hit);
                if (hit.collider != null)
                {
                    bool isAcceptTouch;
                    UIBuildTarget buildTarget = hit.collider.GetComponent<UIBuildTarget>();
                    if (buildTarget != null)
                    {
                        
                    }
                    else {
                        currentRoomSelected = hit.collider.GetComponent<IRoomControler>();
                        if (currentRoomSelected != null)
                        {
                            cameraManager.ChangeTarget(hit.collider.transform.position, hit.point);
                            UIManager.instance.ShowUpgradePanel(currentRoomSelected);
                        }
                    }
                }
            }
        }
    }
    public bool CheckToCallVehicleBroke() {
        LandingPad landingPad = landingPadManager.GetLandingRoom();
        if (landingPad == null)
            return false;
        int countAbleVehicle = carryVehicleCarryManager.CountCarryVehicle();
        if (countAbleVehicle <= 1)
            return false;
        return true;
    }
    public int GetBuffAmountSpawnVehicleBroke() {
        return 0;
    }
}
