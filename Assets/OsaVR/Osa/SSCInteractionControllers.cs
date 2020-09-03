using OsaVR.CockpitFramework.Interactor;

namespace OsaVR.Osa
{
    public class SSCAzimuthPowertraverseController: SwitchController
    {
        public SSCAzimuthPowertraverseController()
        {
            supportedPositions = Position.Left | Position.Right | Position.Neutral;
            neutralPosition = Position.Neutral;
            currentPosition = Position.Neutral;
            springloaded = true;
        }
    }
}