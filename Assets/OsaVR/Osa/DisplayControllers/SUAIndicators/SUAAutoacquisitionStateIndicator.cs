using OsaVR.Osa.Model;

namespace OsaVR.Osa.DisplayControllers.SAUIndicators
{
    public class SUAAutoacquisitionStateIndicator: OsaLightIndicatorController
    {
        private bool _update = true;

        private void Start()
        {
            _sim.ListenNotification(SUAState.ChangedTrackingState, (_) => _update = true);
        }

        private void Update()
        {
            if (_update)
            {
                Set(0, 0, _state.Sua.autoAcquisition);
                ApplyUpdates();
                
                _update = false;
            }
        }
    }
}