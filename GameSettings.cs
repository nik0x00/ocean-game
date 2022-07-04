using System;

namespace OceanGame
{
    public class GameSettings
    {
        public const char ObstacleImage = '#';
        public const char VoidImage = ' ';
        public const char PreyImage = '*';
        public const char PredatorImage = '@';

        public const double PredatorRatio = 0.15;
        public const int PredatorTimeToFeed = 20;
        public const int PredatorTimeToReproduce = 30;

        public const double PreyRatio = 0.31;
        public const int PreyTimeToReproduce = 30;

        public const double ObstacleRatio = 0.22;

        public const int CycleInterval = 50;

        public const int GameCycles = 1000;
        public const int OceanWidth = 80;
        public const int OceanHeight = 30;
    }
}
