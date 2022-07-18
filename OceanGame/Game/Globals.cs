using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OceanGame
{
    public class Globals
    {
        public static Random random = new Random();
        public static UIDGenerator gameUIDGenerator = new UIDGenerator();
        public static Cell cellSingle = new Cell();
        public static Obstacle obstacleSingle = new Obstacle();
    }
}
