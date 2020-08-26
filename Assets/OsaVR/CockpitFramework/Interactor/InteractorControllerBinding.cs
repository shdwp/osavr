using UnityEngine;

namespace OsaVR.CockpitFramework.Interactor
{
    public class InteractorControllerBinding : MonoBehaviour
    {
        public IInteractorController Controller;

        public static void Bind(GameObject o, IInteractorController ctrl)
        {
            var binding = o.AddComponent<InteractorControllerBinding>();
            binding.Controller = ctrl;
        }
    }
}