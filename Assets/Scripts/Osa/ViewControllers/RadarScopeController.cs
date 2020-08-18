using System;
using Interactor;
using Osa.Model;
using Osa.ViewControllers;
using UnityEngine;

namespace Osa
{
    public class RadarScopeController: AViewController
    {
        private OsaState _state;

        protected new void Awake()
        {
            base.Awake();
            _viewSurfaceMat.SetTexture("Texture2D_43292419", Resources.Load<Texture2D>("radar_input"));
        }
        
        private void Start()
        {
            _state = FindObjectOfType<OsaState>();
        }

        private void Update()
        {
            _viewSurfaceMat.SetFloat("Vector1_FC501ECE", _state.Scanline_Azimuth * Mathf.Deg2Rad);
            _viewSurfaceMat.SetFloat("Vector1_7BFDD6BB", _state.Signalscope_Azimuth * Mathf.Deg2Rad);
            _viewSurfaceMat.SetFloat("Vector1_6C640CCF", _state.Signalscope_Distance / 30f);
        }
    }
}