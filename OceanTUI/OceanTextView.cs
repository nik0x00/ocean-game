using System;
using OceanGame;

namespace OceanTUI
{
    public class OceanTextView : IOceanView
    {
        private int currentCursorY = 0;

        private Cell[,] prevFieldState = null;
        private bool isBordersPrinted = false;

        private int baseOffset;

        public event EventHandler PauseReceived;
        public event EventHandler StepReceived;
        public event EventHandler ForceEndReceived;

        public GameSettings ViewGameSettings { get; }
        public OceanTextView(GameSettings settings, int offset = 0)
        {
            Console.CursorVisible = false;
            ViewGameSettings = settings;    
            baseOffset = offset;
            prevFieldState = new Cell[ViewGameSettings.OceanHeight, ViewGameSettings.OceanWidth];
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
        private void PrintBorder()
        {
            DisplayHorizontalBorders(ViewGameSettings.OceanWidth);
            for (int i = 0; i < ViewGameSettings.OceanHeight; i++)
            {
                Console.Write('|');
                for (int j = 0; j < ViewGameSettings.OceanWidth; j++)
                {
                    Console.Write(' ');
                }
                Console.Write("|\n");
            }
            DisplayHorizontalBorders(ViewGameSettings.OceanWidth);
        }
        private void SetCell(int x, int y, Cell cell)
        {
            Console.SetCursorPosition(x + 1, currentCursorY + y + 1);
            Console.Write(cell.Image);
        }
        private void DisplayField(in Cell[,] field)
        {
            currentCursorY = Console.GetCursorPosition().Top;

            if (!isBordersPrinted)
            {
                PrintBorder();
                isBordersPrinted = true;
            }

            for (int i = 0; i < ViewGameSettings.OceanHeight; i++)
            {
                for (int j = 0; j < ViewGameSettings.OceanWidth; j++)
                {
                    if (prevFieldState[i, j] == null || prevFieldState[i, j].UID != field[i, j].UID)
                    {
                        SetCell(j, i, field[i, j]);
                    }
                    prevFieldState[i, j] = field[i, j];
                }
            }

            Console.SetCursorPosition(0, currentCursorY + ViewGameSettings.OceanHeight + 2);
        }

        private void DisplayLegend()
        {
            Console.WriteLine("Map Legend");
            Console.WriteLine($"{GameSettings.VoidImage} Empty Cell");
            Console.WriteLine($"{GameSettings.ObstacleImage} Obstacle");
            Console.WriteLine($"{GameSettings.PreyImage} Prey");
            Console.WriteLine($"{GameSettings.PredatorImage} Predator");
        }

        private void DisplayStats(in GameStats stats)
        {
            Console.WriteLine("Map Stats");
            Console.WriteLine($"Cycle:     {stats.cycle}    ");
            Console.WriteLine($"Predators: {stats.predators}    ");
            Console.WriteLine($"Prey:      {stats.prey}    ");
            Console.WriteLine($"Obstacles: {stats.obstacles}    ");
        }

        public void Display(in Cell[,] field, in GameStats stats)
        {
            Console.SetCursorPosition(0, baseOffset);
            DisplayLegend();
            DisplayField(field);
            DisplayStats(stats);
        }

        public void Pause()
        {
            if (PauseReceived != null)
            {
                PauseReceived(this, null);
            }
        }

        public void Step()
        {
            if (StepReceived != null)
            {
                StepReceived(this, null);
            }
        }

        public void ForceEnd()
        {
            if (ForceEndReceived != null)
            {
                ForceEndReceived(this, null);
            }
        }

        public void DisplayMessage(string message)
        {
            for (int i = 0; i < message.Length + 4; i++)
            {
                Console.Write("*");
            }
            Console.Write($"\n* {message} *\n");
            for (int i = 0; i < message.Length + 4; i++)
            {
                Console.Write("*");
            }
            Console.Write("\n");
        }
    }
}
