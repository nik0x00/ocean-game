using System;
using OceanGame;
using System.Threading;
using System.Runtime.InteropServices;

namespace OceanTUI
{
    internal class Program
    {
        private static bool[] keys = new bool[255];  //KeyUp states
        private static bool[] press = new bool[255]; // Press states

        [DllImport("user32.dll")]
        private static extern ushort GetAsyncKeyState(int vKey);

        static void Main(string[] args)
        {
            var settings = new GameSettings();


            Console.WriteLine("Press any key to start");
            Console.WriteLine("Press P to pause");
            Console.WriteLine("Press S to make single step if paused");
            Console.WriteLine("Press C to change configuration");

            Console.WriteLine("\nOcean:");
            Console.WriteLine($"Width:              {settings.OceanWidth}");
            Console.WriteLine($"Height:             {settings.OceanHeight}");
            Console.WriteLine($"Cycles:             {settings.GameCycles}");

            Console.WriteLine("\nRatios:");
            Console.WriteLine($"Obstacle:           {settings.ObstacleRatio}");
            Console.WriteLine($"Predator:           {settings.PredatorRatio}");
            Console.WriteLine($"Prey:               {settings.PreyRatio}");

            Console.WriteLine("\nCycles:");
            Console.WriteLine($"Predator reproduce: {settings.PredatorTimeToReproduce}");
            Console.WriteLine($"Predator feed:      {settings.PredatorTimeToFeed}");
            Console.WriteLine($"Prey reproduce:     {settings.PreyTimeToReproduce}");

            var key = Console.ReadKey();

            if (key.Key == ConsoleKey.C)
            {
                var menu = new SettingsEditMenuText();
                var editor = new GameSettingsEditor(menu);

                editor.OnSettingsEdited += (s, e) =>
                {
                    settings = e.settings;
                };

                editor.Edit(settings);
            }

            Console.Clear();
            Console.SetWindowSize(settings.OceanWidth + 10, settings.OceanHeight + 16);

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
