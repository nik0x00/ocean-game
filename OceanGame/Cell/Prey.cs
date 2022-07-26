namespace OceanGame
{
    public class Prey : Cell
    {
        public override char Image { get; } = GameSettings.PreyImage;
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
            timeToReproduce = Globals.GRandom.Next(defaultTimeToReproduce) + (defaultTimeToReproduce / 2);
        }

        public override void Process(int x, int y, IOceanCell ocean)
        {
            if (Heat > 0)
            {
                Heat--;
            }

            DirectionTools.DirectionRandomIterate((int nx, int ny) =>
            {
                nx += x;
                ny += y;

                Cell cell = ocean.GetCell(nx, ny);

                if (cell.Image == GameSettings.VoidImage)
                {
                    if (timeToReproduce <= 0)
                    {
                        ResetReproduce();
                        var prey = new Prey(_settings);
                        prey.HeatUp();
                        ocean.TrySetCell(nx, ny, prey);
                        ocean.OnPreyReproduced();
                    }
                    else
                    {
                        HeatUp();
                        cell.HeatUp();
                        ocean.TrySetCell(x, y, cell);
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
