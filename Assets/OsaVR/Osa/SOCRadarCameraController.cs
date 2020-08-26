using System;
using System.Collections;
using System.Threading;
using OsaVR.Osa.Model;
using OsaVR.World.Simulation;
using UnityEngine;

namespace OsaVR.Osa
{
    public class SOCRadarCameraController: MonoBehaviour
    {
        private SimulationController _sim;
        private OsaState _state;
        
        private Camera _cam;
        private RenderTexture _rt;
        
        private Texture2D _tex;
        private bool _waitingOnRender = false;

        private void Awake()
        {
            _sim = FindObjectOfType<SimulationController>();
        }

        private void Start()
        {
            _sim.Listen(OsaState.SOCReceiveTick, _ => { _waitingOnRender = true; });
            _state = FindObjectOfType<OsaState>();
            
            _cam = GetComponent<Camera>();
            
            _rt = _cam.targetTexture;
            _tex = new Texture2D(_rt.width, _rt.height);
        }

        private void LateUpdate()
        {
            if (_waitingOnRender)
            {
                _waitingOnRender = false;

                RenderTexture.active = _rt;
                _tex.ReadPixels(new Rect(0, 0, _rt.width, _rt.height), 0, 0);
            }
        }
    }
}