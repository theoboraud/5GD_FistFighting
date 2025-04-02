/// <summary>
/// Global Game Events
/// </summary>
public enum GameEvent
{
    //GameManager
    OnStartRound,
    OnNewGameRound,
    OnGameReset,
    OnLoadOutro,

    //LevelManager
    OnLoadScene,

    //SpawnPoints
    OnSpawnPointsInit,

    //Player
    OnPlayerDead,

    //PlayerManager
    OnPlayerJoin,
    
    OnShowScore,
    
    OnPauseGame,
    OnResumeGame,
    
    // Menu
    OnMenuUp,
    OnMenuDown,
    OnMenuLeft,
    OnMenuRight,
    OnMenuValidate
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
    
    // Game Flow
    NewRound,
    OnPlayerReady,

    // Character Select
    OnNextCharacter,
    OnPreviousCharacter,

    // Item Usage
    OnUseItem,
    OnKillSelf,
}