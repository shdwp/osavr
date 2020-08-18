using System;
using Interactor;
using Osa.ViewControllers;
using TMPro;
using UnityEngine;

namespace Osa
{
    public class SignalScopeController: AViewController
    {
        protected new void Awake()
        {
            base.Awake();
            
            for (int x = 0; x < 10; x++)
            {
                for (int y = 0; y < 10; y++)
                {
                    _dataTex.SetPixel(x, y, y == 5 ? Color.white : Color.black);
                }
            }
            
            _dataTex.Apply();
            _viewSurfaceMat.SetTexture("Texture2D_1F77D971", Resources.Load<Texture2D>("scope_input"));
        }

        protected void FixedUpdate()
        {
        }
    }
}