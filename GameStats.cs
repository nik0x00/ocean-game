using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OceanGame
{
    public struct GameStats
    {
        public int prey { get; set; }
        public int predators { get; set; }
        public int obstacles { get; set; }
        public int cycle { get; private set; }

        public void NextCycle()
        {
            cycle++;
        }

        public void Reset()
        {
            prey = 0;
            predators = 0;
            obstacles = 0;
            cycle = 0;
        }
    }
}
