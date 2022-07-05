using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OceanGame
{
    public interface IOceanInterface
    {
        public void Display(in Cell[,] field, in GameStats stats);
    }
}
