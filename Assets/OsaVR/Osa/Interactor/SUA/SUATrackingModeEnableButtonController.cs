using OsaVR.Osa.Model;

namespace OsaVR.Osa.Interactor.SUA
{
    public class SUATrackingModeEnableButtonController: OsaButtonController
    {
        public SUAState.TrackingFlags TargetFlags;

        public void SetTargetFlags(SUAState.TrackingFlags flags)
        {
            TargetFlags = flags;
        }

        protected override void OnPressed()
        {
            _state.Sua.trackingFlags |= TargetFlags;
        }
    }
}