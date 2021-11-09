namespace Enums
{
    public enum GlobalGameState
    {
        InPlay,
        ScoreScreen,
        MainMenu,
        CharacterSelectMenu,
        LevelSelectMenu,
        OptionsMenu,
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
        NotReady
    }

    public enum PlayerPhysicState
    {
        OnGround,
        InAir,
        isHit
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
