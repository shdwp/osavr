using System;
using System.Collections;
using OsaVR.RadarSimulation;
using OsaVR.World.Simulation;
using UnityEngine;

namespace OsaVR.Osa.Model
{
    public class SUAState
    {
        private static uint _AutoacquisitionProcessId = OsaState.SAUSimOffset + 1;
        private static uint _TrackingProcessId = OsaState.SAUSimOffset + 2;

        public static uint ChangedTrackingState = OsaState.SAUSimOffset + 50 + 0;
        
        [Flags]
        public enum TrackingFlags
        {
            Disabled = 0,
            Azimuth = 1 << 0,
            Distance = 1 << 1,
            Elevation = 1 << 2,
            SemiAutomatic = 1 << 3,
        }

        public enum AutoacquisitionDirection
        {
            Up,
            Down,
        }

        private bool _autoAcquisition = false;

        public bool autoAcquisition
        {
            get => _autoAcquisition;
            set
            {
                if (_autoAcquisition != value)
                {
                    if (_autoAcquisition && !value)
                    {
                        _sim.RemoveProcess(_AutoacquisitionProcessId);
                    } else if (!_autoAcquisition && value)
                    {
                        _sim.RemoveProcess(_TrackingProcessId);
                        _sim.AddProcess(_AutoacquisitionProcessId, AutoacquisitionProcess());
                    }
                    
                    _sim.PostNotification(ChangedTrackingState);
                    _autoAcquisition = value;
                }
            }
        }

        private TrackingFlags _trackingFlags;
        public TrackingFlags trackingFlags
        {
            get => _trackingFlags;
            set
            {
                if (value != _trackingFlags)
                {
                    _sim.PostNotification(ChangedTrackingState);
                    _trackingFlags = value;
                }
            }
        }
        
        private NativeSSCDeviationInfoStruct _lastDeviationInfo;
        private AutoacquisitionDirection _autoacquisitionDirection = AutoacquisitionDirection.Up;

        private SimulationController _sim;
        private OsaState _state;

        public SUAState(SimulationController sim, OsaState state)
        {
            _sim = sim;
            _state = state;
        }
        
        public void InputDeviationInfo(NativeSSCDeviationInfoStruct info)
        {
            _lastDeviationInfo = info;

            if (info.defined && autoAcquisition)
            {
                autoAcquisition = false;
                trackingFlags = TrackingFlags.Elevation | TrackingFlags.Azimuth | TrackingFlags.Distance;
                _sim.AddProcess(_TrackingProcessId, TrackingProcess());
            }
        }

        private IEnumerator AutoacquisitionProcess()
        {
            var ssc = _state.SSC;
            while (_autoAcquisition)
            {
                switch (_autoacquisitionDirection)
                {
                    case AutoacquisitionDirection.Up:
                        ssc.elevation += 0.3f;
                        if (Mathf.Abs(ssc.elevation - ssc.maxElevation) < 1f)
                        {
                            _autoacquisitionDirection = AutoacquisitionDirection.Down;
                        }
                        break;
                    
                    case AutoacquisitionDirection.Down:
                        ssc.elevation -= 0.3f;
                        if (Mathf.Abs(ssc.elevation - ssc.minElevation) < 1f)
                        {
                            _autoacquisitionDirection = AutoacquisitionDirection.Up;
                        }
                        break;
                }
                
                // @TODO: synchronize with radar image capturing
                yield return _sim.Delay(TimeSpan.FromMilliseconds(20));
            }

            yield return null;
        }

        private IEnumerator TrackingProcess()
        {
            var ssc = _state.SSC;
            while (trackingFlags != 0)
            {
                var deviation = _lastDeviationInfo;
                if (!deviation.defined)
                {
                    trackingFlags = TrackingFlags.Disabled;
                    yield return null;
                }

                if (trackingFlags.HasFlag(TrackingFlags.Azimuth))
                {
                    var azimuthFix = deviation.x * ssc.fov;
                    if (Mathf.Abs(azimuthFix) > 0.05f)
                    {
                        ssc.azimuth += Mathf.Clamp(azimuthFix, -1.2f, 1.2f);
                    }
                }

                if (trackingFlags.HasFlag(TrackingFlags.Elevation))
                {
                    var elevFix = deviation.y * ssc.fov;
                    if (Mathf.Abs(elevFix) > 0.01f)
                    {
                        ssc.elevation += Mathf.Clamp(elevFix, -0.6f, 0.6f);
                    }
                }

                if (trackingFlags.HasFlag(TrackingFlags.Distance))
                {
                    var distFix = deviation.z * ssc.targetGateWidth;
                    if (Mathf.Abs(distFix) > 0.01f)
                    {
                        ssc.distance += distFix;
                    }
                }

                yield return _sim.Delay(TimeSpan.FromMilliseconds(40));
            }

            yield return null;
        }

    }
}