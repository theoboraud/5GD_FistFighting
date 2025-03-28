/// <summary>
/// Global Game Events
/// </summary>
public enum GameEvent
{
    //GameManager
    OnNewGameRound,
    OnGameReset,

    //LevelManager
    OnLoadScene,

    //SpawnPoints
    OnSpawnPointsInit,

    //Player
    OnPlayerDead,
}

/// <summary>
/// Local Events for player
/// </summary>
public enum PlayerEvent
{
    //Player
    OnPlayerSpawn,
    OnPlayerHit,

    //PlayerController
    OnPlayerCollisionEnter,

    //PlayerState
    OnGameStateChange,
    OnPhysicStateChange,
    OnRotateStateChange,
    OnArmStateChange,

    //PlayerInput
    OnExtendArm,
    OnHoldArm,
    OnRotate,
}