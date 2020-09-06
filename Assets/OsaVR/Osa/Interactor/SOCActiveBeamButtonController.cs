using OsaVR.CockpitFramework.Interactor;

namespace OsaVR.Osa.Interactor
{
    public class SOCActiveBeamButtonController: OsaButtonController
    {
        public uint targetBeam = 1;

        protected override void OnPressed()
        {
            _state.SOC.activeBeam = targetBeam;
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