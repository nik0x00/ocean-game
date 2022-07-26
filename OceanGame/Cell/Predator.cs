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
                ocean.TrySetCell(x, y, new Cell());
                ocean.OnPredatorDied();
                return;
            }

            if (Heat > 0)
            {
                Heat--;
            }

            DirectionTools.DirectionRandomIterate((int nx, int ny) =>
            {
                nx += x;
                ny += y;

                Cell cell = ocean.GetCell(nx, ny);

                if (cell.Image == GameSettings.PreyImage)
                {
                    timeToFeed = _settings.PredatorTimeToFeed;
                    cell = new Cell();
                    ocean.TrySetCell(nx, ny, cell);
                    ocean.OnPreyConsumed();
                    return false;
                }

                if (cell.Image == GameSettings.VoidImage)
                {
                    if (timeToReproduce <= 0)
                    {
                        ResetReproduce();
                        var predator = new Predator(_settings);
                        predator.HeatUp();
                        ocean.TrySetCell(nx, ny, predator);
                        ocean.OnPredatorReproduced();
                    }
                    else
                    {
                        cell.HeatUp();
                        HeatUp();
                        ocean.TrySetCell(x, y, cell);
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
