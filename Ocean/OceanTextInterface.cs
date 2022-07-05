using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OceanGame
{
    public class OceanTextInterface : IOceanInterface
    {
        private int lastCursorYField = 0;
        private int lastCursorYLegend = 0;
        private int lastCursorYStats = 0;
        private bool firstDisplayed = true;

        public OceanTextInterface(int width, int height)
        {
            Console.SetWindowSize(width, height);
            Console.Clear();
        }

        private bool IsFirstDisplayed()
        {
            if (firstDisplayed)
            {
                firstDisplayed = false;
                return true;
            } 
            return false;
        }

        private void DisplayHorizontalBorders(int length)
        {
            Console.Write("+");
            for (int i = 0; i < length; i++)
            {
                Console.Write("-");
            }
            Console.Write("+\n");
        }
        private void DisplayField(in Cell[,] field)
        {
            if (lastCursorYField > 0 || IsFirstDisplayed())
            {
                Console.SetCursorPosition(0, lastCursorYField);
            }
            lastCursorYField = Console.GetCursorPosition().Top;

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

        private void DisplayLegend()
        {
            if (lastCursorYLegend > 0 || IsFirstDisplayed())
            {
                Console.SetCursorPosition(0, lastCursorYLegend);
            }
            lastCursorYLegend = Console.GetCursorPosition().Top;

            Console.WriteLine("Map Legend");
            Console.WriteLine($"{GameSettings.VoidImage} Empty Cell");
            Console.WriteLine($"{GameSettings.ObstacleImage} Obstacle");
            Console.WriteLine($"{GameSettings.PreyImage} Prey");
            Console.WriteLine($"{GameSettings.PredatorImage} Predator");
        }

        private void DisplayStats(in GameStats stats)
        {
            if (lastCursorYStats > 0 || IsFirstDisplayed())
            {
                Console.SetCursorPosition(0, lastCursorYStats);
            }
            lastCursorYStats = Console.GetCursorPosition().Top;

            Console.WriteLine("Map Stats");
            Console.WriteLine($"Cycle:     {stats.cycle}    ");
            Console.WriteLine($"Predators: {stats.predators}    ");
            Console.WriteLine($"Prey:      {stats.prey}    ");
            Console.WriteLine($"Obstacles: {stats.obstacles}    ");
        }

        public void Display(in Cell[,] field, in GameStats stats)
        {
            firstDisplayed = true;

            DisplayLegend();
            DisplayField(field);
            DisplayStats(stats);
        }
    }
}
