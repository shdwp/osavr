using System;
using Osa.Model;
using UnityEngine;

namespace Osa
{
    public class OsaController: MonoBehaviour
    {
        private OsaState _state;
        
        private void Start()
        {
            _state = FindObjectOfType<OsaState>();
            
            _state.WorldPosition = transform.position;
            _state.ForwardVector = transform.forward;
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.T))
            {
                FindObjectOfType<MagneticField>().Test();
            }
        }
    }
}