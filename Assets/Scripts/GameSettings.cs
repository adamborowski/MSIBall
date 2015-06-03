using System;

namespace AssemblyCSharp
{
    // poziom trudnosci gry
    public enum GameDifficulty
    {
        EASY,
        MEDIUM,
        HARD
    }
    ;

    // opcja gry z modelem rozpoznawania emocji lub bez
    public enum GameMode
    {
        FIXED,
        EMOTIONAL
    }
    ;

    public class GameSettings
    {
        public GameMode gameMode;
        public GameDifficulty gameDifficulty;
    }
}

