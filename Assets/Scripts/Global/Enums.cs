namespace Enums
{
    public enum GlobalGameState
    {
        Intro,
        InPlay,
        InPause,
        WinnerScreen,
        ScoreScreen,
        PlayerWon,
        MainMenu,
        OptionsMenu,
        Outro,
        Null,
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
        NotReady
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
