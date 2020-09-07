using System;
using System.Diagnostics;
using System.IO;
using OsaVR.World.Simulation;
using UnityEngine;
using UnityEngine.UI;

namespace OsaVR.Utils.DebugUtils
{
    public class DebugPaneController: MonoBehaviour
    {
        public GameObject LabelFrametime, LabelSimRate;
        public GameObject ScreenLabelDebug;

        public RenderTexture SaveRT;

        private Text _textFrametime, _textSimRate, _textScreenDebug;

        private Stopwatch _frametimeStopwatch, _fixedFrametimeStopwatch, _labelUpdateStopwatch = Stopwatch.StartNew();
        private float _lastFixedFrametime;

        private SimulationController _sim;

        private void Awake()
        {
            _sim = FindObjectOfType<SimulationController>();
            _textFrametime = LabelFrametime.GetComponent<Text>();
            _textSimRate = LabelSimRate.GetComponent<Text>();
            _textScreenDebug = ScreenLabelDebug.GetComponent<Text>();

            Application.targetFrameRate = 60;
        }

        private void Update()
        {
            if (_frametimeStopwatch != null && _labelUpdateStopwatch.Elapsed > TimeSpan.FromMilliseconds(200))
            {
                _labelUpdateStopwatch = Stopwatch.StartNew();
                
                var frametime = _frametimeStopwatch.ElapsedMilliseconds;
                _textFrametime.text = $"FT: {frametime:F3}ms\nFPS:{1000f / frametime:F0}";

                _textSimRate.text = $"SimLag: {_sim.averageLag:F3}ms\nSimSleep: {_sim.averageSleep:F3}ms";
                _textScreenDebug.text = $"{1000f / frametime:F0} ({frametime:F3}ms) / {_lastFixedFrametime:F0}ms / lag {_sim.averageLag:F3}ms / sleep {_sim.averageSleep:F3}ms";
            }
            
            _frametimeStopwatch = Stopwatch.StartNew();

            if (SaveRT != null)
            {
                var tex = new Texture2D(SaveRT.width, SaveRT.height);

                RenderTexture.active = SaveRT;
                tex.ReadPixels(new Rect(0, 0, tex.width, tex.height), 0, 0);
                File.WriteAllBytes("Temp/savert.png", tex.EncodeToPNG());
            }
        }

        private void FixedUpdate()
        {
            if (_fixedFrametimeStopwatch != null)
            {
                _lastFixedFrametime = _fixedFrametimeStopwatch.ElapsedMilliseconds;
            }
            
            _fixedFrametimeStopwatch = Stopwatch.StartNew();
        }
    }
}