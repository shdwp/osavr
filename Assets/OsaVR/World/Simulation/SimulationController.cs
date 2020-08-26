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
        public double AverageLag => _lagTimespanQueue.Sum(t => t.TotalMilliseconds) / _lagTimespanQueue.Count;
        public double AverageSleep => _sleepTimespanQueue.Sum(t => t.TotalMilliseconds) / _sleepTimespanQueue.Count;
        
        private Queue<TimeSpan> _lagTimespanQueue = new Queue<TimeSpan>();
        private Queue<TimeSpan> _sleepTimespanQueue = new Queue<TimeSpan>();
        private SimulationRunloop _simRunloop = new SimulationRunloop();

        private void Start()
        {
            _simRunloop.Start();
        }

        public void Add(SimulationProcess p)
        {
            _simRunloop.AddProcess(p);
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

        private void Update()
        {
            if (_lagTimespanQueue.Count > 20)
            {
                _lagTimespanQueue.Dequeue();
            }
            
            _lagTimespanQueue.Enqueue(_simRunloop.LastLagTimespan);

            if (_sleepTimespanQueue.Count > 20)
            {
                _sleepTimespanQueue.Dequeue();
            }
            
            _sleepTimespanQueue.Enqueue(_simRunloop.LastSleepTimespan);
        }

        private void OnApplicationQuit()
        {
            _simRunloop.Stop();
        }
    }
}