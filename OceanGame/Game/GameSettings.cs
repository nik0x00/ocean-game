using System;

namespace OceanGame
{
    public class GameSettings
    {
        public const char ObstacleImage = '#';
        public const char VoidImage = ' ';
        public const char PreyImage = '*';
        public const char PredatorImage = '@';

        public double PredatorRatio = 0.15;
        public int PredatorTimeToFeed = 20;
        public int PredatorTimeToReproduce = 30;

        public double PreyRatio = 0.31;
        public int PreyTimeToReproduce = 30;

        public double ObstacleRatio = 0.22;

        public int FramesPerSecond = 20;

        public int GameCycles = 150;
        public int oceanWidth = 80;
        public int oceanHeight = 30;
    }
}
