using System;
using OsaVR.CockpitFramework.ViewControllers;
using UnityEngine;

namespace OsaVR.Osa.ViewControllers.SSCScope
{
    public class SSCScopeController: AViewController
    {
        private static readonly int _shaderidInput = Shader.PropertyToID("Texture2D_1F77D971");
        
        protected new void Awake()
        {
            _viewSurfaceMat = Resources.Load<Material>("Controls/scope/SSCScopeMat");
            base.Awake();
            
            dataTex.Apply();
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