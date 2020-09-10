using OsaVR.Utils;
using UnityEngine;

namespace OsaVR.CockpitFramework.ViewControllers.RotatingDial
{
    public class GenericCircularDialController : MonoBehaviour, IViewController
    {
        private static readonly int _shaderidDialRotation = Shader.PropertyToID("_DialRotation");

        private Material _mat;
        
        protected void Awake()
        {
            var display = gameObject.FindChildNamed("@ViewSurface");
            var renderer = display.GetComponent<MeshRenderer>();

            _mat = Instantiate(Resources.Load<Material>("Controls/generic_circular_dial/mat"));
            renderer.material = _mat;
        }

        protected void SetDialRotation(float value)
        {
            _mat.SetFloat(_shaderidDialRotation, value);
        }
    }
}
