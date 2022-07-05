using System;

namespace OceanGame
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var oceanInterface = new OceanTextInterface(GameSettings.OceanWidth + 10, GameSettings.OceanHeight + 16);
            Ocean ocean = new Ocean(GameSettings.OceanWidth, GameSettings.OceanHeight, oceanInterface);
            ocean.Run(GameSettings.GameCycles);
        }
    }
}
