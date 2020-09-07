using OsaVR.CockpitFramework.ViewControllers;
using OsaVR.Osa.Model;
using OsaVR.World.Simulation;
using UnityEngine;

namespace OsaVR.Osa.ViewControllers.SOCScope
{
    public class SOCScopeController: OsaDisplayController
    {
        private static readonly int _shaderidInput = Shader.PropertyToID("_InputTex");
        private static readonly int _shaderidSOCAzimuth = Shader.PropertyToID("_SOC_Azimuth");
        private static readonly int _shaderidSOCRangeMarkerMargin = Shader.PropertyToID("_SOC_RangeMarker_Margin");
        private static readonly int _shaderidSOCRangeMarkerCount = Shader.PropertyToID("_SOC_RangeMarker_Count");
        
        private static readonly int _shaderidSSCAzimuth = Shader.PropertyToID("_SSC_Azimuth");
        private static readonly int _shaderidSSCDistance = Shader.PropertyToID("_SSC_Distance");

        private bool _update = true;

        protected new void Awake()
        {
            _viewSurfaceMat = Resources.Load<Material>("Controls/soc_scope/RadarScopeMat");
            base.Awake();
            _viewSurfaceMat.SetTexture(_shaderidInput, dataTex);
        }
        
        private void Start()
        {
            FindObjectOfType<RadarCameraProcessingController>().SetSOCTargetTexture(dataTex);
            
            _sim.ListenNotification(SOCState.ChangedDisplayRange, (_) => _update = true);
        }

        private void Update()
        {
            _viewSurfaceMat.SetFloat(_shaderidSOCAzimuth, _state.SOC.azimuth * Mathf.Deg2Rad);
            _viewSurfaceMat.SetFloat(_shaderidSSCAzimuth, _state.SSC.azimuth * Mathf.Deg2Rad);

            switch (_state.SOC.ScopeScopeDisplayRange)
            {
                case SOCState.ScopeDisplayRange.Zero_Fifteen:
                    _viewSurfaceMat.SetFloat(_shaderidSOCRangeMarkerMargin, 2f / 15f);
                    _viewSurfaceMat.SetFloat(_shaderidSOCRangeMarkerCount, 3f);
                    _viewSurfaceMat.SetFloat(_shaderidSSCDistance, Mathf.InverseLerp(0f, 15f, _state.SSC.distance));
                    break;
                    
                case SOCState.ScopeDisplayRange.Zero_ThirtyFive:
                    _viewSurfaceMat.SetFloat(_shaderidSOCRangeMarkerMargin, 5f / 35f);
                    _viewSurfaceMat.SetFloat(_shaderidSOCRangeMarkerCount, 7f);
                    _viewSurfaceMat.SetFloat(_shaderidSSCDistance, Mathf.InverseLerp(0f, 35f, _state.SSC.distance));
                    break;
                    
                case SOCState.ScopeDisplayRange.Ten_FortyFive:
                    _viewSurfaceMat.SetFloat(_shaderidSOCRangeMarkerMargin, 5f / 35f);
                    _viewSurfaceMat.SetFloat(_shaderidSOCRangeMarkerCount, 7f);
                    _viewSurfaceMat.SetFloat(_shaderidSSCDistance, Mathf.InverseLerp(10f, 45f, _state.SSC.distance));
                    break;
            }
                
            _update = false;
        }
    }
}