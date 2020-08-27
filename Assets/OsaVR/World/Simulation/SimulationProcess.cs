using System;
using System.Collections;

namespace OsaVR.World.Simulation
{
    public class SimulationProcess
    {
        public bool Running = false;
        public IEnumerator Enumerator;
        public uint Id;

        public SimulationProcess(uint id, IEnumerator enumerator)
        {
            Enumerator = enumerator;
            Id = id;
        }
    }

    public class SimulationDelay
    {
        public DateTime DelayUntil;
        
        public SimulationDelay(DateTime delayUntil)
        {
            DelayUntil = delayUntil;
        }
    }

    public class SimulationEvent
    {
        public uint Id;

        public SimulationEvent(uint id)
        {
            Id = id;
        }
    }
    
    public class LambdaEnumerator : IEnumerator
    {
        private Action _lambda;

        public LambdaEnumerator(Action lambda)
        {
            _lambda = lambda;
        }

        public bool MoveNext()
        {
            _lambda();
            return true;
        }

        public void Reset() { 
        }

        public object Current => null;
    }
}