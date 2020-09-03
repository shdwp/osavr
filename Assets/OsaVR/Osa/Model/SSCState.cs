using OsaVR.World.Simulation;

namespace OsaVR.Osa.Model
{
    public class SSCState
    {
        public enum PowertraverseState
        {
            Idle,
            Left,
            Right
        }

        private static int _PowertraverseProcessId = 0;
        private SimulationController _sim;
        
        public float azimuth, distance;

        private PowertraverseState _powertraverseState;

        public PowertraverseState powertraverseState
        {
            get
            {
                return _powertraverseState;
            }

            set
            {
                _powertraverseState = value;
                switch (value)
                {
                    case PowertraverseState.Idle:
                        break;
                }
            }
        }

        public SSCState(SimulationController sim)
        {
            _sim = sim;
        }
    }
}