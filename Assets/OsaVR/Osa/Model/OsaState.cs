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
        public static readonly uint OsaSimOffset = 0;
        public static readonly uint SSCSimOffset = 100;
        public static readonly uint SOCSimOffset = 200;

        public Vector3 worldPosition;
        public Vector3 worldForwardVector;

        public SOCState SOC;
        public SSCState SSC;

        private SimulationController _sim;

        private void Awake()
        {
            _sim = FindObjectOfType<SimulationController>();
        }

        private void Start()
        {
            SOC = new SOCState(_sim);
            SSC = new SSCState(_sim, this);
        }
    }
}