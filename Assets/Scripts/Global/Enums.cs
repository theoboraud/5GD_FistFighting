namespace Enums
{
    public enum GameState
    {
        Intro,
        MainMenu,
        LobbyWaiting,
        AllReady,
        PrePlayCountdown,
        InPlay,
        Paused,
        EndStage,
        ScoreScreen,
        Outro
    }

    public enum GameMode
    {
        Race,
        Versus
    }

    public enum PlayerGameState
    {
        Alive,
        Dead,
        Ready,
        NotReady,
        Invincible
    }

    public enum PlayerPhysicState
    {
        OnGround,
        InAir,
        IsHit
    }

    public enum PlayerArmState
    {
        Extending,
        Extended,
        Unextending,
        Ready,
        NotReady,
        Hold
    }

    public enum PlayerRotateState
    {
        Ready,
        RotatingRight,
        RotatingLeft,
        OnCooldown
    }

    public enum NavType
    {
        Vertical,
        Horizontal,
        TwoDimensions
    }
}
