using OsaVR.Osa.Model;

namespace OsaVR.Osa.Interactor.SOC
{
    public class SOCIFFButtonController: OsaButtonController
    {
        private SOCState.IFFMode _targetMode;

        public void SetTargetMode(SOCState.IFFMode mode)
        {
            _targetMode = mode;
        }
        
        protected override void OnPressed()
        {
            _state.SOC.iffMode = _targetMode;
        }
    }
}