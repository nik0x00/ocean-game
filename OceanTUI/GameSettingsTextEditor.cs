using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using OceanGame;

namespace OceanTUI
{
    public static class GameSettingsTextEditor
    {
        public static void Edit(ref GameSettings gameSettings)
        {
            var menu = new NumberEditMenu();
            foreach (var field in typeof(GameSettings).GetFields().Where(x => !x.IsStatic))
            {
                var val = field.GetValue(gameSettings);
                if (val is double)
                {
                    menu.AddEntry(field.Name, (double)val);
                }
                if (val is int)
                {
                    menu.AddEntry(field.Name, (int)val);
                }
            }

            var edited = menu.Make();

            foreach (var field in typeof(GameSettings).GetFields().Where(x => !x.IsStatic))
            {
                double value = edited.Where((x) => x.entry == field.Name).First().val;
                if (field.FieldType == typeof(int))
                {
                    field.SetValue(gameSettings, (int)value);
                }
                else
                {
                    field.SetValue(gameSettings, value);
                }
            }
        }
    }
}
