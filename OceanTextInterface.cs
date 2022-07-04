using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OceanGame
{
    public class OceanTextInterface : IOceanInterface
    {
        private void DisplayHorizontalBorders(int length)
        {
            Console.Write("+");
            for (int i = 0; i < length; i++)
            {
                Console.Write("-");
            }
            Console.Write("+\n");
        }
        public void DisplayField(in Cell[,] field)
        {
            Console.Clear();
            DisplayHorizontalBorders(field.GetLength(1));

            for (int i = 0; i < field.GetLength(0); i++)
            {
                Console.Write('|');
                for (int j = 0; j < field.GetLength(1); j++)
                {
                    Console.Write(field[i, j].image);
                }
                Console.Write('|');
                Console.Write("\n");
            }

            DisplayHorizontalBorders(field.GetLength(1));
        }

        public void DisplayLegend()
        {
            Console.WriteLine("Map Legend");
            Console.WriteLine($"{GameSettings.VoidImage} Empty Cell");
            Console.WriteLine($"{GameSettings.ObstacleImage} Obstacle");
            Console.WriteLine($"{GameSettings.PreyImage} Prey");
            Console.WriteLine($"{GameSettings.PredatorImage} Predator");
        }

        public void DisplayStats(in GameStats stats)
        {
            Console.WriteLine("Map Stats");
            Console.WriteLine($"Cycle:     {stats.cycle}");
            Console.WriteLine($"Predators: {stats.predators}");
            Console.WriteLine($"Prey:      {stats.prey}");
            Console.WriteLine($"Obstacles: {stats.obstacles}");
        }
    }
}
