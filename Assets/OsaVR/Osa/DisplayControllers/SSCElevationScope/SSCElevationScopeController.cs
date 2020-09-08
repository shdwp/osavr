using System;
using OsaVR.CockpitFramework.ViewControllers;
using OsaVR.Osa.Model;
using UnityEngine;

namespace OsaVR.Osa.DisplayControllers.SSCElevationScope
{
    public class SSCElevationScopeController: ADisplayController
    {
        private static readonly int _shaderidInput = Shader.PropertyToID("_InputTex");
        private static readonly int _shaderidElevation = Shader.PropertyToID("_Elevation");

        private OsaState _state;
        
        protected new void Awake()
        {
            _viewSurfaceMat = Resources.Load<Material>("Controls/ssc_elevation_scope/mat");
            base.Awake();
            _viewSurfaceMat.SetTexture(_shaderidInput, dataTex);

            _state = FindObjectOfType<OsaState>();
        }

        private void Start()
        {
            FindObjectOfType<RadarCameraProcessingController>().SetSSCElevationTargetTexture(dataTex);
        }

        private void Update()
        {
            switch (_state.SOC.activeBeam)
            {
                case 1:
                    _viewSurfaceMat.SetFloat(_shaderidElevation, Mathf.InverseLerp(-1f, 5f, _state.SSC.elevation));
                    break;
                
                case 2:
                    _viewSurfaceMat.SetFloat(_shaderidElevation, Mathf.InverseLerp(3f, 9f, _state.SSC.elevation));
                    break;
                
                case 3:
                    _viewSurfaceMat.SetFloat(_shaderidElevation, Mathf.InverseLerp(6f, 30f, _state.SSC.elevation));
                    break;
            }
        }
    }
}