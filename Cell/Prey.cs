using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OceanGame
{
    public class Prey : Cell
    {
        public override char image { get; } = GameSettings.PreyImage;
        protected virtual int defaultTimeToReproduce { get; } = GameSettings.PreyTimeToReproduce;
        protected int timeToReproduce;

        public Prey()
        {
            ResetReproduce();
        }
        protected void ResetReproduce()
        {
            timeToReproduce = Globals.gameRandom.Next(defaultTimeToReproduce) + (defaultTimeToReproduce / 2);
        }
        public override void Process(int x, int y, Ocean ocean)
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
                        ocean.SetCellOrNothing(nx, ny, new Prey());
                        ocean.OnPreyReproduced();
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
        }
    }
}
