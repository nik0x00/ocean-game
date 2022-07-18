using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;

using OceanGame;

namespace OceanGUI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, IOceanView
    {
        private const int _spriteWidth = 16;
        private const int _spriteHeight = 16;
        private const int _padding = 1;

        private readonly OceanController _controller;

        private readonly ImageBrush _preyImg = ImageLoader.Load(@"pack://application:,,,/res/prey.png");
        private readonly ImageBrush _predatorImg = ImageLoader.Load(@"pack://application:,,,/res/predator.png");
        private readonly ImageBrush _obstacleImg = ImageLoader.Load(@"pack://application:,,,/res/obstacle.png");

        private readonly DispatcherTimer _oceanTimer = new DispatcherTimer();
        public MainWindow()
        {
            InitializeComponent();

            _controller = new OceanController(this, 1000, GameSettings.GameCycles);
            canvas.Focus();

            // We don`t use ocean runner thread here because Display() needs to be called from main thread
            // We are manually making steps with timer instead
            _oceanTimer.Tick += Step;
            _oceanTimer.Tick += WatchAlive;
            _oceanTimer.Interval = TimeSpan.FromMilliseconds(1000 / 20);

            statsText.Text = 
                "Press P to pause/unpause game\n" + 
                "Press S to make single step while paused";
        }

        public void Pause()
        {
            if (!_oceanTimer.IsEnabled)
            {
                _oceanTimer.Start();
            } 
            else
            {
                _oceanTimer.Stop();
            }
        }

        public void Step(object sender, EventArgs e)
        {
            if (StepReceived != null)
            {
                StepReceived(this, null);
            }
        }

        public void WatchAlive(object sender, EventArgs e)
        {
            if (!_controller.IsOceanAlive())
            {
                Application.Current.Shutdown();
            }
        }
        
        private void OnKeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.P)
            {
                Pause();
            }
            if (e.Key == Key.S && !_oceanTimer.IsEnabled)
            {
                Step(this, null);
            }
            if (e.Key == Key.Escape)
            {
                _oceanTimer.Stop();
                DisplayMessage("Game forcibly stopped");
                Application.Current.Shutdown();
            }
        }

        public int oceanWidth => (int)canvas.Width / (_spriteWidth + _padding*2);

        public int oceanHeight => (int)canvas.Height / (_spriteHeight + _padding * 2);

        public event EventHandler PauseReceived;
        public event EventHandler StepReceived;

        public void Display(in Cell[,] field, in GameStats stats)
        {
            canvas.Children.Clear();
            for (int i = 0; i < oceanHeight; i++)
            {
                for (int j = 0; j < oceanWidth; j++)
                {
                    DrawCell(field[i, j], j, i);
                }
            }
            statsText.Text =
                $"Map Stats\n" +
                $"Cycle:     {stats.cycle}\n" +
                $"Predators: {stats.predators}\n" +
                $"Prey:      {stats.prey}\n" +
                $"Obstacles: {stats.obstacles}\n";
        }

        public void DisplayMessage(string message)
        {
            MessageBox.Show(message);
        }

        private void DrawCell(Cell cell, int x, int y)
        {
            ImageBrush img;

            switch (cell.image)
            {
                case GameSettings.ObstacleImage:
                    img = _obstacleImg;
                    break;
                case GameSettings.PredatorImage:
                    img = _predatorImg;
                    break;
                case GameSettings.PreyImage:
                    img = _preyImg;
                    break;
                default:
                    return;
            }

            var sprite = new Rectangle
            {
                Tag = "cell",
                Height = _spriteHeight,
                Width = _spriteWidth,
                Fill = img
            }; 

            Canvas.SetLeft(sprite, x * (_spriteWidth + _padding) + _spriteWidth);
            Canvas.SetTop(sprite, y * (_spriteHeight + _padding));

            canvas.Children.Add(sprite);
        }
    }
}
