using System;
using System.Collections;
using UnityEngine;

namespace Osa.Model
{
    public class OsaState: MonoBehaviour
    {
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

        private SimulationRunloop _runloop;

        private void Start()
        {
            _runloop = FindObjectOfType<SimulationRunloop>();
            _runloop.Add(new SimulationProcess(0, SOC_Process()));
            SSCDistance = 15f;
        }

        public IEnumerator SOC_Process()
        {
            while (true)
            {
                if (SOCTurning)
                {
                    SOCAzimuth = MathUtils.NormalizeAzimuth(SOCAzimuth + 1.5f);
                }

                if (SOCEmitting)
                {
                }

                yield return _runloop.Delay(10f);
            }
        }
    }
}