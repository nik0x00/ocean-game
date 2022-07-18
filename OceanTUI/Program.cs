using System;
using OceanGame;
using System.Threading;
using System.Runtime.InteropServices;

namespace OceanTUI
{
    internal class Program
    {
        static bool[] keys = new bool[255];  //KeyUp states
        static bool[] press = new bool[255]; // Press states

        [DllImport("user32.dll")]
        static extern ushort GetAsyncKeyState(int vKey);

        static void Main(string[] args)
        {
            var settings = new GameSettings();


            Console.WriteLine("Press any key to start");
            Console.WriteLine("Press P to pause");
            Console.WriteLine("Press S to make single step if paused");

            Console.WriteLine("\nOcean:");
            Console.WriteLine($"Width:              {settings.oceanWidth}");
            Console.WriteLine($"Height:             {settings.oceanHeight}");
            Console.WriteLine($"Cycles:             {settings.GameCycles}");

            Console.WriteLine("\nRatios:");
            Console.WriteLine($"Obstacle:           {settings.ObstacleRatio}");
            Console.WriteLine($"Predator:           {settings.PredatorRatio}");
            Console.WriteLine($"Prey:               {settings.PreyRatio}");

            Console.WriteLine("\nCycles:");
            Console.WriteLine($"Predator reproduce: {settings.PredatorTimeToReproduce}");
            Console.WriteLine($"Predator feed:      {settings.PredatorTimeToFeed}");
            Console.WriteLine($"Prey reproduce:     {settings.PreyTimeToReproduce}");

            Console.ReadKey();
            Console.Clear();
            Console.SetWindowSize(settings.oceanWidth + 10, settings.oceanHeight + 16);

            var oceanView = new OceanTextView(settings);
            var oceanController = new OceanController(oceanView);
            
            oceanView.Pause();
            while (oceanController.IsOceanAlive())
            {
                UpdateKeyStates();
                if (WasKeyUp('P'))
                {
                    oceanView.Pause();
                }
                if (WasKeyUp('S'))
                {
                    oceanView.Step();
                }
                // Esc
                if (WasKeyUp(27))
                {
                    oceanView.ForceEnd();
                    break;
                }
            }
            Console.ReadKey();
        }

        private static bool WasKeyUp(int key)
        {
            // Get KeyUp state and clear
            bool b = keys[key];
            keys[key] = false;
            return b;
        }

        private static void UpdateKeyStates()
        {
            for (int i = 0; i < keys.Length; i++)
            {
                bool state = GetAsyncKeyState(i) != 0;
                // if wasn`t pressed
                if (state && !press[i]) 
                {
                    press[i] = true;
                }
                // if was pressed
                else if (!state && press[i]) 
                {
                    keys[i] = true; // set KeyUp
                    press[i] = false; // clear press
                }
            }
        }
    }
}
