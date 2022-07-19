using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;

namespace OceanGame
{
    public class GameSettingsEditor
    {
        ISettingsMenu _menu;

        public GameSettingsEditor(ISettingsMenu menu)
        {
            _menu = menu;
        }

        public void Edit(GameSettings gameSettings)
        {
            foreach (var field in typeof(GameSettings).GetFields().Where(x => !x.IsStatic))
            {
                var val = field.GetValue(gameSettings);
                if (val is double)
                {
                    _menu.AddEntry(field.Name, (double)val, 0, 1);
                }
                if (val is int)
                {
                    _menu.AddEntry(field.Name, (int)val, 1, 10000);
                }
            }

            _menu.OnMenuEnd += (s, e) =>
            {
                foreach (var field in typeof(GameSettings).GetFields().Where(x => !x.IsStatic))
                {
                    double value = e.entries.Where((x) => x.entry == field.Name).First().val;
                    if (field.FieldType == typeof(int))
                    {
                        field.SetValue(gameSettings, (int)value);
                    }
                    else
                    {
                        field.SetValue(gameSettings, value);
                    }
                }
                if (OnSettingsEdited != null)
                {
                    OnSettingsEdited(this, new SettingsSaveEventArgs(gameSettings));
                }
            };

            _menu.Make();
        }

        public event SettingsSaveEventHandler OnSettingsEdited;
    }


    public class SettingsSaveEventArgs : EventArgs
    {
        public GameSettings settings;

        public SettingsSaveEventArgs(GameSettings mSettings)
        {
            settings = mSettings;
        }
    }

    public delegate void SettingsSaveEventHandler(object sender, SettingsSaveEventArgs e);
}
