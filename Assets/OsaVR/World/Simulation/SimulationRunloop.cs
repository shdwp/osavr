using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace OsaVR.World.Simulation
{
    public class SimulationProcess
    {
        public uint Id;
        public IEnumerator Enumerator;

        public SimulationProcess(uint id, IEnumerator enumerator)
        {
            Id = id;
            Enumerator = enumerator;
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
    
    public class SimulationRunloop: MonoBehaviour
    {
        public DateTime CurrentTime;
        private List<SimulationProcess> _processes = new List<SimulationProcess>();
        private Dictionary<uint, SimulationProcess> _processIds = new Dictionary<uint, SimulationProcess>();
        
        public void Add(SimulationProcess e)
        {
            e.Enumerator.MoveNext();
            
            if (!SimulateEnumeratorCompletion(e))
            {
                _processes.Add(e);
                _processIds.Add(e.Id, e);
            }
        }

        public void Remove(uint id)
        {
            _processes.Remove(_processIds[id]);
            _processIds.Remove(id);
        }

        public void Simulate(float secondsAmount)
        {
            CurrentTime = CurrentTime.AddSeconds(secondsAmount);

            SimulationProcess[] array = new SimulationProcess[_processes.Count];
            _processes.CopyTo(array);
            
            foreach (var e in array)
            {
                if (SimulateEnumeratorCompletion(e))
                {
                    _processes.Remove(e);
                    _processIds.Remove(e.Id);
                }
            }
        }

        public bool SimulateEnumeratorCompletion(SimulationProcess e)
        {
            if (e.Enumerator.Current == null)
            {
                return true;
            }
            
            switch (e.Enumerator.Current)
            {
                case SimulationDelay d:
                    if (d.DelayUntil <= CurrentTime)
                    {
                        return !e.Enumerator.MoveNext() || SimulateEnumeratorCompletion(e);
                    }
                    else
                    {
                        return false;
                    }
                default:
                    throw new NotImplementedException();
            }
        }

        public SimulationDelay Delay(float millis)
        {
            return new SimulationDelay(CurrentTime.AddMilliseconds(millis));
        }

        private void FixedUpdate()
        {
            Simulate(Time.fixedDeltaTime);
        }
    }
}