using System;
using OsaVR.CockpitFramework.ViewControllers.Indicator;
using OsaVR.Osa.Model;
using OsaVR.World.Simulation;
using UnityEngine;

namespace OsaVR.Osa.DisplayControllers
{
    public class SOCActiveBeamIndicatorController: IndicatorController
    {
        private SimulationController _sim;
        private OsaState _state;
        private bool _update = true, _forceUpdate = true;
        private int _currentActiveBeam = 1;
        
        protected new void Awake()
        {
            dataWidth = 1;
            dataHeight = 3;

            _baseTex = Resources.Load<Texture2D>( "Controls/soc_active_beam_indicator/Indicator_Tex-assets/Indicator_Tex");
            _illuminatedTex = Resources.Load<Texture2D>( "Controls/soc_active_beam_indicator/Indicator_Tex-assets/Indicator_Tex_Illum");
            
            base.Awake();
        }

        private void Start()
        {
            _sim = FindObjectOfType<SimulationController>();
            _state = FindObjectOfType<OsaState>();

            _sim.ListenNotification(SOCState.ChangedActiveBeamNotification, (_) =>
            {
                _update = true;
            });
        }

        private void Update()
        {
            if (_update)
            {
                if (_currentActiveBeam != _state.SOC.activeBeam || _forceUpdate)
                {
                    var activeBeam = (int)_state.SOC.activeBeam;
                    dataTex.SetPixel(0, _currentActiveBeam - 1, Color.black);
                    dataTex.SetPixel(1, activeBeam - 1, Color.white);
                    dataTex.Apply();

                    _currentActiveBeam = activeBeam;
                }
                
                _update = false;
                _forceUpdate = false;
            }
        }
    }
}