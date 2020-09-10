using System.Linq;
using OsaVR.CockpitFramework.Interactor;
using OsaVR.Osa;
using UnityEngine;

namespace OsaVR.InputControllers
{
    public class MouseController: MonoBehaviour
    {
        public Camera freeCamera;
        
        private IInteractorController _lastMouseInteractor;
        private OsaController _controller;

        private void Awake()
        {
            _controller = FindObjectOfType<OsaController>();
        }

        private void Update()
        {
            var primaryClick = Input.GetMouseButtonDown(0);
            var secondaryClick = Input.GetMouseButtonDown(1);

            if (primaryClick || secondaryClick)
            {
                _lastMouseInteractor = InteractorUnderMouse();
                if (_lastMouseInteractor != null)
                {
                    if (primaryClick)
                    {
                        SendFirstApplicableInteraction(_lastMouseInteractor, new InteractionType[] {InteractionType.Press, InteractionType.MoveDown, InteractionType.MoveLeft});
                    }

                    if (secondaryClick)
                    {
                        SendFirstApplicableInteraction(_lastMouseInteractor, new InteractionType[] {InteractionType.Press, InteractionType.MoveUp, InteractionType.MoveRight});
                    }
                }
            }

            if (_lastMouseInteractor != null && (Input.GetMouseButtonUp(0) || Input.GetMouseButtonUp(1)))
            {
                SendFirstApplicableInteraction(_lastMouseInteractor, new InteractionType[] {InteractionType.Release});
            
                _lastMouseInteractor = null;
            }

            if (Input.GetMouseButton(1) && _lastMouseInteractor == null && freeCamera.enabled)
            {
                var rot = freeCamera.transform.eulerAngles;
                var speed = 70f;

                rot.y += Input.GetAxis("Mouse X") * speed * Time.deltaTime;
                rot.x += -Input.GetAxis("Mouse Y") * speed * Time.deltaTime;

                freeCamera.transform.eulerAngles = rot;
            }
        }

        private IInteractorController InteractorUnderMouse()
        {
            RaycastHit hit;
            var ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out hit))
            {
                var interactor = _controller.InteractorFromCollider(hit.transform.gameObject);
                if (interactor)
                {
                    return interactor.GetComponent<IInteractorController>();
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
                    break;
                }
            }
        }
    }
}