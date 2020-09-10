using OsaVR.Osa.Model;

namespace OsaVR.Osa.Interactor.SUA
{
    public class SUATrackingModeDisableButtonController: OsaButtonController
    {
        private SUAState.TrackingFlags _targetFlags;

        public void SetTargetFlags(SUAState.TrackingFlags flags)
        {
            _targetFlags = flags;
        }
        
        protected override void OnPressed()
        {
            _state.Sua.trackingFlags &= ~_targetFlags;
            _state.Sua.autoAcquisition = false;
        }
    }
}