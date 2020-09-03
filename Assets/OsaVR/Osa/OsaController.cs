using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;
using JetBrains.Annotations;
using OsaVR.CockpitFramework.Interactor;
using OsaVR.Osa.Model;
using OsaVR.Osa.ViewControllers;
using OsaVR.Osa.ViewControllers.SOCScope;
using OsaVR.Osa.ViewControllers.SSCScope;
using OsaVR.Utils;
using OsaVR.World.Simulation;
using TMPro;
using UnityEngine;
using Debug = UnityEngine.Debug;

namespace OsaVR.Osa
{
    public class OsaController: MonoBehaviour
    {
        private OsaState _state;
        private GameObject _SOCRoot;
        private SSCScopeController _scope;

        private Dictionary<string, Camera> _utilityCameras = new Dictionary<string, Camera>();
        private Dictionary<GameObject, GameObject> _interactorColliders = new Dictionary<GameObject, GameObject>();
        private Dictionary<string, GameObject> _interactors = new Dictionary<string, GameObject>();
        
        private void Start()
        {
            _scope = FindObjectOfType<SSCScopeController>();
            _state = FindObjectOfType<OsaState>();
            _SOCRoot = gameObject.FindChildNamed("SOC_Root");
            
            _state.worldPosition = transform.position;
            _state.worldForwardVector = transform.forward;
        }

        public void BindView(string id, GameObject o)
        {
            switch (id)
            {
                case "signal_scope":
                    o.AddComponent<SSCScopeController>();
                    break;
                
                case "radar_scope":
                    o.AddComponent<SOCScopeController>();
                    break;
                
                default: 
                    throw new NotImplementedException();
            }
        }

        public void BindUtilityCamera(string id, GameObject o)
        {
            _utilityCameras[id] = o.GetComponent<Camera>();
        }

        public void BindInteractor(string id, GameObject interactorObject, GameObject colliderObject)
        {
            _interactorColliders[colliderObject] = interactorObject;
            _interactors[id] = interactorObject;
            
            switch (id)
            {
                case "emissions_off":
                case "emissions_on":
                    interactorObject.AddComponent<ButtonController>();
                    break;
                
                case "ssc_azimuth_powertraverse":
                    var sw = interactorObject.AddComponent<SSCAzimuthPowertraverseController>();
                    break;
                
                default:
                    // throw new NotImplementedException();
                    break;
            }
        }

        public Camera SOCUtilityCamera(int idx)
        {
            switch (idx)
            {
                case 1:
                    return _utilityCameras["SOC_1Beam"];
                case 2:
                    return _utilityCameras["SOC_2Beam"];
                case 3:
                    return _utilityCameras["SOC_3Beam"];
                default:
                    throw new ArgumentException();
            }
        }

        public GameObject InteractorFromCollider(GameObject collider)
        {
            GameObject value = null;
            _interactorColliders.TryGetValue(collider, out value);
            return value;
        }
    }
}