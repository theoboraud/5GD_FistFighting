namespace Enums
{
    public enum GlobalGameState
    {
        InPlay,
        MainMenu,
        CharacterSelectMenu,
        LevelSelectMenu,
        OptionsMenu,
    }

    public enum PlayerGameState
    {
        Dead,
        Ready,
        NotReady
    }

    public enum GameMode
    {
        Race,
        Versus
    }

    public enum PlayerArmState
    {
        Extending,
        Extended,
        Unextending,
        Default
    }

    public enum NavType
    {
        Vertical,
        Horizontal,
        TwoDimensions
    }

    public enum PlayerPhysicState
    {
        OnGround,
        OnAir,
        isHit
    }
}
