public enum CustomerState { 
    OnVehicle,
    Idle,
    Move,
    Talk
}
public enum VehicleType {
    HoverCraft
}
public enum VehicleState
{
    Idle,
    Move,
    Landing,
    Rotage,
    OnLand,
    TakeOff,
    OpenWindown
}
public enum LandingPadState { 
    Idle,
    CallWorker,
    LaunchPadCall,
    ElevatorDown
}
public enum LanchPadState { 
    Idle,
    PickUp,
    DropVehicle,
    CallCarry
}
public enum FixRoomState
{
    Idle,
    PickUp,
    Drop,
    BackAndSp,
    FixedDone
}
public enum HouseRoomState { 
    Idle,
    FreeTime,
    SleepTime,
    EatTime,
    WorkTime
}
public enum RoomType { 
    LandingRoom,
    FixRoom,
    CarryRoom,
    House,
    TakeOffRoom
}
public enum StaffState { 
    Idle,
    Move,
    Work,
    Rotage,
    Sleep,
    FreeTime
}
public enum TakeOffRoomState { 
    Idle,
    MoveVehicle,
    GetBrokeVehicle,
    Drop,
    TakeOff
}
public enum StaffType { 
    Worker,
    Planter
}
#region Model Type
public enum FixRoomModelType { 
    FixRoom_Energy,
    FixRoom_Tool,
    FixRoom_Table
}
public enum CarryRoomModelType
{
    CarryRoom_Atten,
    CarryRoom_Energy,
    CarryRoom_VehicleEnergy
}
public enum HouseModelType {
    House_Wardrobe,
    House_Bed,
    House_TV,
    House_Sofa
}
public enum LandingPadModelType
{
    LandingPad_Monitor,
    LandingPad_Led,
    LandingPad_Elevator
}
public enum TakeOffModelType
{
    TakeOff_Elevator,
    TakeOff_Led
}
#endregion
#region Staff Model
public enum WorkerModelType {
    Worker_Bag,
    Worker_Hair
}
public enum PlantCareModelType {
    Bag,
    Hair
}
#endregion
#region UI
public enum UIPanelType {
    UpgradePanel
}
#endregion
#region TimeManager
public enum BehaviorType { 
    Eat,
    Work,
    FreeTime,
    Sleep
}
public enum FreeBehavior { 
    Talking,
    WatchTV
}
#endregion