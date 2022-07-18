using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

namespace OceanGame
{
    public class OceanRunner
    {
        private Ocean _ocean;
        private IOceanView _oceanView;
        private int _cycles;
        private bool _paused = true;
        private bool _alive;

        public OceanRunner(Ocean ocean, IOceanView view, int clock, int cycles)
        {
            _ocean = ocean;
            _oceanView = view;
            _cycles = cycles;
            _alive = true;

            int frameInterval = 1000 / clock;

            var clockThread = new Thread(() => {
                while (_cycles > 0)
                {
                    if (!_paused && _alive)
                    {
                        TryStep();
                        Thread.Sleep(frameInterval);
                    }
                }
            });

            clockThread.IsBackground = true;
            clockThread.Start();
        }

        private void TryStep()
        {
            try
            {
                _ocean.Step();
                _cycles--;
                if (_cycles == 0)
                {
                    _oceanView.DisplayMessage("Game End");
                    _alive = false;
                }
            }
            catch (IndexOutOfRangeException)
            {
                _alive = false;
                _oceanView.DisplayMessage("Error: Tried to place cell out of field");
            }
        }

        public void Step()
        {
            if (_paused && _alive)
            {
                TryStep();
            }
        }

        public void Switch()
        {
            _paused = !_paused;
        }

        public bool IsAlive()
        {
            return _alive;
        }
    }
}
