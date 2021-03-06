﻿// #define DEBUG_LOGS

using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using Debug = UnityEngine.Debug;

namespace OsaVR.World.Simulation
{
    
    public class SimulationRunloop
    {
        public bool run = true;

        public double averageLag;
        public double averageSleep;

        private uint _autoPidCounter = 1000;
        private Thread _thread;
        private List<SimulationProcess> _processes = new List<SimulationProcess>();
        private Dictionary<uint, SimulationProcess> _processIds = new Dictionary<uint, SimulationProcess>();
        private Dictionary<uint, List<Action<SimulationEvent>>> _eventListeners = new Dictionary<uint, List<Action<SimulationEvent>>>();
        private DateTime _currentTime = DateTime.Now;
        
        public void AddProcess(SimulationProcess e)
        {
            lock (this)
            {
                if (e.id == 0)
                {
                    _autoPidCounter++;
                    e.id = _autoPidCounter;
                }
                _processes.Add(e);
                _processIds.Add(e.id, e);
                
                // @TODO: figure out whether its viable to interrupt the thread
            }
        }

        public void RemoveProcess(uint id)
        {
            lock (this)
            {
                if (_processIds.ContainsKey(id))
                {
                    _processes.Remove(_processIds[id]);
                    _processIds.Remove(id);
                }
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
            run = true;
            _thread = new Thread(ThreadMain);
            _thread.Start();
        }

        public void Stop()
        {
            run = false;
            _thread.Abort();
        }
        
        public void ThreadMain()
        {
            while (run)
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
                    } 
                    else if (ts < sleepTimeSpan)
                    {
                        sleepTimeSpan = ts;
                    }
                }

                if (sleepTimeSpan == TimeSpan.MaxValue)
                {
#if DEBUG_LOGS
                        Debug.Log($"SimulationRunloop sleep indef");
#endif
                    Thread.Sleep(32);
                }
                else
                {
                    var correlatedTimespan = sleepTimeSpan - frameSw.Elapsed;
#if DEBUG_LOGS
                        Debug.Log($"SimulationRunloop sleep for {correlatedTimespan.TotalMilliseconds}");
#endif

                    if (correlatedTimespan > TimeSpan.Zero)
                    {
                        _currentTime += sleepTimeSpan;
                        averageSleep = (averageSleep + correlatedTimespan.TotalMilliseconds) / 2;
                        
                        try {
                            Thread.Sleep((int) correlatedTimespan.TotalMilliseconds);
                        }
                        catch (ThreadInterruptedException)
                        {
#if DEBUG_LOGS
                            Debug.Log("SimulationRunloop interrupted");
#endif
                        }
                    }
                    else
                    {
                        _currentTime += frameSw.Elapsed;
                        averageSleep /= 2;
                    }
                }
            }
        }

        private TimeSpan HandleProcess(SimulationProcess p)
        {
            if (p.running == false)
            {
                p.running = true;
                p.enumerator.MoveNext();
            }
                    
            switch (p.enumerator.Current)
            {
                case SimulationDelay d:
                    var delta = d.delayUntil - _currentTime;
                    if (delta <= TimeSpan.Zero)
                    {
#if DEBUG_LOGS
                        Debug.Log($"Simulation lag {delta.TotalMilliseconds}");
#endif
                        // Debug.Log(delta.TotalMilliseconds);
                        averageLag = (averageLag + -delta.TotalMilliseconds) / 2;
                        p.enumerator.MoveNext();
                        return HandleProcess(p);
                    }
                    else
                    {
                        return delta;
                    }
                
                case SimulationEvent e:
                    NotifyListenersAbout(e);
                    p.enumerator.MoveNext();
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
                contains = _eventListeners.ContainsKey(e.id);
            }

            if (contains)
            {
                Action<SimulationEvent>[] array;
                lock (this)
                {
                    var list = _eventListeners[e.id];
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