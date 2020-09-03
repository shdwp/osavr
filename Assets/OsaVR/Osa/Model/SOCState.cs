using System;
using System.Collections;
using OsaVR.Utils;
using OsaVR.World.Simulation;

namespace OsaVR.Osa.Model
{
    public class SOCState
    {
        private static readonly uint _SOCProcess = OsaState.SOCProcOffset + 0;
        
        public static readonly uint TurnEvent = OsaState.SOCProcOffset + 1;
        public static readonly uint ReceiveEvent = OsaState.SOCProcOffset + 2;
        
        public float azimuth;
        public bool turning = true;
        public bool emitting = true;
        public uint activeBeam = 2;

        private SimulationController _sim;

        public SOCState(SimulationController sim)
        {
            _sim = sim;
            
            sim.Add(new SimulationProcess(_SOCProcess, SimulationProcess()));
        }
        
        public IEnumerator SimulationProcess()
        {
            while (true)
            {
                // 1.818s per revolution
                // 90 adjustments per rotation
                // 20.13ms per adjustment
                
                if (turning)
                {
                    azimuth = MathUtils.NormalizeAzimuth(azimuth + 4f);
                    yield return new SimulationEvent(TurnEvent);
                }
                
                if (emitting)
                {
                    yield return new SimulationEvent(ReceiveEvent);
                }
                
                yield return _sim.Delay(TimeSpan.FromMilliseconds(20.13));
            }
        }
    }
}