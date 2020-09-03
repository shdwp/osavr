using System;
using System.IO;
using OsaVR.CockpitFramework.Interactor;
using OsaVR.CockpitFramework.ViewControllers;
using OsaVR.Osa;
using TMPro;
using UnityEngine;
using Quaternion = UnityEngine.Quaternion;
using Vector3 = UnityEngine.Vector3;

namespace OsaVR.Prefabs
{
    public class OsaPrefabController : MonoBehaviour
    {
        private OsaController _controller;
        
        private void Awake()
        {
            _controller = FindObjectOfType<OsaController>();
        }

        private void Start()
        {
            ProcessChildrenOf(this.gameObject);
        }

        private void ProcessChildrenOf(GameObject current)
        {
            foreach (Transform t in current.transform)
            {
                var processing = t.gameObject;
                var name = processing.name;
                var skip_prefix = "@Skip:";
                var intr_prefix = "@Intr:";
                var view_prefix = "@View:";
                var utility_cam_prefix = "@UtilityCamera:";

                GameObject item = null;
                if (name.StartsWith(intr_prefix))
                {
                    var description = name.Substring(intr_prefix.Length);
                    var components = description.Split(':');
                    item = ProcessInteractorPlaceholder(current, processing, components);
                } 
                else if (name.StartsWith(view_prefix))
                {
                    var description = name.Substring(intr_prefix.Length);
                    var components = description.Split(':');
                    item = ProcessViewPlaceholder(current, processing, components);
                }
                else if (name.StartsWith(utility_cam_prefix))
                {
                    var description = name.Substring(utility_cam_prefix.Length);
                    var components = description.Split(':');
                    ProcessUtilityCamera(current, processing, components);
                }
                else if (name.StartsWith(skip_prefix))
                {
                    Destroy(processing);
                    continue;
                }

                if (item != null)
                {
                    item.transform.localPosition = processing.transform.localPosition;
                }

                ProcessChildrenOf(processing);
            }
        }

        private GameObject ProcessInteractorPlaceholder(GameObject parent, GameObject placeholder, string[] components)
        {
            placeholder.AddComponent<SphereCollider>();
            Destroy(placeholder.GetComponent<MeshRenderer>());
            Destroy(placeholder.GetComponent<MeshFilter>());

            var interactorIdentifier = components[0];
            var interactorPrefabName = components[1];

            var path = Path.Combine("Controls", interactorPrefabName, interactorPrefabName);
            var item = Instantiate(Resources.Load<GameObject>(path), parent.transform);
            
            var animator = item.GetComponent<Animator>();
            if (animator != null)
            {
                animator.runtimeAnimatorController = Resources.Load<RuntimeAnimatorController>(path + "_animctrl");
            }

            item.transform.localRotation = Quaternion.Euler(90f, 0f, 0f);
            
            _controller.BindInteractor(interactorIdentifier, item, placeholder);
            return item;
        }

        private GameObject ProcessViewPlaceholder(GameObject parent, GameObject placeholder, string[] components)
        {
            var interactorIdentifier = components[0];
            var interactorPrefabName = components[1];
            
            Destroy(placeholder.GetComponent<MeshRenderer>());
            Destroy(placeholder.GetComponent<MeshFilter>());
            var path = Path.Combine("Controls", interactorPrefabName, interactorPrefabName);
            
            var item = Instantiate(Resources.Load<GameObject>(path), parent.transform);
            item.transform.localRotation = Quaternion.identity;

            _controller.BindView(interactorIdentifier, item);
            return item;
        }

        private void ProcessUtilityCamera(GameObject parent, GameObject placeholder, string[] components)
        {
            var identifier = components[0];
            _controller.BindUtilityCamera(identifier, placeholder);
        }
    }
}
