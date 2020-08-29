using UnityEngine;

namespace OsaVR.CockpitFramework.Interactor
{
    public class InteractorControllerBinding : MonoBehaviour
    {
        public IInteractorController controller;

        public static void Bind(GameObject o, IInteractorController ctrl)
        {
            var binding = o.AddComponent<InteractorControllerBinding>();
            binding.controller = ctrl;
        }
    }
}