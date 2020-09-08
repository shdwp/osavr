// #define TIMING_DEBUG

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
    public class RadarCameraProcessingController: MonoBehaviour
    {
        public RenderTexture SOCRenderTexture, SSCRenderTexture;
        
        public GameObject SOCCameraTransformRoot, SSCCameraTransformRoot;
        
        private SimulationController _sim;
        private OsaState _state;
        private OsaController _controller;
        private SOCScopeController _scope;

        private IRadarProcessingThread[] _processingThreads;
        private SOCImageProcessingThread _socThread;
        private SSCImageProcessingThread _sscThread;
        
        private Texture2D _socResultTex, _sscResultTex, _sscElevResultTex;
        private Color32[] _socResultTexBuffer, _sscResultTexBuffer, _sscElevResultTexBuffer;
        
        private bool _waitingOnRender = false;
        private bool _setupDone = false;

        private static int SOCIndex = 0, SSCIndex = 1;

        private void Awake()
        {
            _sim = FindObjectOfType<SimulationController>();
            _controller = FindObjectOfType<OsaController>();
        }

        private void Start()
        {
            _sim.ListenProcessEvent(SOCState.ReceiveEvent, _ => { _waitingOnRender = true; });
            _state = FindObjectOfType<OsaState>();

            _socThread = new SOCImageProcessingThread(SOCRenderTexture);
            _sscThread = new SSCImageProcessingThread(SSCRenderTexture);

            _processingThreads = new IRadarProcessingThread[2];
            _processingThreads[SOCIndex] = _socThread;
            _processingThreads[SSCIndex] = _sscThread;
            
            foreach (var thread in _processingThreads)
            {
                thread.Start();
            }
        }

        public void SetSOCTargetTexture(Texture2D tex)
        {
            _socResultTex = tex;
            _socResultTexBuffer = tex.GetPixels32();
            _processingThreads[SOCIndex].SetOutput(SOCImageProcessingThread.ScopeIndex, _socResultTexBuffer, _socResultTex.width, _socResultTex.height);

            _setupDone = _socResultTex != null && _sscResultTex != null && _sscElevResultTex != null;
        }

        public void SetSSCTargetTexture(Texture2D tex)
        {
            _sscResultTex = tex;
            _sscResultTexBuffer = tex.GetPixels32();
            
            _processingThreads[SSCIndex].SetOutput(SSCImageProcessingThread.SignalScopeIndex, _sscResultTexBuffer, _sscResultTex.width, _sscResultTex.height);
            _setupDone = _socResultTex != null && _sscResultTex != null && _sscElevResultTex != null;
        }

        public void SetSSCElevationTargetTexture(Texture2D tex)
        {
            _sscElevResultTex = tex;
            _sscElevResultTexBuffer = tex.GetPixels32();
            
            _processingThreads[SSCIndex].SetOutput(SSCImageProcessingThread.ElevationScopeIndex, _sscElevResultTexBuffer, _sscElevResultTex.width, _sscElevResultTex.height);
            _setupDone = _socResultTex != null && _sscResultTex != null && _sscElevResultTex != null;
        }

        private unsafe void Update()
        {
            if (_waitingOnRender && _setupDone)
            {
                _waitingOnRender = false;
#if TIMING_DEBUG
                var sw = Stopwatch.StartNew();
#endif
                
                fixed (Color32* outputBufferPinned = _socResultTexBuffer)
                {
                    var output = new NativeOutputStruct
                    {
                        buf = (IntPtr) outputBufferPinned,
                        width = _socResultTex.width,
                        height = _socResultTex.height,
                        channels = 4,
                        far_plane = 0,
                        near_plane = 0
                    };
                    
                    RadarProcNative.fade_radar_image(output, 1);
                }
                
#if TIMING_DEBUG
                Debug.Log($"Faded in {sw.Elapsed.TotalMilliseconds}ms");
#endif

                var handles = new WaitHandle[_processingThreads.Length];
                for (int i = 0; i < _processingThreads.Length; i++)
                {
                    handles[i] = _processingThreads[i].StartProcessing();
                }
                
#if TIMING_DEBUG
                Debug.Log($"Started threads in {sw.Elapsed.TotalMilliseconds}ms");
#endif

                if (!WaitHandle.WaitAll(handles, 64))
                {
                    File.WriteAllBytes("Temp/slog_soc.png", _processingThreads[SOCIndex].InputData());
                    File.WriteAllBytes("Temp/slog_ssc.png", _processingThreads[SSCIndex].InputData());
                    Debug.Log($"RadarProcessing threads didn't finish in 64ms!");
                }
                
#if TIMING_DEBUG
                Debug.Log($"Done processing {sw.Elapsed.TotalMilliseconds}ms");
#endif
                
                _socResultTex.SetPixels32(_socResultTexBuffer);
                _socResultTex.Apply();
                
                _sscResultTex.SetPixels32(_sscResultTexBuffer);
                _sscResultTex.Apply();
                
                _sscElevResultTex.SetPixels32(_sscElevResultTexBuffer);
                _sscElevResultTex.Apply();
                
#if TIMING_DEBUG
                Debug.Log($"Done in {sw.Elapsed.TotalMilliseconds}ms");
#endif
            }
            
            SOCCameraTransformRoot.transform.eulerAngles = new Vector3(-90f, _state.SOC.azimuth, 0f);
            SSCCameraTransformRoot.transform.eulerAngles = new Vector3(-90f + _state.SSC.elevation, _state.SSC.azimuth, 0f);

            _socThread.Azimuth = _state.SOC.azimuth;
            switch (_state.SOC.ScopeScopeDisplayRange)
            {
                case SOCState.ScopeDisplayRange.Zero_Fifteen:
                    _socThread.OutputNearPlane = 0f;
                    _socThread.OutputFarPlane = 15f;
                    break;
                
                case SOCState.ScopeDisplayRange.Zero_ThirtyFive:
                    _socThread.OutputNearPlane = 0f;
                    _socThread.OutputFarPlane = 35f;
                    break;
                
                case SOCState.ScopeDisplayRange.Ten_FortyFive:
                    _socThread.OutputNearPlane = 10f;
                    _socThread.OutputFarPlane = 45f;
                    break;
            }
            
            _sscThread.Azimuth = _state.SSC.azimuth;
            _sscThread.TargetGateNearPlane = _state.SSC.distance - 1.5f;
            _sscThread.TargetGateFarPlane = _state.SSC.distance + 1.5f;

            for (int i = 1; i <= 3; i++)
            {
                _controller.SOCUtilityCamera(i).enabled = _state.SOC.activeBeam == i;
            }
        }

        private unsafe void LateUpdate()
        {
        }

        private void OnApplicationQuit()
        {
            foreach (var thread in _processingThreads)
            {
                thread.Stop();
            }
        }
    }
}