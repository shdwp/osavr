namespace OsaVR.Osa.Interactor.SSC
{
    public class SSCEnergyStateButtonController: OsaButtonController
    {
        private bool _targetState;

        public void SetTargetState(bool state)
        {
            _targetState = state;
        }
        
        protected override void OnPressed()
        {
            _state.SSC.emitting = _targetState;
        }
    }
}