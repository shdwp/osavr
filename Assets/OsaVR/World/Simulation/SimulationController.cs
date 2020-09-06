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
        private Dictionary<uint, List<Action<uint>>> _notificationListeners = new Dictionary<uint, List<Action<uint>>>();

        private void Start()
        {
            _simRunloop.Start();
        }

        public void AddProcess(SimulationProcess p)
        {
            _simRunloop.AddProcess(p);
        }

        public void AddProcess(IEnumerator procEnumerator)
        {
            _simRunloop.AddProcess(new SimulationProcess(0, procEnumerator));
        }

        public void AddProcess(uint procId, IEnumerator procEnumerator)
        {
            _simRunloop.AddProcess(new SimulationProcess(procId, procEnumerator)); 
        }

        public void RemoveProcess(uint id)
        {
            _simRunloop.RemoveProcess(id);
        }

        public void ListenProcessEvent(uint id, Action<SimulationEvent> listener)
        {
            _simRunloop.ListenEvent(id, listener);
        }
        
        public void ListenNotification(uint id, Action<uint> listener)
        {
            List<Action<uint>> listeners;
            if (!_notificationListeners.TryGetValue(id, out listeners))
            {
                listeners = new List<Action<uint>>();
                _notificationListeners.Add(id, listeners);
            }
            
            listeners.Add(listener);
        }

        public void PostNotification(uint id)
        {
            List<Action<uint>> listeners;
            if (_notificationListeners.TryGetValue(id, out listeners))
            {
                foreach (var listener in listeners)
                {
                    listener(id);
                }
            }
        }

        public SimulationDelay Delay(TimeSpan ts)
        {
            return _simRunloop.Delay(ts);
        }

        private void OnApplicationQuit()
        {
            _simRunloop.Stop();
        }
    }
}