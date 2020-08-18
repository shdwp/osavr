using System;
using UnityEngine;

namespace Osa.ViewControllers
{
    public interface IViewController
    {
        
    }
    
    public abstract class AViewController: MonoBehaviour, IViewController
    {
        protected Texture2D _dataTex;
        protected Material _viewSurfaceMat;

        protected void Awake()
        {
            var display = gameObject.FindChildNamed("@ViewSurface");
            var renderer = display.GetComponent<MeshRenderer>();

            _dataTex = new Texture2D(10, 10, TextureFormat.ARGB32, false);
            for (int x = 0; x < 10; x++)
            {
                for (int y = 0; y < 10; y++)
                {
                    _dataTex.SetPixel(x, y, Color.black);
                }
            }
            _dataTex.Apply();
            
            _viewSurfaceMat = renderer.material;
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