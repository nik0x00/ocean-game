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

        private OceanController _controller;

        private readonly ImageBrush _preyImg = ImageLoader.Load(@"pack://application:,,,/res/prey.png");
        private readonly ImageBrush _predatorImg = ImageLoader.Load(@"pack://application:,,,/res/predator.png");
        private readonly ImageBrush _obstacleImg = ImageLoader.Load(@"pack://application:,,,/res/obstacle.png");

        private readonly DispatcherTimer _oceanTimer = new DispatcherTimer();

        private bool _gameStarted = false;
        public GameSettings ViewGameSettings { get; private set; }

        public MainWindow()
        {
            InitializeComponent();
            ViewGameSettings = new GameSettings();

            helpText.TextWrapping = TextWrapping.Wrap;
            helpText.Text = 
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
        
        private void StartSettings(object sender, RoutedEventArgs e)
        {
            IsEnabled = false;

            var settingsWindow = new SettingsEditWindow(ViewGameSettings);
            var editor = new GameSettingsEditor(settingsWindow);

            editor.OnSettingsEdited +=
                (object sender, SettingsSaveEventArgs e) =>
                {
                    ViewGameSettings = e.settings;
                    IsEnabled = true;
                    ScaleCanvas();
                };

            editor.Edit(ViewGameSettings);
        }

        private void StartGame(object sender, RoutedEventArgs e)
        {
            ScaleCanvas();

            _controller = new OceanController(this);

            // We don`t use ocean runner thread here because Display() needs to be called from main thread
            // We are manually making steps with timer instead
            _oceanTimer.Tick += Step;
            _oceanTimer.Tick += WatchAlive;
            _oceanTimer.Interval = TimeSpan.FromMilliseconds(1000 / ViewGameSettings.MaxFramesPerSecond);

            _gameStarted = true;

            Pause(); // Actually start game

            startButton.IsEnabled = false;
            settingsButton.IsEnabled = false;
        }

        private void ScaleCanvas()
        {
            canvas.Height = (_spriteHeight + _padding) * ViewGameSettings.OceanHeight;
            canvas.Width = (_spriteWidth + _padding) * ViewGameSettings.OceanWidth;
        }

        private void OnKeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.P && _gameStarted)
            {
                Pause();
            }
            if (e.Key == Key.S && !_oceanTimer.IsEnabled && _gameStarted)
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

        public event EventHandler PauseReceived;
        public event EventHandler StepReceived;
        public event EventHandler ForceEndReceived;

        public void Display(in Cell[,] field, in GameStats stats)
        {
            canvas.Children.Clear();
            DrawingVisual drawingVisual = new DrawingVisual();
            DrawingContext drawingContext = drawingVisual.RenderOpen();

            var group = new DrawingGroup();

            for (int i = 0; i < ViewGameSettings.OceanHeight; i++)
            {
                for (int j = 0; j < ViewGameSettings.OceanWidth; j++)
                {
                    DrawCell(field[i, j], j, i, group);
                }
            }

            drawingContext.DrawDrawing(group);
            drawingContext.Close();

            var bmp = new RenderTargetBitmap((int)canvas.Width, (int)canvas.Height, 96, 96, PixelFormats.Pbgra32);
            bmp.Render(drawingVisual);

            var img = new Image();
            img.Source = bmp;

            canvas.Children.Add(img);

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

        private void DrawCell(Cell cell, int x, int y, DrawingGroup group)
        {
            ImageBrush img;

            switch (cell.Image)
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

            var rect = new Rect
            (
                x * (_spriteWidth + _padding),
                y * (_spriteHeight + _padding),
                _spriteWidth,
                _spriteHeight
            );

            var g = new RectangleGeometry(rect);

            var drawing = new GeometryDrawing(img, null, g);

            group.Children.Add(drawing);
        }
    }
}
