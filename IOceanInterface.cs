using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OceanGame
{
    public interface IOceanInterface
    {
        public void DisplayField(in Cell[,] field);
        public void DisplayStats(in GameStats stats);
        public void DisplayLegend();
    }
}
