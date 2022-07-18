using System;

namespace OceanGame
{
    public interface IOceanView
    {
        public GameSettings gameSettings { get; }
        public void Display(in Cell[,] field, in GameStats stats);

        public void DisplayMessage(string message);

        public event EventHandler ForceEndReceived;

        public event EventHandler PauseReceived;

        public event EventHandler StepReceived;
    }
}
