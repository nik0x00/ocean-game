namespace OceanGame
{
    public class Prey : Cell
    {
        public override char image { get; } = GameSettings.PreyImage;
        protected int defaultTimeToReproduce;
        protected int timeToReproduce;

        protected readonly GameSettings _settings;
        public Prey(GameSettings settings)
        {
            _settings = settings;
            defaultTimeToReproduce = settings.PreyTimeToReproduce;
            ResetReproduce();
        }

        public Prey()
        {
            ResetReproduce();
        }
        protected void ResetReproduce()
        {
            timeToReproduce = Globals.random.Next(defaultTimeToReproduce) + (defaultTimeToReproduce / 2);
        }

        public override void Process(int x, int y, IOceanCell ocean)
        {
            DirectionTools.DirectionRandomIterate((int nx, int ny) =>
            {
                nx += x;
                ny += y;

                Cell cell = ocean.GetCell(nx, ny);

                if (cell.image == GameSettings.VoidImage)
                {
                    if (timeToReproduce <= 0)
                    {
                        ResetReproduce();
                        ocean.TrySetCell(nx, ny, new Prey(_settings));
                        ocean.OnPreyReproduced();
                    }
                    else
                    {
                        ocean.TrySetCell(x, y, Globals.cellSingle);
                        ocean.TrySetCell(nx, ny, this);
                    }
                    return false;
                }

                return true;
            });

            timeToReproduce--;
        }
    }
}
