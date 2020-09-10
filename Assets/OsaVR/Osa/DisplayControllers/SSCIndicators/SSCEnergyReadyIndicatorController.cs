using OsaVR.Osa.Model;
using UnityEngine;

namespace OsaVR.Osa.DisplayControllers.SSCIndicators
{
    public class SSCEnergyReadyIndicatorController: OsaLightIndicatorController
    {
        private bool _update = true;
        
        private new void Awake()
        {
            color = Color.green;
            base.Awake();
        }
        
        private void Start()
        {
            _sim.ListenNotification(SSCState.ChangedEnergyStateNotification, (_) => _update = true);
        }

        private void Update()
        {
            if (_update)
            {
                Set(0, 0, !_state.SSC.emitting);
                ApplyUpdates();
                
                _update = false;
            }
        }
    }
}