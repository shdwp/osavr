// #define DEBUG_LOGS

using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using Debug = UnityEngine.Debug;

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
    
    public class SimulationRunloop
    {
        public bool Run = true;
        public TimeSpan LastLagTimespan;
        public TimeSpan LastSleepTimespan;
        
        private Thread _thread;
        private List<SimulationProcess> _processes = new List<SimulationProcess>();
        private Dictionary<uint, SimulationProcess> _processIds = new Dictionary<uint, SimulationProcess>();
        private Dictionary<uint, List<Action<SimulationEvent>>> _eventListeners = new Dictionary<uint, List<Action<SimulationEvent>>>();
        private DateTime _currentTime;
        
        public void AddProcess(SimulationProcess e)
        {
            lock (this)
            {
                _processes.Add(e);
                _processIds.Add(e.Id, e);

                if (_thread != null)
                {
                    _thread.Interrupt();
                }
            }
        }

        public void RemoveProcess(uint id)
        {
            lock (this)
            {
                _processes.Remove(_processIds[id]);
                _processIds.Remove(id);
            }
        }

        public void ListenEvent(uint id, Action<SimulationEvent> callback)
        {
            lock (this)
            {
                List<Action<SimulationEvent>> list;
                if (!_eventListeners.TryGetValue(id, out list))
                {
                    list = new List<Action<SimulationEvent>>();
                    _eventListeners[id] = list;
                }

                list.Add(callback);
            }
        }

        public SimulationDelay Delay(TimeSpan span)
        {
            return new SimulationDelay(_currentTime + span);
        }

        public void Start()
        {
            Run = true;
            _thread = new Thread(ThreadMain);
            _thread.Start();
        }

        public void Stop()
        {
            Run = false;
            _thread.Abort();
        }
        
        public void ThreadMain()
        {
            while (Run)
            {
                var frameSw = Stopwatch.StartNew();
                var sleepTimeSpan = TimeSpan.MaxValue;

                SimulationProcess[] array;
                lock (this)
                {
                    array = new SimulationProcess[_processes.Count];
                    _processes.CopyTo(array);
                }

                foreach (var p in array)
                {
                    var ts = HandleProcess(p);
                    if (ts == TimeSpan.MaxValue)
                    {
#if DEBUG_LOGS
                        Debug.Log($"SimulationRunloop removed process {p.Id}");
#endif
                        lock (this)
                        {
                            _processes.Remove(p);
                        }
                    } else if (ts < sleepTimeSpan)
                    {
                        sleepTimeSpan = ts;
                    }
                }

                try
                {
                    if (sleepTimeSpan == TimeSpan.MaxValue)
                    {
#if DEBUG_LOGS
                        Debug.Log($"SimulationRunloop sleep indef");
#endif
                        Thread.Sleep(-1);
                    }
                    else if (sleepTimeSpan < frameSw.Elapsed)
                    {
                        LastSleepTimespan = TimeSpan.Zero;
                    } 
                    else 
                    {
                        var correlatedTimespan = sleepTimeSpan - frameSw.Elapsed;
#if DEBUG_LOGS
                        Debug.Log($"SimulationRunloop sleep for {correlatedTimespan.TotalMilliseconds}");
#endif
                        LastSleepTimespan = correlatedTimespan;
                        Thread.Sleep((int) correlatedTimespan.TotalMilliseconds);
                    }
                }
                catch (ThreadInterruptedException)
                {
#if DEBUG_LOGS
                    Debug.Log("SimulationRunloop interrupted");
#endif
                }

                _currentTime += frameSw.Elapsed;
            }
        }

        private TimeSpan HandleProcess(SimulationProcess p)
        {
            if (p.Running == false)
            {
                p.Running = true;
                p.Enumerator.MoveNext();
            }
                    
            switch (p.Enumerator.Current)
            {
                case SimulationDelay d:
                    var delta = d.DelayUntil - _currentTime;
                    if (delta <= TimeSpan.Zero)
                    {
#if DEBUG_LOGS
                        Debug.Log($"Simulation lag {delta.TotalMilliseconds}");
#endif
                        LastLagTimespan = delta;
                        p.Enumerator.MoveNext();
                        return HandleProcess(p);
                    }
                    else
                    {
                        return delta;
                    }
                
                case SimulationEvent e:
                    NotifyListenersAbout(e);
                    p.Enumerator.MoveNext();
                    return HandleProcess(p);
                
                case null:
                default:
                    return TimeSpan.MaxValue;
            }
        }

        private void NotifyListenersAbout(SimulationEvent e)
        {
            bool contains;
            lock (this)
            {
                contains = _eventListeners.ContainsKey(e.Id);
            }

            if (contains)
            {
                Action<SimulationEvent>[] array;
                lock (this)
                {
                    var list = _eventListeners[e.Id];
                    array = new Action<SimulationEvent>[list.Count];
                    list.CopyTo(array);
                }

                foreach (var listener in array)
                {
                    listener(e);
                }
            }
        }
    }
}