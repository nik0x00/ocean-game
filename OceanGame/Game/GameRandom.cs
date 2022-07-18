using System;
using System.Collections.Generic;
using System.Linq;

namespace OceanGame
{
    public class GameRandom : Random
    {
        private List<(char img, double ratio)> ratios;

        public GameRandom(GameSettings settings)
        {
            ratios = new List<(char, double)>
            {
                new (GameSettings.ObstacleImage, settings.ObstacleRatio),
                new (GameSettings.PredatorImage, settings.PredatorRatio),
                new (GameSettings.PreyImage, settings.PreyRatio),
                new (GameSettings.VoidImage, 1)
            };
        }

        public char NextCellImage()
        {
            var rands = ratios.Select(r => (r.img, NextDouble() * r.ratio));

            return rands.OrderBy(rand => rand.Item2).Last().img;
        }
    }
}
