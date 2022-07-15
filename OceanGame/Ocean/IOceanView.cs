using System;

namespace OceanGame
{
    public interface IOceanView
    {
        public int oceanWidth { get; }
        public int oceanHeight { get; }
        public void Display(in Cell[,] field, in GameStats stats);

        public void DisplayMessage(string message);

        public event EventHandler PauseReceived;

        public event EventHandler StepReceived;
    }
}
