using System;
using OsaVR.CockpitFramework.Interactor;
using OsaVR.Osa.Interactor;
using OsaVR.Osa.Model;

namespace OsaVR.Osa
{
    public class SSCAzimuthPowertraverseController: OsaSwitchController
    {
        public SSCAzimuthPowertraverseController()
        {
            supportedPositions = Position.Left | Position.Right | Position.Neutral;
            neutralPosition = Position.Neutral;
            currentPosition = Position.Neutral;
            springloaded = true;
        }

        protected override void OnMovedInto(Position pos)
        {
            switch (pos)
            {
                case Position.Left:
                    _state.SSC.powertraverseState = SSCState.PowertraverseState.Left;
                    break;
                
                case Position.Right:
                    _state.SSC.powertraverseState = SSCState.PowertraverseState.Right;
                    break;
                
                case Position.Neutral:
                    _state.SSC.powertraverseState = SSCState.PowertraverseState.Idle;
                    break;
                
                default:
                    throw new NotImplementedException();
            }
        }
    }

    public class SSCDistanceWheelController : OsaWheelController
    {
        protected override void OnTurn(float value)
        {
            _state.SSC.distance += value / 10f;
        }
    }

    public class SSCAzimuthWheelController : OsaWheelController
    {
        protected override void OnTurn(float value)
        {
            _state.SSC.azimuth += value;
        }
    }
}