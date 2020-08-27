using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
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
        
        private Dictionary<RenderTexture, Texture2D> _rts = new Dictionary<RenderTexture, Texture2D>();
        private Texture2D _resultTex;
        
        private bool _waitingOnRender = false;

        private void Awake()
        {
            _sim = FindObjectOfType<SimulationController>();
        }

        private void Start()
        {
            _sim.Listen(OsaState.SOCReceiveTick, _ => { _waitingOnRender = true; });
            _state = FindObjectOfType<OsaState>();

            _rts[Beam1RT] = new Texture2D(Beam1RT.width, Beam2RT.height);
            _rts[Beam2RT] = new Texture2D(Beam1RT.width, Beam2RT.height);
            _rts[Beam3RT] = new Texture2D(Beam3RT.width, Beam2RT.height);
        }

        private void Update()
        {
            if (_scope == null)
            {
                _scope = FindObjectOfType<SOCScopeController>();
                _resultTex = _scope.DataTex;
            }

            CameraTransformRoot.transform.eulerAngles = new Vector3(-90f, _state.SOCAzimuth, 0f);
        }
        
        private unsafe void LateUpdate()
        {
            if (_waitingOnRender && _scope != null)
            {
                _waitingOnRender = false;

                var handles_idx = 0;
                var handles = new List<JobHandle>(_rts.Count);
                
                var outputBuffer = _resultTex.GetPixels32();
                
                fixed (Color32* output_buffer_pinned = outputBuffer)
                {
                    fade_radar_image((IntPtr) output_buffer_pinned, _resultTex.width, _resultTex.height, 4, 3);
                    
                    foreach (var kv in _rts)
                    {
                        if (!kv.Key.name.EndsWith("_2"))
                        {
                            // continue;
                        }
                        
                        RenderTexture.active = kv.Key;
                        kv.Value.ReadPixels(new Rect(0, 0, kv.Value.width, kv.Value.height), 0, 0);
                        // File.WriteAllBytes("Temp/input.png", kv.Value.EncodeToPNG());
                        
                        var input_buffer = kv.Value.GetPixels32();
                        fixed (Color32* input_buffer_pinned = input_buffer)
                        {
                            var job = new RadarProcessingJob(
                                (IntPtr)input_buffer_pinned,
                                kv.Value.width,
                                kv.Value.height,
                                (IntPtr)output_buffer_pinned,
                                _resultTex.width,
                                _resultTex.height,
                                0,
                                4,
                                80,
                                0,
                                90
                            );

                            handles.Add(job.Schedule());
                        }
                    }
                }

                foreach (var handle in handles)
                {
                    handle.Complete();
                }
                
                _resultTex.SetPixels32(outputBuffer);
                _resultTex.Apply();
                //File.WriteAllBytes("Temp/output.png", _resultTex.EncodeToPNG());
            }
        }

        [DllImport("radarimg_proc")]
        private static extern int fade_radar_image(IntPtr output, int output_width, int output_height, int output_ch, int output_fade_speed);
    }
}