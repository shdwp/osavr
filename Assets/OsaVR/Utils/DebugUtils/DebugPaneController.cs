using System;
using System.Diagnostics;
using OsaVR.World.Simulation;
using UnityEngine;
using UnityEngine.UI;

namespace OsaVR.Utils.DebugUtils
{
    public class DebugPaneController: MonoBehaviour
    {
        public GameObject LabelFrametime, LabelSimRate;

        private Text _textFrametime, _textSimRate;

        private Stopwatch _frametimeStopwatch;

        private SimulationController _sim;

        private void Awake()
        {
            _sim = FindObjectOfType<SimulationController>();
            _textFrametime = LabelFrametime.GetComponent<Text>();
            _textSimRate = LabelSimRate.GetComponent<Text>();
        }

        private void Update()
        {
            if (_frametimeStopwatch != null)
            {
                var frametime = _frametimeStopwatch.ElapsedMilliseconds;
                _textFrametime.text = $"FT: {frametime:F3}ms\nFPS:{1000f / frametime:F0}";
            }

            _textSimRate.text = $"SimLag: {_sim.AverageLag:F3}ms\nSimSleep: {_sim.AverageSleep:F3}ms";
            
            _frametimeStopwatch = Stopwatch.StartNew();
        }
    }
}