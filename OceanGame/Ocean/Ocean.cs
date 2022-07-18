using System.Collections.Generic;
using System.Threading;
using System;

namespace OceanGame
{
    public class Ocean : IOceanCell
    {
        private Cell[,] _cells;

        private GameSettings _settings;
        private GameRandom _random;

        private GameStats _stats;

        public Ocean(GameSettings settings)
        {
            _settings = settings;

            _random = new GameRandom(_settings);

            _stats.Reset();

            InitCells();
        }

        private Cell GenerateCellFromImage(char img)
        {
            switch (img)
            {
                case GameSettings.ObstacleImage:
                    _stats.obstacles++;
                    return Globals.ObstacleSingle;
                case GameSettings.PredatorImage:
                    _stats.predators++;
                    return new Predator(_settings);
                case GameSettings.PreyImage:
                    _stats.prey++;
                    return new Prey(_settings);
                default:
                    return Globals.CellSingle;
            }
        }

        private void InitCells()
        {
            _cells = new Cell[_settings.OceanHeight, _settings.OceanWidth];

            for (int i = 0; i < _settings.OceanHeight; i++)
            {
                for (int j = 0; j < _settings.OceanWidth; j++)
                {
                    _cells[i, j] = GenerateCellFromImage(_random.NextCellImage());
                }
            }
        }
        private bool IsOutOfBorder(int x, int y)
        {
            return x >= _settings.OceanWidth || y >= _settings.OceanHeight || x < 0 || y < 0;
        }

        public Cell GetCell(int x, int y)
        {
            if (IsOutOfBorder(x, y))
            {
                return Globals.ObstacleSingle;
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

            for (int i = 0; i < _settings.OceanHeight; i++)
            {
                for (int j = 0; j < _settings.OceanWidth; j++)
                {
                    int uid = _cells[i, j].UID;

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
