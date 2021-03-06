using System;

namespace OceanGame
{
    public class GameSettings
    {
        public const char ObstacleImage = '#';
        public const char VoidImage = ' ';
        public const char PreyImage = '*';
        public const char PredatorImage = '@';
        public const int HeatLength = 7;

        public double PredatorRatio = 0.15;
        public double ObstacleRatio = 0.22;
        public double PreyRatio = 0.31;

        public int PreyTimeToReproduce = 30;
        public int PredatorTimeToReproduce = 30;
        public int PredatorTimeToFeed = 20;

        public int MaxFramesPerSecond = 20;

        public int GameCycles = 150;
        public int OceanWidth = 80;
        public int OceanHeight = 30;
    }
}
