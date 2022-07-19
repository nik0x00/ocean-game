using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OceanGame
{ 
    public interface ISettingsMenu
    {
        public void AddEntry(string name, double defaultValue, double min, double max);

        public void Make();

        public event MenuEndEventHandler OnMenuEnd;
    }

    public class MenuEndEventArgs : EventArgs
    {
        public List<(string entry, double val)> entries;

        public MenuEndEventArgs(List<(string entry, double val)> mEntries)
        {
            entries = mEntries;
        }
    }

    public delegate void MenuEndEventHandler(object sender, MenuEndEventArgs e);
}
