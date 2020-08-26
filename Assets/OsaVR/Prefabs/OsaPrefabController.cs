using System;
using System.IO;
using OsaVR.CockpitFramework.Interactor;
using OsaVR.CockpitFramework.ViewControllers;
using TMPro;
using UnityEngine;
using Quaternion = UnityEngine.Quaternion;
using Vector3 = UnityEngine.Vector3;

namespace OsaVR.Prefabs
{
    public class OsaPrefabController : MonoBehaviour
    {
        void Start()
        {
            processChildrenOf(this.gameObject);
        }

        void processChildrenOf(GameObject current)
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
                    item = _processInteractorPlaceholder(current, processing, components);
                } 
                else if (name.StartsWith(view_prefix))
                {
                    var description = name.Substring(intr_prefix.Length);
                    var components = description.Split(':');
                    item = _processViewPlaceholder(current, processing, components);
                }
                else if (name.StartsWith(utility_cam_prefix))
                {
                    var description = name.Substring(utility_cam_prefix.Length);
                    var components = description.Split(':');
                    _processUtilityCamera(current, processing, components);
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

                processChildrenOf(processing);
            }
        }

        GameObject _processInteractorPlaceholder(GameObject parent, GameObject placeholder, string[] components)
        {
            placeholder.AddComponent<SphereCollider>();
            Destroy(placeholder.GetComponent<MeshRenderer>());
            Destroy(placeholder.GetComponent<MeshFilter>());


            var textObject = new GameObject();
            textObject.transform.SetParent(parent.transform);
            var titleText = textObject.AddComponent<TextMeshPro>();
            titleText.fontSize = 24;
            titleText.text = "Title";
            titleText.alignment = TextAlignmentOptions.Center;
            titleText.transform.localPosition = placeholder.transform.localPosition + placeholder.transform.up * -0.0002f;
            titleText.transform.localScale = new Vector3(0.0001f, 0.0001f, 0.0001f);
            titleText.transform.localRotation = Quaternion.Euler(180f, 90f, 90f);

            var interactor_identifier = components[0];
            var interactor_prefab_name = components[1];
            Debug.Log(String.Format("Interactor from {0} - control {1}", placeholder.name, interactor_prefab_name));

            var path = Path.Combine("Controls", interactor_prefab_name, interactor_prefab_name);
            var item = Instantiate(Resources.Load<GameObject>(path), parent.transform);
            var animator = item.GetComponent<Animator>();
            animator.runtimeAnimatorController = Resources.Load<RuntimeAnimatorController>(path + "_animctrl");
            
            item.transform.localRotation = Quaternion.Euler(90f, 0f, 0f);

            var interaction_controller = AInteractorController.AddControllerForInteractorId(item, interactor_identifier);
            InteractorControllerBinding.Bind(placeholder, interaction_controller);
            return item;
        }

        GameObject _processViewPlaceholder(GameObject parent, GameObject placeholder, string[] components)
        {
            var interactor_identifier = components[0];
            var interactor_prefab_name = components[1];
            
            Destroy(placeholder.GetComponent<MeshRenderer>());
            Destroy(placeholder.GetComponent<MeshFilter>());
            var path = Path.Combine("Controls", interactor_prefab_name, interactor_prefab_name);
            var item = Instantiate(Resources.Load<GameObject>(path), parent.transform);

            var controller = AViewController.AddControllerForViewId(item, interactor_identifier);
            item.transform.localRotation = Quaternion.identity;
            return item;
        }

        void _processUtilityCamera(GameObject parent, GameObject placeholder, string[] components)
        {
            var identifier = components[0];
            var camera = placeholder.GetComponent<Camera>();

            camera.depthTextureMode = DepthTextureMode.Depth;
            //camera.enabled = identifier == "SOC_3Beam";
        }

        void Update()
        {
        
        }
    }
}
