﻿// #define TIMING_DEBUG

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
        
        private Texture2D _socResultTex, _sscResultTex;
        private Color32[] _socResultTexBuffer, _sscResultTexBuffer;
        
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
            _sim.Listen(OsaState.SOCReceiveTick, _ => { _waitingOnRender = true; });
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

            _processingThreads[SOCIndex].SetOutput(_socResultTexBuffer, _socResultTex.width, _socResultTex.height);

            _setupDone = _socResultTex != null && _sscResultTex != null;
        }

        public void SetSSCTargetTexture(Texture2D tex)
        {
            _sscResultTex = tex;
            _sscResultTexBuffer = tex.GetPixels32();
            
            _processingThreads[SSCIndex].SetOutput(_sscResultTexBuffer, _sscResultTex.width, _sscResultTex.height);
            _setupDone = _socResultTex != null && _sscResultTex != null;
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
                    fade_radar_image((IntPtr)outputBufferPinned, _socResultTex.width, _socResultTex.height, 4, 3);
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
                    Debug.Log($"RadarProcessing threads didn't finish in 64ms!");
                }
                
#if TIMING_DEBUG
                Debug.Log($"Done processing {sw.Elapsed.TotalMilliseconds}ms");
#endif
                
                File.WriteAllBytes("Temp/soc.png", _socResultTex.EncodeToPNG());
                
                _socResultTex.SetPixels32(_socResultTexBuffer);
                _socResultTex.Apply();
                
                _sscResultTex.SetPixels32(_sscResultTexBuffer);
                _sscResultTex.Apply();
                
#if TIMING_DEBUG
                Debug.Log($"Done in {sw.Elapsed.TotalMilliseconds}ms");
#endif
            }
            
            SOCCameraTransformRoot.transform.eulerAngles = new Vector3(-90f, _state.SOCAzimuth, 0f);
            SSCCameraTransformRoot.transform.eulerAngles = new Vector3(-90f, _state.SSCAzimuth, 0f);

            _socThread.Azimuth = _state.SOCAzimuth;
            
            _sscThread.Azimuth = _state.SSCAzimuth;
            _sscThread.TargetGateNearPlane = _state.SSCDistance - 1.5f;
            _sscThread.TargetGateFarPlane = _state.SSCDistance + 1.5f;

            for (int i = 1; i <= 3; i++)
            {
                _controller.SOCUtilityCamera(i).enabled = _state.SOCActiveBeam == i;
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

        // @TODO: move onto separate class?
        [DllImport("radarimg_proc")]
        private static extern int fade_radar_image(IntPtr output, int output_width, int output_height, int output_ch, int output_fade_speed);
    }
}