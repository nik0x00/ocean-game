using System;
using System.Collections.Generic;
using System.Threading;

namespace OceanGame
{
    public class Ocean
    {
        private IOceanInterface _interface;

        private Cell[,] _cells;

        private int _width;
        private int _height;

        private GameStats _stats;

        public Ocean(int width, int height, IOceanInterface oceanInterface)
        {
            _width = width;
            _height = height;
            _interface = oceanInterface;

            _stats.Reset();

            InitCells();
        }

        private Cell GenerateCellFromImage(char img)
        {
            switch (img)
            {
                case GameSettings.ObstacleImage:
                    _stats.obstacles++;
                    return Globals.obstacleSingle;
                case GameSettings.PredatorImage:
                    _stats.predators++;
                    return new Predator();
                case GameSettings.PreyImage:
                    _stats.prey++;
                    return new Prey();
                default:
                    return Globals.cellSingle;
            }
        }

        private void InitCells()
        {
            _cells = new Cell[_height, _width];

            for (int i = 0; i < _height; i++)
            {
                for (int j = 0; j < _width; j++)
                {
                    _cells[i, j] = GenerateCellFromImage(Globals.gameRandom.NextCellImage());
                }
            }
        }
        private bool IsOutOfBorder(int x, int y)
        {
            return x >= _width || y >= _height || x < 0 || y < 0;
        }

        public Cell GetCell(int x, int y)
        {
            if (IsOutOfBorder(x, y))
            {
                return Globals.obstacleSingle;
            }

            return _cells[y, x];
        }

        public void SetCellOrNothing(int x, int y, Cell cell)
        {
            if (!IsOutOfBorder(x, y))
            {
                _cells[y, x] = cell;
            }
        }

        public void OnPreyConsumed()
        {
            _stats.prey--;
        }

        public void OnPreyReproduced()
        {
            _stats.prey++;
        }

        public void OnPredatorDied()
        {
            _stats.predators--;
        }

        public void OnPredatorReproduced()
        {
            _stats.predators++;
        }

        private void Process()
        {
            var processed = new HashSet<int>();

            for (int i = 0; i < _height; i++)
            {
                for (int j = 0; j < _width; j++)
                {
                    int uid = _cells[i, j].uid;

                    if (!processed.Contains(uid))
                    {
                        _cells[i, j].Process(j, i, this);
                        processed.Add(uid);
                    }
                }
            }
        }

        public void Run(int cycles)
        {
            for(int i = 0; i < cycles; i++)
            {
                _stats.NextCycle();

                _interface.DisplayField(_cells);
                _interface.DisplayStats(_stats);
                _interface.DisplayLegend();

                Process();

                Thread.Sleep(GameSettings.CycleInterval);
            }
        }
    }
}
