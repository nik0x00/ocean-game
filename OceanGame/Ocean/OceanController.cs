using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OceanGame
{
    public class OceanController
    {
        private Ocean _ocean;
        private readonly IOceanView _view;
        private OceanRunner _oceanRunner;
        public OceanController(IOceanView view, int fps, int cycles)
        {
            _view = view;
            _ocean = new Ocean(_view.oceanWidth, _view.oceanHeight);
            _ocean.OceanDataChanged += RefreshView;

            _oceanRunner = new OceanRunner(_ocean, _view, fps, cycles);

            _view.PauseReceived += View_Pause;
            _view.StepReceived += View_Step;
        }

        public bool IsOceanAlive()
        {
            return _oceanRunner.IsAlive();
        }

        private void View_Pause(object sender, EventArgs e)
        {
            _oceanRunner.Switch();
        }

        private void View_Step(object sender, EventArgs e)
        {
            _oceanRunner.Step();
        }

        private void RefreshView(object sender, OceanDataChangedEventArgs e)
        {
            _view.Display(e.Field, e.Stats);
        }
    }
}
