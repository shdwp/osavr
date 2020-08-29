using System.Linq;
using OsaVR.CockpitFramework.Interactor;
using UnityEngine;

namespace OsaVR.InputControllers
{
    public class MouseController: MonoBehaviour
    {
        private IInteractorController _lastMouseInteractor = null;
        private Camera _camera;

        private void Start()
        {
            _camera = Camera.main;
        }

        private void Update()
        {
            var primaryClick = UnityEngine.Input.GetMouseButtonDown(0);
            var secondaryClick = UnityEngine.Input.GetMouseButtonDown(1);

            if (primaryClick || secondaryClick)
            {
                _lastMouseInteractor = InteractorUnderMouse();
                if (_lastMouseInteractor != null)
                {
                    if (primaryClick)
                    {
                        SendFirstApplicableInteraction(_lastMouseInteractor, new InteractionType[] {InteractionType.Press, InteractionType.MoveDown});
                    }

                    if (secondaryClick)
                    {
                        SendFirstApplicableInteraction(_lastMouseInteractor, new InteractionType[] {InteractionType.Press, InteractionType.MoveUp});
                    }
                }
            }

            if (UnityEngine.Input.GetMouseButton(1) && _lastMouseInteractor == null)
            {
                var rot = _camera.transform.eulerAngles;
                var speed = 70f;

                rot.y += UnityEngine.Input.GetAxis("Mouse X") * speed * Time.deltaTime;
                rot.x += -UnityEngine.Input.GetAxis("Mouse Y") * speed * Time.deltaTime;

                _camera.transform.eulerAngles = rot;
            }

            if (_lastMouseInteractor != null && (UnityEngine.Input.GetMouseButtonUp(0) || UnityEngine.Input.GetMouseButtonUp(1)))
            {
                SendFirstApplicableInteraction(_lastMouseInteractor, new InteractionType[] {InteractionType.Release});
            
                _lastMouseInteractor = null;
            }
        }

        private IInteractorController InteractorUnderMouse()
        {
            RaycastHit hit;
            var camera = Camera.main;
            var ray = camera.ScreenPointToRay(UnityEngine.Input.mousePosition);

            if (Physics.Raycast(ray, out hit))
            {
                var interactor = hit.transform.gameObject.GetComponent<InteractorControllerBinding>();
                if (interactor)
                {
                    return interactor.controller;
                }
            }

            return null;
        }

        private void SendFirstApplicableInteraction(IInteractorController ctrl, InteractionType[] types)
        {
            var supported_types = ctrl.InteractionTypes();
            
            foreach (var t in types)
            {
                if (supported_types.Contains(t))
                {
                    ctrl.Handle(t);
                }
            }
        }
    }
}