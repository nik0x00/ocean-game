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
        public OceanController(IOceanView view)
        {
            _view = view;
            _ocean = new Ocean(view.gameSettings);
            _ocean.OceanDataChanged += RefreshView;

            _oceanRunner = new OceanRunner(_ocean, _view, view.gameSettings.FramesPerSecond, view.gameSettings.GameCycles);

            _view.PauseReceived += View_Pause;
            _view.StepReceived += View_Step;
            _view.ForceEndReceived += View_End;
        }

        public bool IsOceanAlive()
        {
            return _oceanRunner.IsAlive();
        }

        private void View_End (object sender, EventArgs e)
        {
            _oceanRunner.ForceEnd();
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
