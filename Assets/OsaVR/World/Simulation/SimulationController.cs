using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using UnityEngine;

namespace OsaVR.World.Simulation
{
    public class SimulationController : MonoBehaviour
    {
        public double averageLag => _simRunloop.averageLag;
        public double averageSleep => _simRunloop.averageSleep;
        
        private SimulationRunloop _simRunloop = new SimulationRunloop();

        private void Start()
        {
            _simRunloop.Start();
        }

        public void Add(SimulationProcess p)
        {
            _simRunloop.AddProcess(p);
        }

        public void Add(IEnumerator procEnumerator)
        {
            _simRunloop.AddProcess(new SimulationProcess(0, procEnumerator));
        }

        public void Add(uint procId, IEnumerator procEnumerator)
        {
            _simRunloop.AddProcess(new SimulationProcess(procId, procEnumerator)); 
        }

        public void Remove(uint id)
        {
            _simRunloop.RemoveProcess(id);
        }

        public void Listen(uint id, Action<SimulationEvent> listener)
        {
            _simRunloop.ListenEvent(id, listener);
        }

        public SimulationDelay Delay(TimeSpan ts)
        {
            return _simRunloop.Delay(ts);
        }

        private void LateUpdate()
        {
            
        }

        private void OnApplicationQuit()
        {
            _simRunloop.Stop();
        }
    }
}