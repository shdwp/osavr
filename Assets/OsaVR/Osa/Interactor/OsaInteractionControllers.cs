using OsaVR.CockpitFramework.Interactor;
using OsaVR.Osa.Model;
using OsaVR.World.Simulation;

namespace OsaVR.Osa.Interactor
{
    public class OsaButtonController: ButtonController
    {
        protected OsaState _state;
        protected SimulationController _sim;

        protected new void Awake()
        {
            base.Awake();

            _state = FindObjectOfType<OsaState>();
            _sim = FindObjectOfType<SimulationController>();
        }
    }

    public class OsaWheelController : WheelController
    {
        protected OsaState _state;
        protected SimulationController _sim;

        protected new void Awake()
        {
            base.Awake();

            _state = FindObjectOfType<OsaState>();
            _sim = FindObjectOfType<SimulationController>();
        }
    }
    
    public class OsaSwitchController: SwitchController
    {
        protected OsaState _state;
        protected SimulationController _sim;

        protected new void Awake()
        {
            base.Awake();

            _state = FindObjectOfType<OsaState>();
            _sim = FindObjectOfType<SimulationController>();
        }
    }
}