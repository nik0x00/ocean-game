using System.Collections.Generic;
using System.Threading;
using System;

namespace OceanGame
{
    public class Ocean : IOceanCell
    {
        private Cell[,] _cells;

        private int _width;
        private int _height;

        private GameStats _stats;

        public Ocean(int width, int height)
        {
            _width = width;
            _height = height;

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

        public void TrySetCell(int x, int y, Cell cell)
        {
            if (IsOutOfBorder(x, y))
            {
                throw new IndexOutOfRangeException("Cell coordinates is out of game field");
            }
            _cells[y, x] = cell;
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

        public event OceanDataChangedEventHandler OceanDataChanged;

        public void Step()
        {
            _stats.NextCycle();

            if (OceanDataChanged != null)
            {
                OceanDataChanged(this, new OceanDataChangedEventArgs(_cells, _stats));
            }

            Process();
        }
    }

    public class OceanDataChangedEventArgs : EventArgs
    {
        public Cell[,] Field;
        public GameStats Stats;

        public OceanDataChangedEventArgs(Cell[,] field, GameStats stats)
        {
            Field = field;
            Stats = stats;
        }
    }

    public delegate void OceanDataChangedEventHandler(object sender, OceanDataChangedEventArgs e);
}
