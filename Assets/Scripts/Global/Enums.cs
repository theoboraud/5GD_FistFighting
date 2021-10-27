namespace Enums
{
    public enum GameState
    {
        InPlay,
        MainMenu,
        CharacterSelectMenu,
        LevelSelectMenu,
        OptionsMenu,
    }

    public enum PlayerState
    {
        Alive,
        Dead,
        Ready,
        NotReady
    }

    public enum GameMode
    {
        Race,
        Versus
    }

    public enum ArmState
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
}
