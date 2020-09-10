using OsaVR.Osa.Model;
using UnityEngine;

namespace OsaVR.Osa.DisplayControllers.SOCIndicators
{
    public class SOCEnergyIndicatorController : OsaIndicatorController
    {
        private bool _update = true, _forceUpdate = true;

        protected new void Awake()
        {
            dataWidth = 1;
            dataHeight = 2;

            _baseTex = Resources.Load<Texture2D>("Controls/soc_energy_indicator/Indicator_Base");
            _illuminatedTex = Resources.Load<Texture2D>("Controls/soc_energy_indicator/Indicator_Illum");

            base.Awake();
        }

        private void Start()
        {
            _sim.ListenNotification(SOCState.ChangedEnergyStateNotification, (_) => { _update = true; });
        }

        private void Update()
        {
            if (_update)
            {
                
                if (_state.SOC.emitting)
                {
                    Set(0, 0, true);
                    Set(0, 1, false);
                }
                else
                {
                    Set(0, 0, false);
                    Set(0, 1, true);
                }
                
                ApplyUpdates();
            }
        }
    }
}