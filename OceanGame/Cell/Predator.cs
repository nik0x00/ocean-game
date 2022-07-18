namespace OceanGame
{
    public class Predator : Prey
    {
        public override char Image { get; } = GameSettings.PredatorImage;

        private int timeToFeed;

        public Predator(GameSettings settings) : base(settings: settings)
        {
            defaultTimeToReproduce = _settings.PredatorTimeToReproduce;
            timeToFeed = _settings.PredatorTimeToFeed;
            ResetReproduce();
        }

        public override void Process(int x, int y, IOceanCell ocean)
        {
            if (timeToFeed <= 0)
            {
                ocean.TrySetCell(x, y, Globals.CellSingle);
                ocean.OnPredatorDied();
                return;
            }

            DirectionTools.DirectionRandomIterate((int nx, int ny) =>
            {
                nx += x;
                ny += y;

                Cell cell = ocean.GetCell(nx, ny);

                if (cell.Image == GameSettings.PreyImage)
                {
                    timeToFeed = _settings.PredatorTimeToFeed;
                    cell = Globals.CellSingle;
                    ocean.TrySetCell(nx, ny, cell);
                    ocean.OnPreyConsumed();
                    return false;
                }
                if (cell.Image == GameSettings.VoidImage)
                {
                    if (timeToReproduce <= 0)
                    {
                        ResetReproduce();
                        ocean.TrySetCell(nx, ny, new Predator(_settings));
                        ocean.OnPredatorReproduced();
                    }
                    else
                    {
                        ocean.TrySetCell(x, y, Globals.CellSingle);
                        ocean.TrySetCell(nx, ny, this);
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
