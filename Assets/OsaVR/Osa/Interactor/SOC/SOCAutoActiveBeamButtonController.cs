using OsaVR.Osa.Model;

namespace OsaVR.Osa.Interactor.SOC
{
    public class SOCAutoActiveBeamButtonController: OsaButtonController
    {
        private SOCState.ActiveBeamAutoMode _targetMode;

        public void SetTargetMode(SOCState.ActiveBeamAutoMode mode)
        {
            _targetMode = mode;
        }

        protected override void OnPressed()
        {
            _state.SOC.activeBeamAutoMode = _targetMode;
        }
    }
}