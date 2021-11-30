namespace Enums
{
    public enum GlobalGameState
    {
        InPlay,
        WinnerScreen,
        ScoreScreen,
        PlayerWon,
        MainMenu,
        OptionsMenu,
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
