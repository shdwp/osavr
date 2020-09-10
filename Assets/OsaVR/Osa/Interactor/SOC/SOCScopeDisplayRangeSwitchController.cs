using OsaVR.Osa.Model;

namespace OsaVR.Osa.Interactor.SOC
{
    public class SOCScopeDisplayRangeSwitchController: OsaSwitchController
    {
        public SOCScopeDisplayRangeSwitchController()
        {
            supportedPositions = Position.Neutral | Position.Up | Position.Down;
            neutralPosition = Position.Neutral;
            currentPosition = Position.Neutral;
        }

        protected override void OnMovedInto(Position pos)
        {
            switch (pos)
            {
                case Position.Down:
                    _state.SOC.ScopeScopeDisplayRange = SOCState.ScopeDisplayRange.Ten_FortyFive;
                    break;
                
                case Position.Neutral:
                    _state.SOC.ScopeScopeDisplayRange = SOCState.ScopeDisplayRange.Zero_ThirtyFive;
                    break;
                
                case Position.Up:
                    _state.SOC.ScopeScopeDisplayRange = SOCState.ScopeDisplayRange.Zero_Fifteen;
                    break;
            }
        }
    }
}