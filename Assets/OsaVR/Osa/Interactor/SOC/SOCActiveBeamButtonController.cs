using OsaVR.Osa.Model;

namespace OsaVR.Osa.Interactor.SOC
{
    public class SOCActiveBeamButtonController: OsaButtonController
    {
        public uint targetBeam = 1;

        protected override void OnPressed()
        {
            _state.SOC.activeBeam = targetBeam;
            _state.SOC.activeBeamAutoMode = SOCState.ActiveBeamAutoMode.Disabled;
        }

        public void SetTargetBeamBasedOnId(string id)
        {
            if (id.EndsWith("1"))
            {
                targetBeam = 1;
            } 
            else if (id.EndsWith("2"))
            {
                targetBeam = 2;
            } 
            else if (id.EndsWith("3"))
            {
                targetBeam = 3;
            }
        }
    }
}