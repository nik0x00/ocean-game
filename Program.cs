using System;

namespace OceanGame
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Press any key to start");

            Console.WriteLine("\nOcean:");
            Console.WriteLine($"Width:              {GameSettings.OceanWidth}");
            Console.WriteLine($"Height:             {GameSettings.OceanHeight}");
            Console.WriteLine($"Cycles:             {GameSettings.GameCycles}");

            Console.WriteLine("\nRatios:");
            Console.WriteLine($"Obstacle:           {GameSettings.ObstacleRatio}");
            Console.WriteLine($"Predator:           {GameSettings.PredatorRatio}");
            Console.WriteLine($"Prey:               {GameSettings.PreyRatio}");

            Console.WriteLine("\nCycles:");
            Console.WriteLine($"Predator reproduce: {GameSettings.PredatorTimeToReproduce}");
            Console.WriteLine($"Predator feed:      {GameSettings.PredatorTimeToFeed}");
            Console.WriteLine($"Prey reproduce:     {GameSettings.PreyTimeToReproduce}");

            Console.ReadKey();

            var oceanInterface = new OceanTextInterface(GameSettings.OceanWidth + 10, GameSettings.OceanHeight + 16);
            Ocean ocean = new Ocean(GameSettings.OceanWidth, GameSettings.OceanHeight, oceanInterface);
            ocean.Run(GameSettings.GameCycles);
        }
    }
}
