﻿namespace OceanGame
{
    public class Predator : Prey
    {
        public override char image { get; } = GameSettings.PredatorImage;
        protected override int defaultTimeToReproduce { get; } = GameSettings.PredatorTimeToReproduce;
        private int timeToFeed = GameSettings.PredatorTimeToFeed;

        public override void Process(int x, int y, IOceanCell ocean)
        {
            if (timeToFeed <= 0)
            {
                ocean.SetCellOrNothing(x, y, Globals.cellSingle);
                ocean.OnPredatorDied();
                return;
            }

            DirectionTools.DirectionRandomIterate((int nx, int ny) =>
            {
                nx += x;
                ny += y;

                Cell cell = ocean.GetCell(nx, ny);

                if (cell.image == GameSettings.PreyImage)
                {
                    timeToFeed = GameSettings.PredatorTimeToFeed;
                    cell = Globals.cellSingle;
                    ocean.SetCellOrNothing(nx, ny, cell);
                    ocean.OnPreyConsumed();
                    return false;
                }
                if (cell.image == GameSettings.VoidImage)
                {
                    if (timeToReproduce <= 0)
                    {
                        ResetReproduce();
                        ocean.SetCellOrNothing(nx, ny, new Predator());
                        ocean.OnPredatorReproduced();
                    }
                    else
                    {
                        ocean.SetCellOrNothing(x, y, Globals.cellSingle);
                        ocean.SetCellOrNothing(nx, ny, this);
                    }
                    return false;
                }
                return true;
            });

            timeToReproduce--;
            timeToFeed--;
        }
    }
}
