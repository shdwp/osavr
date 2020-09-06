using System;
using OsaVR.Osa.ViewControllers;
using OsaVR.Osa.ViewControllers.SOCScope;
using OsaVR.Osa.ViewControllers.SSCScope;
using OsaVR.Utils;
using UnityEngine;

namespace OsaVR.CockpitFramework.ViewControllers
{
    public interface IViewController
    {
        
    }
    
    public abstract class ADisplayController: MonoBehaviour, IViewController
    {
        public int dataWidth = 256, dataHeight = 256;
        public Texture2D dataTex;
        protected Material _viewSurfaceMat;

        protected void Awake()
        {
            var display = gameObject.FindChildNamed("@ViewSurface");
            var renderer = display.GetComponent<MeshRenderer>();

            dataTex = new Texture2D(dataWidth, dataHeight, TextureFormat.ARGB32, false);
            dataTex.SetPixels32(new Color32[dataWidth * dataHeight]);
            dataTex.Apply();

            renderer.material = _viewSurfaceMat;
        }
    }
}