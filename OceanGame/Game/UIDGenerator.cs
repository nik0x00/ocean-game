using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OceanGame
{
    public class UIDGenerator
    {
        private int currentID = 0;

        public int Generate()
        {
            currentID++;
            return currentID;
        }
    }
}
