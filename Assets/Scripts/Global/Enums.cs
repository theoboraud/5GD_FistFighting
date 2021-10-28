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

    public enum GameMode
    {
        Race,
        Versus
    }

    public enum PlayerGameState
    {
        Dead,
        Ready,
        NotReady
    }

    public enum PlayerPhysicState
    {
        OnGround,
        OnAir,
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

    public enum NavType
    {
        Vertical,
        Horizontal,
        TwoDimensions
    }
}
