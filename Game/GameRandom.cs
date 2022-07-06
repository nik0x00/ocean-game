using System;
using System.Collections.Generic;
using System.Linq;

namespace OceanGame
{
    public class GameRandom : Random
    {
        private static List<(char img, double ratio)> ratios = new List<(char, double)>
        {
            new (GameSettings.ObstacleImage, GameSettings.ObstacleRatio),
            new (GameSettings.PredatorImage, GameSettings.PredatorRatio),
            new (GameSettings.PreyImage, GameSettings.PreyRatio),
            new (GameSettings.VoidImage, 1)
        };

        public char NextCellImage()
        {
            var rands = ratios.Select(r => (r.img, NextDouble() * r.ratio));

            return rands.OrderBy(rand => rand.Item2).Last().img;
        }
    }
}
