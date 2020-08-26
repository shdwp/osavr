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
            
            for (int x = 0; x < 10; x++)
            {
                for (int y = 0; y < 10; y++)
                {
                    _dataTex.SetPixel(x, y, y == 5 ? Color.white : Color.black);
                }
            }
            
            _dataTex.Apply();
            //_viewSurfaceMat.SetTexture(_shaderidInput, Resources.Load<Texture2D>("scope_input"));
            _viewSurfaceMat.SetTexture(_shaderidInput, _dataTex);
        }

        protected void FixedUpdate()
        {
        }
    }
}