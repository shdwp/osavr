using System;
using System.Collections;
using OsaVR.Utils;
using OsaVR.World.Simulation;
using UnityEngine;

namespace OsaVR.Osa.Model
{
    public class SSCState
    {
        public enum PowertraverseState
        {
            Idle,
            Left,
            Right
        }

        private static uint _PowertraverseProcessId = OsaState.SSCSimOffset + 0;
        private SimulationController _sim;
        private OsaState _state;

        private float _azimuth;
        public float azimuth
        {
            get => _azimuth;
            set => _azimuth = ClampAzimuth(value);
        }
        
        private float _elevation;
        public float elevation
        {
            get => _elevation;
            set => _elevation = Mathf.Clamp(value, minElevation, maxElevation);
        }

        public float minElevation = 3f, maxElevation = 9f;

        private float _distance;
        public float distance
        {
            get => _distance;
            set => _distance = Mathf.Clamp(value, minDistance, maxDistance);
        }

        public float minDistance = 0f, maxDistance = 28f;

        private PowertraverseState _powertraverseState;

        public PowertraverseState powertraverseState
        {
            get => _powertraverseState;
            set
            {
                _powertraverseState = value;
                switch (value)
                {
                    case PowertraverseState.Idle:
                        _sim.RemoveProcess(_PowertraverseProcessId);
                        break;
                    
                    case PowertraverseState.Right:
                    case PowertraverseState.Left:
                        _sim.AddProcess(_PowertraverseProcessId, PowertraverseProcess());
                        break;
                }
            }
        }

        public SSCState(SimulationController sim, OsaState state)
        {
            _sim = sim;
            _state = state;
            
            _sim.ListenNotification(SOCState.ChangedActiveBeamNotification, (_) =>
            {
                switch (_state.SOC.activeBeam)
                {
                case 1:
                    minElevation = -1f;
                    maxElevation = 5f;
                    break;
                
                case 2:
                    minElevation = 3f;
                    maxElevation = 9f;
                    break;
                
                case 3:
                    minElevation = 6f;
                    maxElevation = 30f;
                    break;
                }
            });
        }

        private IEnumerator PowertraverseProcess()
        {
            while (_powertraverseState != PowertraverseState.Idle)
            {
                var newAzimuth = _azimuth;
                switch (_powertraverseState)
                {
                    case PowertraverseState.Left:
                        newAzimuth -= 1f;
                        break;
                    
                    case PowertraverseState.Right:
                        newAzimuth += 1f;
                        break;
                }

                _azimuth = ClampAzimuth(newAzimuth);
                yield return _sim.Delay(TimeSpan.FromMilliseconds(20));
            }
        }

        private float ClampAzimuth(float value)
        {
            value = MathUtils.NormalizeAzimuth(value);
            if (value > 165f && value < 195f)
            {
                value = value < 180f ? 165f : 195f;
            }

            return value;
        }
    }
}