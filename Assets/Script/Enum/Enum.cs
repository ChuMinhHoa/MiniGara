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
    FixRoom
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
