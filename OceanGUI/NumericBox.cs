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

namespace OceanGUI
{
    public class NumericBox : TextBox
    {
        private bool _maxBound = false;
        private bool _minBound = false;
        private double _minValue = -1;
        private double _maxValue = -1;

        public double MinValue { 
            get { return _minValue; }
            set {
                _minBound = true;
                _minValue = value;
            } 
        }
        public double MaxValue
        {
            get { return _maxValue; }
            set
            {
                _maxBound = true;
                _maxValue = value;
            }
        }

        public NumericBox()
        {
            PreviewTextInput += NumericValidate;
        }

        private void NumericValidate(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex(@"[^0-9,]+");
            if (regex.IsMatch(e.Text))
            {
                e.Handled = true;
            }

            var text = Text + e.Text;

            bool success = Double.TryParse(text, out var val);
            if (!success 
                || (_minBound && val < _minValue)
                || (_maxBound && val > _maxValue))
            {
                e.Handled = true;
            }
        }
    }


}
