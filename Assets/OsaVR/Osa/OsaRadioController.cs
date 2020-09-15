using System;
using OsaVR.Osa.Model;
using OsaVR.World;
using UnityEngine;

namespace OsaVR.Osa
{
    public class OsaRadioController: MonoBehaviour
    {
        private RadioBroadcastController _radio;
        private OsaState _state;
        
        private void Awake()
        {
            _state = FindObjectOfType<OsaState>();
            _radio = FindObjectOfType<RadioBroadcastController>();
        }

        private void Update()
        {
            _radio.iffRequest = _state.SOC.iffMode == SOCState.IFFMode.Request;
        }
    }
}