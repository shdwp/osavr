using System;
using OsaVR.CockpitFramework.ViewControllers;
using UnityEngine;

namespace OsaVR.Osa.ViewControllers.SSCScope
{
    public class SSCScopeController: ADisplayController
    {
        private static readonly int _shaderidInput = Shader.PropertyToID("Texture2D_1F77D971");
        
        protected new void Awake()
        {
            _viewSurfaceMat = Resources.Load<Material>("Controls/ssc_scope/SSCScopeMat");
            base.Awake();
            _viewSurfaceMat.SetTexture(_shaderidInput, dataTex);
        }

        private void Start()
        {
            FindObjectOfType<RadarCameraProcessingController>().SetSSCTargetTexture(dataTex);
        }

        protected void FixedUpdate()
        {
        }
    }
}