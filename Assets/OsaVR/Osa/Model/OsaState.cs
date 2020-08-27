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
        /// SSC azimuth in degrees
        /// </summary>
        public float SSCAzimuth = 0;

        /// <summary>
        /// Whether search radar is currently turning
        /// </summary>
        public bool SOCTurning = true;

        /// <summary>
        /// Whether search radar is currently emitting and receiving returns
        /// </summary>
        public bool SOCEmitting = true;
        
        /// <summary>
        /// SSC distance in degrees
        /// </summary>
        public float SSCDistance = 0;

        /// <summary>
        /// 
        /// </summary>
        public Vector3 WorldPosition;

        /// <summary>
        /// 
        /// </summary>
        public Vector3 ForwardVector;

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
                if (SOCTurning)
                {
                    SOCAzimuth = MathUtils.NormalizeAzimuth(SOCAzimuth + 3f);
                }
                
                yield return new SimulationEvent(SOCTurnTick);

                if (SOCEmitting)
                {
                }
                
                yield return new SimulationEvent(SOCReceiveTick);
                yield return _sim.Delay(TimeSpan.FromMilliseconds(15.5));
            }
        }
    }
}