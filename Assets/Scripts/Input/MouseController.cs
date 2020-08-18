using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Interactor;
using TMPro;
using UnityEditor.UI;
using UnityEngine;
using UnityEngine.UI;

public class MouseController: MonoBehaviour
{
    private IInteractorController _lastMouseInteractor = null;
    
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
                    SendFirstApplicableInteraction(_lastMouseInteractor, new InteractionType[] {InteractionType.Press, InteractionType.MoveDown});
                }

                if (secondaryClick)
                {
                    SendFirstApplicableInteraction(_lastMouseInteractor, new InteractionType[] {InteractionType.Press, InteractionType.MoveUp});
                }
            }
        }

        if (_lastMouseInteractor != null && (Input.GetMouseButtonUp(0) || Input.GetMouseButtonUp(1)))
        {
            SendFirstApplicableInteraction(_lastMouseInteractor, new InteractionType[] {InteractionType.Release});
            
            _lastMouseInteractor = null;
        }
    }

    private IInteractorController InteractorUnderMouse()
    {
        RaycastHit hit;
        var camera = Camera.main;
        var ray = camera.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out hit))
        {
            var interactor = hit.transform.gameObject.GetComponent<InteractorControllerBinding>();
            return interactor.Controller;
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