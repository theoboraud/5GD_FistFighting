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

}