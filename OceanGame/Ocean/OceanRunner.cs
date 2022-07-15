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
        private bool _paused = false;
        private Mutex _mutex = new Mutex();
        private bool _alive;
        public OceanRunner(Ocean ocean, IOceanView view, int clock, int cycles)
        {
            _ocean = ocean;
            _oceanView = view;
            _cycles = cycles;
            _alive = true;

            int frameInterval = 1000 / clock;

            var clockThread = new Thread(() => {
                while (_cycles != 0)
                {
                    _mutex.WaitOne();
                    if (!_paused && _alive)
                    {
                        TryStep();
                        _cycles--;
                        Thread.Sleep(frameInterval);
                    }
                    _mutex.ReleaseMutex();
                }
                _alive = false;
            });

            clockThread.IsBackground = true;
            clockThread.Start();
        }

        private void TryStep()
        {
            try
            {
                _ocean.Step();
            }
            catch (IndexOutOfRangeException)
            {
                _oceanView.DisplayMessage("Error: Tried to place cell out of field");
                _alive = false;
            }
        }

        public void Step()
        {
            _mutex.WaitOne();
            if (_paused && _alive)
            {
                TryStep();
                _cycles--;
            }
            if (_cycles == 0)
            {
                _alive = false;
            }
            _mutex.ReleaseMutex();
        }

        public void Switch()
        {
            _mutex.WaitOne();
            _paused = !_paused;
            _mutex.ReleaseMutex();
        }

        public bool IsAlive()
        {
            return _alive;
        }
    }
}
