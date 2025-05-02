/// <summary>
/// Global Game Events
/// </summary>
public enum GameEvent
{
    //GameManager
    OnEnterLobby,
    OnNewGameRound,//Call on new game round start
    OnNewStage, //On new game stage loaded
    OnPauseGame,
    OnResumeGame,
    OnStageEnd, //On the end of each stage
    OnShowScore,
    OnGameReset,
    OnLoadOutro,
    OnGameStateChange, //On game state change

    //LevelManager
    OnLoadScene,

    //SpawnPoints
    OnSpawnPointsInit,

    //Player
    OnPlayerDead,

    //PlayerManager
    OnPlayerJoin,
    
    //Input Receiver
    OnInputModeChange,
    OnStartInput, //OnStartInput, global event

    // Menu
    OnMenuUp,
    OnMenuDown,
    OnMenuLeft,
    OnMenuRight,
}

/// <summary>
/// Local Events for player
/// </summary>
public enum PlayerEvent
{
    //Player
    OnPlayerSpawn,

    //PlayerController
    OnPlayerCollisionEnter,

    //PlayerState
    OnGameStateChange,
    OnPhysicStateChange,
    OnRotateStateChange,
    OnArmStateChange,

    //PlayerData
    OnPlayerLivesChange,
    OnPlayerScoreChange,


    //PlayerInput
    OnExtendArm,
    OnHoldArm,
    OnRotate,
    OnStartInput, //On gameplay start input, individual player input
    
    // Game Flow
    NewStage,

    // Character Select
    OnNextCharacter,
    OnPreviousCharacter,

    // Item Usage
    OnUseItem,
    OnKillSelf,
}