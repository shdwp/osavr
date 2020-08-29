using OsaVR.CockpitFramework.ViewControllers;
using OsaVR.Osa.Model;
using UnityEngine;

namespace OsaVR.Osa.ViewControllers.SOCScope
{
    public class SOCScopeController: AViewController
    {
        private OsaState _state;
        private static readonly int _shaderidInput = Shader.PropertyToID("Texture2D_43292419");
        private static readonly int _shaderidSOCAzimuth = Shader.PropertyToID("Vector1_FC501ECE");
        private static readonly int _shaderidSSCAzimuth = Shader.PropertyToID("Vector1_7BFDD6BB");
        private static readonly int _shaderidSSCDistance = Shader.PropertyToID("Vector1_6C640CCF");

        protected new void Awake()
        {
            _viewSurfaceMat = Resources.Load<Material>("Controls/radar_scope/RadarScopeMat");
            base.Awake();
            
            //_viewSurfaceMat.SetTexture(_shaderidInput, Resources.Load<Texture2D>("radar_input"));
            _viewSurfaceMat.SetTexture(_shaderidInput, dataTex);
        }
        
        private void Start()
        {
            _state = FindObjectOfType<OsaState>();
            
            FindObjectOfType<RadarCameraProcessingController>().SetSOCTargetTexture(dataTex);
        }

        private void Update()
        {
            _viewSurfaceMat.SetFloat(_shaderidSOCAzimuth, _state.SOCAzimuth * Mathf.Deg2Rad);
            _viewSurfaceMat.SetFloat(_shaderidSSCAzimuth, _state.SSCAzimuth * Mathf.Deg2Rad);
            _viewSurfaceMat.SetFloat(_shaderidSSCDistance, _state.SSCDistance / 30f);
        }
    }
}