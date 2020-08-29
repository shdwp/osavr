using System;
using System.Collections;

namespace OsaVR.World.Simulation
{
    public class SimulationProcess
    {
        public bool running = false;
        public IEnumerator enumerator;
        public uint id;

        public SimulationProcess(uint id, IEnumerator enumerator)
        {
            this.enumerator = enumerator;
            this.id = id;
        }
    }

    public class SimulationDelay
    {
        public DateTime delayUntil;
        
        public SimulationDelay(DateTime delayUntil)
        {
            this.delayUntil = delayUntil;
        }
    }

    public class SimulationEvent
    {
        public uint id;

        public SimulationEvent(uint id)
        {
            this.id = id;
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