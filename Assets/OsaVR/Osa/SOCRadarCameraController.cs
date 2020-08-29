using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading;
using OsaVR.Osa.Model;
using OsaVR.Osa.ViewControllers.SOCScope;
using OsaVR.RadarSimulation;
using OsaVR.World.Simulation;
using Unity.Jobs;
using UnityEngine;
using Debug = UnityEngine.Debug;

namespace OsaVR.Osa
{
    public class SOCRadarCameraController: MonoBehaviour
    {
        public RenderTexture Beam1RT, Beam2RT, Beam3RT;
        public GameObject CameraTransformRoot;
        
        private SimulationController _sim;
        private OsaState _state;
        private SOCScopeController _scope;

        private RadarProcessingThread[] _processingThreads;
        private Texture2D _resultTex;
        private Color32[] _resultTexBuffer;
        
        private bool _waitingOnRender = false;

        private void Awake()
        {
            _sim = FindObjectOfType<SimulationController>();
        }

        private void Start()
        {
            _sim.Listen(OsaState.SOCReceiveTick, _ => { _waitingOnRender = true; });
            _state = FindObjectOfType<OsaState>();

            _processingThreads = new[]
            {
                new RadarProcessingThread(Beam1RT, _resultTexBuffer, _resultTex.width, _resultTex.height), 
                new RadarProcessingThread(Beam2RT, _resultTexBuffer, _resultTex.width, _resultTex.height), 
                new RadarProcessingThread(Beam3RT, _resultTexBuffer, _resultTex.width, _resultTex.height), 
            };
        }

        private void Update()
        {
            if (_scope == null)
            {
                _scope = FindObjectOfType<SOCScopeController>();
                _resultTex = _scope.dataTex;
            }

            CameraTransformRoot.transform.eulerAngles = new Vector3(-90f, _state.SOCAzimuth, 0f);
        }

        private unsafe void LateUpdate()
        {
            if (_waitingOnRender && _scope != null)
            {
                var sw = Stopwatch.StartNew();
                
                fixed (Color32* outputBufferPinned = _resultTexBuffer)
                {
                    fade_radar_image((IntPtr)outputBufferPinned, _resultTex.width, _resultTex.height, 4, 3);
                }
                
                Debug.Log($"Faded in {sw.Elapsed.TotalMilliseconds}ms");

                var handles = new List<WaitHandle>(_processingThreads.Length);
                foreach (var th in _processingThreads)
                {
                    handles.Add(th.ProcessImage());
                }
                Debug.Log($"Started threads in {sw.Elapsed.TotalMilliseconds}ms");

                WaitHandle.WaitAll(handles.ToArray());
                
                Debug.Log($"Done in {sw.Elapsed.TotalMilliseconds}ms");
            }
        }

        // @TODO: move onto separate class?
        [DllImport("radarimg_proc")]
        private static extern int fade_radar_image(IntPtr output, int output_width, int output_height, int output_ch, int output_fade_speed);
    }
}