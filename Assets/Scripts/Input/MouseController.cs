using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Interactor;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MouseController: MonoBehaviour
{
    private void Update()
    {
        var primaryClick = Input.GetMouseButtonDown(0);
        var secondaryClick = Input.GetMouseButtonDown(1);

        if (primaryClick)
        {
            SendFirstApplicableInteraction(new InteractionType[] {InteractionType.Press, InteractionType.MoveDown});
        }

        if (secondaryClick)
        {
            SendFirstApplicableInteraction(new InteractionType[] {InteractionType.Press, InteractionType.MoveUp});
        }
    }

    private bool SendFirstApplicableInteraction(InteractionType[] types)
    {
        RaycastHit hit;
        var camera = Camera.main;
        var ray = camera.ScreenPointToRay(Input.mousePosition);
            
        if (Physics.Raycast(ray, out hit))
        {
            var interactor = hit.transform.gameObject.GetComponent<InteractorControllerBinding>();
            var supported_types = interactor.Controller.InteractionTypes();
            
            foreach (var t in types)
            {
                if (supported_types.Contains(t))
                {
                    interactor.Controller.Handle(t);
                    return true;
                }
            }
        }

        return false;
    }
}
