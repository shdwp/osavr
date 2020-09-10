namespace OsaVR.Osa.Interactor.SOC
{
    public class SOCEnergyStateButtonController: OsaButtonController
    {
        private bool _targetState;

        public void SetTargetState(bool state)
        {
            _targetState = state;
        }
        
        protected override void OnPressed()
        {
            _state.SOC.emitting = _targetState;
        }
    }
}