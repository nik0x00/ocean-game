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
using System.Windows.Shapes;
using System.Text.RegularExpressions;

using OceanGame;

namespace OceanGUI
{
    /// <summary>
    /// Interaction logic for Window1.xaml
    /// </summary>
    public partial class SettingsEditWindow : Window, ISettingsMenu
    {
        private GameSettings _settings;

        private int _currCol = 0;
        private int _currRow = 0;

        public event MenuEndEventHandler OnMenuEnd;

        public SettingsEditWindow(GameSettings settings)
        {
            InitializeComponent();
            ResizeMode = ResizeMode.NoResize;
            _settings = settings;
        }

        private void OnSave(object sender, RoutedEventArgs e)
        {
            var entries = new List<(string entry, double val)>();
            foreach (var c in mainGrid.Children)
            {
                if (c is TextBox)
                {
                    var box = c as TextBox;
                    Double.TryParse(box.Text, out var val); //TODO: Add validation
                    entries.Add((box.Name, val));
                }
            }

            if (OnMenuEnd != null)
            {
                OnMenuEnd(this, new MenuEndEventArgs(entries));
            }

            Close();
        }

        public void AddEntry(string name, double defaultValue, double min, double max)
        {
            var label = new Label();

            label.ToolTip = $"{min} - {max}";
            label.Content = name;

            Grid.SetColumn(label, _currCol);
            Grid.SetRow(label, _currRow);
            mainGrid.Children.Add(label);

            var box = new NumericBox();

            box.Text = defaultValue.ToString();
            box.MinValue = min;
            box.MaxValue = max;
            box.Width = 100;
            box.Height = 25;
            box.HorizontalAlignment = HorizontalAlignment.Left;
            box.Margin = new Thickness(5, 0, 0, 0);
            box.Name = name;
            Grid.SetColumn(box, _currCol);
            Grid.SetRow(box, _currRow);
            mainGrid.Children.Add(box);

            if (++_currRow >= mainGrid.RowDefinitions.Count)
            {
                _currCol++;
                _currRow = 0;
            }

            if (_currCol >= mainGrid.ColumnDefinitions.Count)
            {
                throw new IndexOutOfRangeException("too many fields");
            }
        }

        public void Make()
        {
            Show();
        }
    }

}
