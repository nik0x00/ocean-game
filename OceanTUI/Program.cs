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
            Console.WriteLine("Press any key to start");
            Console.WriteLine("Press P to pause");
            Console.WriteLine("Press S to make single step if paused");

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

            var oceanView = new OceanTextView(GameSettings.OceanWidth, GameSettings.OceanHeight);
            var oceanController = new OceanController(oceanView, GameSettings.FramesPerSecond, GameSettings.GameCycles);


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
                    oceanView.DisplayMessage("Game forcibly stopped");
                    oceanView.Pause();
                    break;
                }
            }
            // If not force exit
            if (!oceanController.IsOceanAlive()) 
            {
                oceanView.DisplayMessage("Game End");
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
