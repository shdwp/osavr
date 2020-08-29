using System;
using System.Collections;
using System.Diagnostics;
using System.Threading;
using OsaVR.Utils;
using OsaVR.World.Simulation;
using UnityEngine;
using Debug = UnityEngine.Debug;

namespace OsaVR.Osa.Model
{
    public class OsaState: MonoBehaviour
    {
        public static readonly uint SOCTurnTick = 2;
        public static readonly uint SOCReceiveTick = 1;
        
        /// <summary>
        /// SOC azimuth in degrees
        /// </summary>
        public float SOCAzimuth = 0;

        /// <summary>
        /// Whether search radar is currently turning
        /// </summary>
        public bool SOCTurning = true;

        /// <summary>
        /// Whether search radar is currently emitting and receiving returns
        /// </summary>
        public bool SOCEmitting = true;

        /// <summary>
        /// Active SOC beam
        /// </summary>
        public uint SOCActiveBeam = 2;
        
        /// <summary>
        /// SSC azimuth in degrees
        /// </summary>
        public float SSCAzimuth = 0;
        
        /// <summary>
        /// SSC distance in degrees
        /// </summary>
        public float SSCDistance = 0;

        /// <summary>
        /// 
        /// </summary>
        public Vector3 worldPosition;

        /// <summary>
        /// 
        /// </summary>
        public Vector3 worldForwardVector;

        private SimulationController _sim;

        private void Awake()
        {
            _sim = FindObjectOfType<SimulationController>();
        }

        private void Start()
        {
            _sim.Add(new SimulationProcess(0, SOC_Process()));
            SSCDistance = 15f;
        }

        public IEnumerator SOC_Process()
        {
            while (true)
            {
                // 1.818s per revolution
                // 90 adjustments per rotation
                // 20.13ms per adjustment
                
                if (SOCTurning)
                {
                    SOCAzimuth = MathUtils.NormalizeAzimuth(SOCAzimuth + 4f);
                    yield return new SimulationEvent(SOCTurnTick);
                }
                
                if (SOCEmitting)
                {
                    yield return new SimulationEvent(SOCReceiveTick);
                }
                
                yield return _sim.Delay(TimeSpan.FromMilliseconds(20.13));
            }
        }
    }
}