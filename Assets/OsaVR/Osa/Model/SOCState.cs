using System;
using System.Collections;
using OsaVR.Utils;
using OsaVR.World.Simulation;
using UnityEngine;

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
        
        private static readonly uint _IFFProcess = OsaState.SOCSimOffset + 6;
        public static readonly uint ChangedIFFState = OsaState.SOCSimOffset + 7;

        public enum ScopeDisplayRange
        {
            Zero_Fifteen,
            Zero_ThirtyFive,
            Ten_FortyFive
        }

        public enum ActiveBeamAutoMode
        {
            Disabled,
            Beam1_3,
            Beam1_2,
        }

        public enum IFFMode
        {
            Off,
            Test,
            Request,
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

        private ActiveBeamAutoMode _activeBeamAutoMode = ActiveBeamAutoMode.Disabled;
        public ActiveBeamAutoMode activeBeamAutoMode
        {
            get => _activeBeamAutoMode;
            set => _activeBeamAutoMode = value;
        }

        private float _elevation;
        public float elevation => _elevation;

        private IFFMode _iffMode = IFFMode.Off;
        public IFFMode iffMode
        {
            get => _iffMode;
            set
            {
                if (_iffMode != value)
                {
                    switch (value)
                    {
                        case IFFMode.Test:
                        case IFFMode.Request:
                            _sim.RemoveProcess(_IFFProcess);
                            _sim.AddProcess(_IFFProcess, IFFProcess());
                            break;
                    }
                    
                    _iffMode = value;
                    _sim.PostNotification(ChangedIFFState);
                }
            }
        }

        private SimulationController _sim;

        public SOCState(SimulationController sim)
        {
            _sim = sim;
            
            sim.AddProcess(new SimulationProcess(_SOCProcess, SimulationProcess()));
        }
        
        private IEnumerator SimulationProcess()
        {
            while (true)
            {
                var oldAzimuth = azimuth;
                var adjustment = 4f;
                if (turning)
                {
                    azimuth = MathUtils.NormalizeAzimuth(azimuth + adjustment);
                    yield return new SimulationEvent(TurnEvent);
                }

                if (emitting)
                {
                    yield return new SimulationEvent(ReceiveEvent);
                }
                
                if (activeBeamAutoMode != ActiveBeamAutoMode.Disabled && Mathf.Abs(oldAzimuth - azimuth) > (360f - adjustment - 1f))
                {
                    var newActiveBeam = activeBeam + 1;
                    switch (activeBeamAutoMode)
                    {
                        case ActiveBeamAutoMode.Beam1_2:
                            activeBeam = newActiveBeam > 2 ? 1 : newActiveBeam;
                            break;
                        
                        case ActiveBeamAutoMode.Beam1_3:
                            activeBeam = newActiveBeam > 3 ? 1 : newActiveBeam;
                            break;
                    }
                }
                
                yield return _sim.Delay(TimeSpan.FromMilliseconds(20.13));
            }
        }

        private IEnumerator IFFProcess()
        {
            yield return _sim.Delay(TimeSpan.FromSeconds(3));
            iffMode = IFFMode.Off;
            yield return null;
        }
    }
}