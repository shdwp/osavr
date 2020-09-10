namespace OsaVR.Osa.Interactor.SOC
{
    public class SOCTraverseStateButtonController: OsaButtonController
    {
        private bool _targetState;

        public void SetTargetState(bool state)
        {
            _targetState = state;
        }
        
        protected override void OnPressed()
        {
            _state.SOC.turning = _targetState;
        }
    }
}