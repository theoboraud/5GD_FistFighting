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
}

/// <summary>
/// Local Events for player
/// </summary>
public enum PlayerEvent
{
    //Player
    OnHit,
    OnPlayerInit,
    OnPlayerKilled,
    OnPlayerSpawn,
    OnPlayerHit,
    OnInvinciblityStart,
    OnInvinciblityStop,

    //PlayerController
    OnStunStart,
    OnStunStop,
    OnPlayerCollisionEnter,
    OnGround,
    OnAir,

    //PlayerState
    OnGameStateChange,
    OnPhysicStateChange,
    OnRotateStateChange,

    //PlayerInput
    OnExtendArm,
    OnHoldArm,
    OnRotate,
}