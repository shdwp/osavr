using OsaVR.CockpitFramework.ViewControllers;
using UnityEngine;

namespace OsaVR.Osa.DisplayControllers.SSCElevationScope
{
    public class SSCElevationScopeController: ADisplayController
    {
        private static readonly int _shaderidInput = Shader.PropertyToID("_InputTex");
        private static readonly int _shaderidElevation = Shader.PropertyToID("_Elevation");
        
        protected new void Awake()
        {
            _viewSurfaceMat = Resources.Load<Material>("Controls/ssc_elevation_scope/mat");
            base.Awake();
            _viewSurfaceMat.SetTexture(_shaderidInput, dataTex);
        }

        private void Start()
        {
            FindObjectOfType<RadarCameraProcessingController>().SetSSCElevationTargetTexture(dataTex);
        }
    }
}