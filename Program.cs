using System;
using System.Threading;
using System.Runtime.InteropServices;

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
            Console.Clear();
            Console.SetWindowSize(GameSettings.OceanWidth + 10, GameSettings.OceanHeight + 16);


            var oceanInterface = new OceanTextInterface(GameSettings.OceanWidth, GameSettings.OceanHeight);
            Ocean ocean = new Ocean(oceanInterface);


            int i = 0;
            bool simulationActive = true;
            while (i < GameSettings.GameCycles)
            {
                if (Console.KeyAvailable)
                {
                    var key = Console.ReadKey(true);
                    if (key.Key == ConsoleKey.P)
                    {
                        simulationActive = !simulationActive;
                    }
                    if (key.Key == ConsoleKey.Escape)
                    {
                        PPrint("Game execution forcibly stopped");
                        break;
                    }
                }

                if (simulationActive)
                {
                    try
                    {
                        ocean.Step();
                    }
                    catch (System.IndexOutOfRangeException)
                    {
                        PPrint("Error: tried to place cell out of field");
                    }
                    i++;
                }
            }
        }

        private static void PPrint(string msg)
        {
            for (int i = 0; i < msg.Length + 4; i++)
            {
                Console.Write("*");
            }

            Console.Write($"\n* {msg} *\n");

            for (int i = 0; i < msg.Length + 4; i++)
            {
                Console.Write("*");
            }
            Console.Write("\n");
        }
    }


}
