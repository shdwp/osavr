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
    
    public abstract class AViewController: MonoBehaviour, IViewController
    {
        public Texture2D DataTex;
        protected Material _viewSurfaceMat;

        protected void Awake()
        {
            var display = gameObject.FindChildNamed("@ViewSurface");
            var renderer = display.GetComponent<MeshRenderer>();

            DataTex = new Texture2D(256, 256, TextureFormat.ARGB32, false);
            for (int x = 0; x < DataTex.width; x++)
            {
                for (int y = 0; y < DataTex.height; y++)
                {
                    DataTex.SetPixel(x, y, Color.black);
                }
            }
            DataTex.Apply();

            renderer.material = _viewSurfaceMat;
        }

        public static IViewController AddControllerForViewId(GameObject o, string id)
        {
            switch (id)
            {
                case "signal_scope":
                    return o.AddComponent<SSCScopeController>();
                
                case "radar_scope":
                    return o.AddComponent<SOCScopeController>();
            }

            throw new NotImplementedException();
        }
    }
}