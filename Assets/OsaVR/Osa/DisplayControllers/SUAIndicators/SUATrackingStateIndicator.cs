using OsaVR.Osa.Model;

namespace OsaVR.Osa.DisplayControllers.SUAIndicators
{
    public class SUATrackingStateIndicator: OsaLightIndicatorController
    {
        public SUAState.TrackingFlags TargetFlags;
        public bool TrackFlagsIncluded;
        
        private bool _update = true;

        private void Start()
        {
            _sim.ListenNotification(SUAState.ChangedTrackingState, (_) => _update = true);
        }

        public void SetTargetFlags(bool shouldBeIncluded, SUAState.TrackingFlags flags)
        {
            TrackFlagsIncluded = shouldBeIncluded;
            TargetFlags = flags;
        }

        private void Update()
        {
            if (_update)
            {
                var included = _state.Sua.trackingFlags.HasFlag(TargetFlags); 
                Set(0, 0, TrackFlagsIncluded ? included : !included);
                ApplyUpdates();
                
                _update = false;
            }
        }
    }
}