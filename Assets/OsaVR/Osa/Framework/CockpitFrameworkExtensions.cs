using OsaVR.CockpitFramework.Interactor;
using OsaVR.CockpitFramework.ViewControllers;
using OsaVR.CockpitFramework.ViewControllers.Indicator;
using OsaVR.CockpitFramework.ViewControllers.RotatingDial;
using OsaVR.Osa.Model;
using OsaVR.World.Simulation;

namespace OsaVR.Osa
{
    public class OsaIndicatorController: IndicatorController
    {
        protected SimulationController _sim;
        protected OsaState _state;

        protected void Awake()
        {
            base.Awake();
            
            _sim = FindObjectOfType<SimulationController>();
            _state = FindObjectOfType<OsaState>();
        }
    }

    public class OsaDisplayController : ADisplayController
    {
        protected SimulationController _sim;
        protected OsaState _state;

        protected void Awake()
        {
            base.Awake();
            
            _sim = FindObjectOfType<SimulationController>();
            _state = FindObjectOfType<OsaState>();
        }
    }
    
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

    public class OsaCircularDialController : GenericCircularDialController
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