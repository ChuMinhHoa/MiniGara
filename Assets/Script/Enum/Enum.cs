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
    LanchRoom,
    FixRoom,
    CarryRoom,
    House,
    TakeOffRoom
}
public enum ModelType { }
public enum WorkerState { 
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
    CarryRoom_Carry
}
public enum HouseModelType {
    House_Table,
    House_Bed,
    House_TV
}
public enum LandingPadModelType
{
    LandingPad_Table,
    LandingPad_Bed,
    LandingPad_TV
}
public enum TakeOffModelType
{
    TakeOff_Table,
    TakeOff_Bed,
    TakeOff_TV
}
public enum LanchPadModelType
{
    LanchPad_LoadingBay,
    LanchPad_Energy,
}
#endregion