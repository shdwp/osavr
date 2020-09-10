using System;
using System.Collections;
using OsaVR.Utils;
using OsaVR.World.Simulation;

namespace OsaVR.Osa.Model
{
    public class SOCState
    {
        private static readonly uint _SOCProcess = OsaState.SOCSimOffset + 0;
        
        public static readonly uint TurnEvent = OsaState.SOCSimOffset + 1;
        public static readonly uint ReceiveEvent = OsaState.SOCSimOffset + 2;
        
        public static readonly uint ChangedActiveBeamNotification = OsaState.SOCSimOffset + 3;
        public static readonly uint ChangedEnergyStateNotification = OsaState.SOCSimOffset + 4;
        public static readonly uint ChangedDisplayRange = OsaState.SOCSimOffset + 5;

        public enum ScopeDisplayRange
        {
            Zero_Fifteen,
            Zero_ThirtyFive,
            Ten_FortyFive
        }

        private ScopeDisplayRange _scopeScopeDisplayRange = ScopeDisplayRange.Zero_ThirtyFive;

        public ScopeDisplayRange ScopeScopeDisplayRange
        {
            get => _scopeScopeDisplayRange;
            set
            {
                _scopeScopeDisplayRange = value;
                _sim.PostNotification(ChangedDisplayRange);
            }
        }
        
        public float azimuth;
        public bool turning = true;

        private bool _emitting = true;

        public bool emitting
        {
            get => _emitting;
            set
            {
                _sim.PostNotification(ChangedEnergyStateNotification);
                _emitting = value;
            }
        }

        private uint _activeBeam = 2;
        public uint activeBeam
        {
            get => _activeBeam;
            set
            {
                _activeBeam = value;
                switch (_activeBeam)
                {
                    case 1:
                        _elevation = 3f;
                        break;
                    case 2:
                        _elevation = 6f;
                        break;
                    case 3:
                        _elevation = 12f;
                        break;
                    default:
                        throw new NotImplementedException();
                }
                
                _sim.PostNotification(ChangedActiveBeamNotification);
            }
        }

        private float _elevation;
        public float elevation => _elevation;

        private SimulationController _sim;

        public SOCState(SimulationController sim)
        {
            _sim = sim;
            
            sim.AddProcess(new SimulationProcess(_SOCProcess, SimulationProcess()));
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
                // yield return _sim.Delay(TimeSpan.FromMilliseconds(40.26));
            }
        }
    }
}