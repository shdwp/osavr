using System;
using OsaVR.Osa.ViewControllers;
using OsaVR.Utils;
using UnityEngine;

namespace OsaVR.CockpitFramework.ViewControllers
{
    public interface IViewController
    {
        
    }
    
    public abstract class AViewController: MonoBehaviour, IViewController
    {
        public Texture2D _dataTex;
        protected Material _viewSurfaceMat;

        protected void Awake()
        {
            var display = gameObject.FindChildNamed("@ViewSurface");
            var renderer = display.GetComponent<MeshRenderer>();

            _dataTex = new Texture2D(256, 256, TextureFormat.ARGB32, false);
            for (int x = 0; x < _dataTex.width; x++)
            {
                for (int y = 0; y < _dataTex.height; y++)
                {
                    _dataTex.SetPixel(x, y, Color.black);
                }
            }
            _dataTex.Apply();

            renderer.material = _viewSurfaceMat;
        }

        public static IViewController AddControllerForViewId(GameObject o, string id)
        {
            switch (id)
            {
                case "signal_scope":
                    return o.AddComponent<SignalScopeController>();
                
                case "radar_scope":
                    return o.AddComponent<RadarScopeController>();
            }

            throw new NotImplementedException();
        }
    }
}