using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OceanTUI
{ 
    public class NumberEditMenu
    {
        private int _offsetY = 0;
        private int _currHighlight = 0;
        private List<(string entry, double val)> _entries = new List<(string, double)>();

        public void AddEntry(string name, double defaultValue)
        {
            _entries.Add((name, defaultValue));
        }

        public List<(string entry, double val)> Make()
        {
            Console.Clear();
            Console.WriteLine("Press Q to exit menu");
            Console.WriteLine("Use arrows to navigate, Enter to edit or confirm\n\n");
            Console.CursorVisible = false;
            _offsetY = Console.GetCursorPosition().Top;
            for (int i = 0; i < _entries.Count; i++)
            {
                DisplayEntry(i, false);
            }
            while (true)
            {
                if (Console.KeyAvailable)
                { 
                    var key = Console.ReadKey().Key;

                    if (key == ConsoleKey.Q)
                    {
                        InvertColor(false);
                        Console.CursorVisible = false;
                        return _entries;
                    }

                    else if (key == ConsoleKey.UpArrow)
                    {
                        PrevItem();
                    }

                    else if (key == ConsoleKey.DownArrow)
                    {
                        NextItem();
                    }

                    else if (key == ConsoleKey.Enter)
                    {
                        UpdateEntry(_currHighlight);
                    }
                    
                    else
                    {
                        // Clear symbol
                        var pos = Console.GetCursorPosition();
                        Console.SetCursorPosition(pos.Left - 1, pos.Top);
                        InvertColor(false);
                        Console.Write(" ");
                    }
                }
            }
        }

        private void NextItem()
        {
            DisplayEntry(_currHighlight, false);
            if (++_currHighlight >= _entries.Count)
            {
                _currHighlight = 0;
            }
            DisplayEntry(_currHighlight, true);
        }

        private void PrevItem()
        {
            DisplayEntry(_currHighlight, false);
            if (--_currHighlight < 0)
            {
                _currHighlight = _entries.Count - 1;
            }
            DisplayEntry(_currHighlight, true);
        }

        private void InvertColor(bool inv)
        {
            if (inv)
            {
                Console.ForegroundColor = ConsoleColor.Black;
                Console.BackgroundColor = ConsoleColor.White;
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.White;
                Console.BackgroundColor = ConsoleColor.Black;
            }
        }

        private void DisplayEntry(int index, bool inv)
        {
            InvertColor(false);
            Console.SetCursorPosition(0, index + _offsetY);
            Console.Write($"{_entries[index].entry}: ");
            InvertColor(inv);
            Console.Write($"{ _entries[index].val}");
        }

        private void ClearEntry(int index)
        {
            InvertColor(false);
            var posX = _entries[_currHighlight].entry.Length + 2;
            Console.SetCursorPosition(posX, index + _offsetY);
            
            Console.Write("                               ");

            Console.SetCursorPosition(posX, index + _offsetY);
        }

        private void UpdateEntry(int index)
        {
            ClearEntry(index);
            Console.CursorVisible = true;
            while (true)
            {
                bool success = Double.TryParse(Console.ReadLine(), out var val);
                if (success)
                {
                    _entries[_currHighlight] = (_entries[_currHighlight].entry, val);
                    DisplayEntry(index, true);
                    Console.CursorVisible = false;
                    return;
                }
                ClearEntry(index);
            }
        }
    }
}
