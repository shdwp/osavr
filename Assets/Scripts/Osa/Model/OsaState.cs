using System;
using System.Collections;
using UnityEngine;

namespace Osa.Model
{
    public class OsaState: MonoBehaviour
    {
        /// <summary>
        /// Scanline azimuth in degrees
        /// </summary>
        public float Scanline_Azimuth = 0;
        
        /// <summary>
        /// Signalscope azimuth in degrees
        /// </summary>
        public float Signalscope_Azimuth = 0;
        
        /// <summary>
        /// Signalscope distance in degrees
        /// </summary>
        public float Signalscope_Distance = 0;

        private SimulationRunloop _runloop;

        private void Start()
        {
            _runloop = FindObjectOfType<SimulationRunloop>();
            _runloop.Add(new SimulationProcess(0, ScanlineRotationProcess()));
            Signalscope_Distance = 15f;
        }

        public IEnumerator ScanlineRotationProcess()
        {
            while (true)
            {
                Scanline_Azimuth = MathUtils.NormalizeAzimuth(Scanline_Azimuth + 1.5f);
                Signalscope_Azimuth = MathUtils.NormalizeAzimuth( Signalscope_Azimuth + 0.3f);
                yield return _runloop.Delay(10f);
            }
        }
    }
}