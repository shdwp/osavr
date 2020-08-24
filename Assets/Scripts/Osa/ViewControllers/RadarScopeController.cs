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
        private static readonly int _shaderidInput = Shader.PropertyToID("Texture2D_43292419");
        private static readonly int _shaderidSOCAzimuth = Shader.PropertyToID("Vector1_FC501ECE");
        private static readonly int _shaderidSSCAzimuth = Shader.PropertyToID("Vector1_7BFDD6BB");
        private static readonly int _shaderidSSCDistance = Shader.PropertyToID("Vector1_6C640CCF");

        protected new void Awake()
        {
            _viewSurfaceMat = Resources.Load<Material>("Controls/radar_scope/radar_display_mat");
            base.Awake();
            
            _viewSurfaceMat.SetTexture(_shaderidInput, Resources.Load<Texture2D>("radar_input"));
        }
        
        private void Start()
        {
            _state = FindObjectOfType<OsaState>();
        }

        private void Update()
        {
            _viewSurfaceMat.SetFloat(_shaderidSOCAzimuth, _state.SOCAzimuth * Mathf.Deg2Rad);
            _viewSurfaceMat.SetFloat(_shaderidSSCAzimuth, _state.SSCAzimuth * Mathf.Deg2Rad);
            _viewSurfaceMat.SetFloat(_shaderidSSCDistance, _state.SSCDistance / 30f);
        }
    }
}