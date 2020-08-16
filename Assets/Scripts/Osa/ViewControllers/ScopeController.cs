using System;
using Interactor;
using Osa.ViewControllers;
using TMPro;
using UnityEngine;

namespace Osa
{
    public class ScopeController: AViewController
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
        }
    }
}