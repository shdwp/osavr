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

        public RenderTexture SaveRT;

        private Text _textFrametime, _textSimRate;

        private Stopwatch _frametimeStopwatch;

        private SimulationController _sim;

        private void Awake()
        {
            _sim = FindObjectOfType<SimulationController>();
            _textFrametime = LabelFrametime.GetComponent<Text>();
            _textSimRate = LabelSimRate.GetComponent<Text>();

            Application.targetFrameRate = 45;
        }

        private void Update()
        {
            if (_frametimeStopwatch != null)
            {
                var frametime = _frametimeStopwatch.ElapsedMilliseconds;
                _textFrametime.text = $"FT: {frametime:F3}ms\nFPS:{1000f / frametime:F0}";
            }

            _textSimRate.text = $"SimLag: {_sim.averageLag:F3}ms\nSimSleep: {_sim.averageSleep:F3}ms";
            
            _frametimeStopwatch = Stopwatch.StartNew();

            if (SaveRT != null)
            {
                var tex = new Texture2D(SaveRT.width, SaveRT.height);

                RenderTexture.active = SaveRT;
                tex.ReadPixels(new Rect(0, 0, tex.width, tex.height), 0, 0);
                File.WriteAllBytes("Temp/savert.png", tex.EncodeToPNG());
            }
        }
    }
}