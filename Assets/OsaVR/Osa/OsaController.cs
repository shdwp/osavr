using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;
using JetBrains.Annotations;
using OsaVR.CockpitFramework.Interactor;
using OsaVR.Osa.DisplayControllers;
using OsaVR.Osa.DisplayControllers.SAUIndicators;
using OsaVR.Osa.DisplayControllers.SOCIndicators;
using OsaVR.Osa.DisplayControllers.SSCElevationScope;
using OsaVR.Osa.DisplayControllers.SSCIndicators;
using OsaVR.Osa.DisplayControllers.SUAIndicators;
using OsaVR.Osa.Interactor;
using OsaVR.Osa.Interactor.SOC;
using OsaVR.Osa.Interactor.SSC;
using OsaVR.Osa.Interactor.SUA;
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

        private Dictionary<string, Camera> _utilityCameras = new Dictionary<string, Camera>();
        private Dictionary<GameObject, GameObject> _interactorColliders = new Dictionary<GameObject, GameObject>();
        private Dictionary<string, GameObject> _interactors = new Dictionary<string, GameObject>();
        
        private void Start()
        {
            _state = FindObjectOfType<OsaState>();
            
            _state.worldPosition = transform.position;
            _state.worldForwardVector = transform.forward;
        }

        public void BindView(string id, GameObject o)
        {
            switch (id)
            {
                case "ssc_scope":
                    o.AddComponent<SSCScopeController>();
                    break;
                
                case "ssc_elevation_scope":
                    o.AddComponent<SSCElevationScopeController>();
                    break;
                
                case "soc_scope":
                    o.AddComponent<SOCScopeController>();
                    break;
                
                case "soc_active_beam_indicator":
                    o.AddComponent<SOCActiveBeamIndicatorController>();
                    break;
                
                case "soc_energy_indicator":
                    o.AddComponent<SOCEnergyIndicatorController>();
                    break;
                
                case "soc_azimuth_dial":
                    o.AddComponent<SOCAzimuthDialController>();
                    break;
                
                case "ssc_energy_emitting_indicator":
                    o.AddComponent<SSCEnergyEmittingIndicatorController>();
                    break;
                    
                case "ssc_energy_ready_indicator":
                    o.AddComponent<SSCEnergyReadyIndicatorController>();
                    break;
                
                case "sua_disabled_indicator":
                    o.AddComponent<SUATrackingStateIndicator>().SetTargetFlags(false, SUAState.TrackingFlags.All);
                    break;
                
                case "sua_dist_semi_track_indicator":
                    o.AddComponent<SUATrackingStateIndicator>().SetTargetFlags(true, SUAState.TrackingFlags.DistanceSemiAutomatic);
                    break;
                
                case "sua_tv_track_indicator":
                    o.AddComponent<SUATrackingStateIndicator>().SetTargetFlags(true, SUAState.TrackingFlags.TV);
                    break;
                
                case "sua_tsc_track_indicator":
                    o.AddComponent<SUATrackingStateIndicator>().SetTargetFlags(true, SUAState.TrackingFlags.Azimuth);
                    break;
                
                case "sua_elevation_track_indicator":
                    o.AddComponent<SUATrackingStateIndicator>().SetTargetFlags(true, SUAState.TrackingFlags.Elevation);
                    break;
                
                case "sua_auto_acq_indicator":
                    o.AddComponent<SUAAutoacquisitionStateIndicator>();
                    break;
                
                case "sua_track_indicator":
                    o.AddComponent<SUATrackingStateIndicator>().SetTargetFlags(true, SUAState.TrackingFlags.FullyAutomatic);
                    break;
                
                case "ssc_elevation_dial":
                    o.AddComponent<SSCElevationDialController>();
                    break;
                
                case "ssc_distance_dial":
                    o.AddComponent<SSCDistanceDialController>();
                    break;
                
                default:
                    Debug.LogError($"ViewController not found for {id}");
                    break;
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
                case "soc_switch_active_beam_1":
                case "soc_switch_active_beam_2":
                case "soc_switch_active_beam_3":
                    interactorObject.AddComponent<SOCActiveBeamButtonController>().SetTargetBeamBasedOnId(id);
                    break;
                
                case "soc_active_beam_auto_1":
                    interactorObject.AddComponent<SOCAutoActiveBeamButtonController>().SetTargetMode(SOCState.ActiveBeamAutoMode.Beam1_3);
                    break;
                
                case "soc_active_beam_auto_2":
                    interactorObject.AddComponent<SOCAutoActiveBeamButtonController>().SetTargetMode(SOCState.ActiveBeamAutoMode.Beam1_2);
                    break;
                
                case "soc_energy_on":
                    interactorObject.AddComponent<SOCEnergyStateButtonController>().SetTargetState(true);
                    break;
                
                case "soc_energy_off":
                    interactorObject.AddComponent<SOCEnergyStateButtonController>().SetTargetState(false);
                    break;
                
                case "soc_traverse_on":
                    interactorObject.AddComponent<SOCTraverseStateButtonController>().SetTargetState(true);
                    break;
                
                case "soc_traverse_off":
                    interactorObject.AddComponent<SOCTraverseStateButtonController>().SetTargetState(false);
                    break;
                
                case "ssc_emission_on":
                    interactorObject.AddComponent<SSCEnergyStateButtonController>().SetTargetState(true);
                    break;
                
                case "ssc_emission_off":
                    interactorObject.AddComponent<SSCEnergyStateButtonController>().SetTargetState(false);
                    break;
                
                case "ssc_azimuth_powertraverse":
                    interactorObject.AddComponent<SSCAzimuthPowertraverseController>();
                    break;
                
                case "ssc_distance_wheel":
                    interactorObject.AddComponent<SSCDistanceWheelController>();
                    break;
                
                case "ssc_azimuth_wheel":
                    interactorObject.AddComponent<SSCAzimuthWheelController>();
                    break;
                
                case "ssc_elevation_wheel":
                    interactorObject.AddComponent<SSCElevationWheelController>();
                    break;
                
                case "soc_scope_range":
                    interactorObject.AddComponent<SOCScopeDisplayRangeSwitchController>();
                    break;
                
                case "sua_auto_acq_1":
                case "sua_auto_acq_2":
                    interactorObject.AddComponent<SUAAutoacquireButtonController>();
                    break;
                
                case "sua_tsc_1":
                case "sua_tsc_2":
                    interactorObject.AddComponent<SUATrackingModeEnableButtonController>().SetTargetFlags(SUAState.TrackingFlags.Azimuth);
                    break;
                
                case "sua_dist_semi_track":
                    interactorObject.AddComponent<SUATrackingModeEnableButtonController>().SetTargetFlags(SUAState.TrackingFlags.DistanceSemiAutomatic);
                    break;
                
                case "sua_track_disable_1":
                case "sua_track_disable_2":
                    interactorObject.AddComponent<SUATrackingModeDisableButtonController>().SetTargetFlags(SUAState.TrackingFlags.All);
                    break;
                
                case "sua_tv_track":
                    // @TODO
                    break;
                
                case "sua_elevation_track":
                    interactorObject.AddComponent<SUATrackingModeEnableButtonController>().SetTargetFlags(SUAState.TrackingFlags.Elevation);
                    break;
                
                case "sua_auto_acq":
                    interactorObject.AddComponent<SUAAutoacquireButtonController>();
                    break;
                
                default:
                    Debug.LogError($"InteractorController not found for {id}");
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