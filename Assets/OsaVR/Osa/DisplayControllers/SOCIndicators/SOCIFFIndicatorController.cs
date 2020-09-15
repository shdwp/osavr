using System;
using OsaVR.Osa.Model;
using UnityEngine;

namespace OsaVR.Osa.DisplayControllers.SOCIndicators
{
    public class SOCIFFIndicatorController: OsaLightIndicatorController
    {
        private bool _update = true;
        private SOCState.IFFMode _targetMode;
        
        protected new void Awake()
        {
            color = Color.red;
            base.Awake();
        }

        private void Start()
        {
            _sim.ListenNotification(SOCState.ChangedIFFState, (_) =>
            {
                _update = true;
            });
        }

        private void Update()
        {
            if (_update)
            {
                Set(0, 0, _state.SOC.iffMode == _targetMode);
                ApplyUpdates();
                _update = false;
            }
        }

        public void SetTargetMode(SOCState.IFFMode mode)
        {
            _targetMode = mode;
        }
    }
}