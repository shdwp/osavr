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

        protected void Awake()
        {
            var display = gameObject.FindChildNamed("@ViewSurface");
            var renderer = display.GetComponent<MeshRenderer>();

            _dataTex = new Texture2D(10, 10, TextureFormat.ARGB32, false);
            for (int x = 0; x < 10; x++)
            {
                for (int y = 0; y < 10; y++)
                {
                    _dataTex.SetPixel(x, y, y == 5 ? Color.white : Color.black);
                }
            }
            _dataTex.Apply();
            
            renderer.material.SetTexture("Texture2D_1F77D971", _dataTex);
        }

        public static IViewController AddControllerForViewId(GameObject o, string id)
        {
            switch (id)
            {
                case "signal_scope":
                    return o.AddComponent<ScopeController>();
                
                case "radar_scope":
                    return o.AddComponent<RadarScopeController>();
            }

            throw new NotImplementedException();
        }
    }
}