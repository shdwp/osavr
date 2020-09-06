using System;
using OsaVR.Utils;
using UnityEngine;

namespace OsaVR.CockpitFramework.ViewControllers.Indicator
{
    public class IndicatorController: ADisplayController
    {
        private static readonly int _BaseTex = Shader.PropertyToID("_BaseTex");
        private static readonly int _IllumTex = Shader.PropertyToID("_IllumTex");
        private static readonly int _IllumMaskTex = Shader.PropertyToID("_IllumMaskTex");

        protected Texture2D _baseTex;
        protected Texture2D _illuminatedTex;

        protected new void Awake()
        {
            _viewSurfaceMat = Resources.Load<Material>("Controls/generic_indicator/IndicatorMat");
            base.Awake();

            if (_baseTex != null)
            {
                _viewSurfaceMat.SetTexture(_BaseTex, _baseTex);
            }

            if (_illuminatedTex != null)
            {
                _viewSurfaceMat.SetTexture(_IllumTex, _illuminatedTex);
            }

            _viewSurfaceMat.SetTexture(_IllumMaskTex, dataTex);
        }
    }
}