public enum PlayerState { 
    Idle,
    Drive,
    Move,
    MoveJoyStick
}
public enum MortoBikeState { 
    Unusing,
    StartUp,
    Idle,
    Move,
    ShutDown
}
public enum EMode{ 
    None,
    Bike
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
    TakeOff
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
    Rotage
}
public enum TakeOffRoomState { 
    Idle,
    MoveVehicle,
    GetBrokeVehicle,
    Drop,
    TakeOff
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