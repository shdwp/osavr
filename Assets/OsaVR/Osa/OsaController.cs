using System;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;
using JetBrains.Annotations;
using OsaVR.Osa.Model;
using OsaVR.Osa.ViewControllers;
using OsaVR.Osa.ViewControllers.SSCScope;
using OsaVR.Utils;
using OsaVR.World.Simulation;
using UnityEngine;
using Debug = UnityEngine.Debug;

namespace OsaVR.Osa
{
    public class OsaController: MonoBehaviour
    {
        private OsaState _state;
        private GameObject _SOCRoot;
        private SSCScopeController _scope;
        
        private void Start()
        {
            _scope = FindObjectOfType<SSCScopeController>();
            _state = FindObjectOfType<OsaState>();
            _SOCRoot = gameObject.FindChildNamed("SOC_Root");
            
            _state.worldPosition = transform.position;
            _state.worldForwardVector = transform.forward;
            
            /*
            _sim.Listen(OsaState.SOCTurnTick, _ =>
            {
                _SOCRoot.transform.eulerAngles = new Vector3(-90f, _state.SOCAzimuth, 0f);
            });
            */
        }
    }
}