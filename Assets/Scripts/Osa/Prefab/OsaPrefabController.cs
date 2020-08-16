using System;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using Interactor;
using Osa;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Quaternion = UnityEngine.Quaternion;
using Vector3 = UnityEngine.Vector3;

namespace Osa
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
                    Destroy(processing.GetComponent<MeshRenderer>());
                    Destroy(processing.GetComponent<MeshFilter>());
                    item = Instantiate(Resources.Load<GameObject>("Controls/scope"), current.transform);
                    item.AddComponent<ScopeController>();
                    
                    item.transform.localRotation = Quaternion.identity;
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

            var item = Instantiate(Resources.Load<GameObject>("Controls/" + interactor_prefab_name), parent.transform);
            var animator = item.AddComponent<Animator>();
            animator.runtimeAnimatorController = Resources.Load<RuntimeAnimatorController>("Controls/" + interactor_prefab_name + "_animctrl");
            
            item.transform.localRotation = Quaternion.Euler(90f, 0f, 0f);

            var interaction_controller = AInteractorController.AddControllerForInteractorId(item, interactor_identifier);
            InteractorControllerBinding.Bind(placeholder, interaction_controller);
            return item;
        }

        void Update()
        {
        
        }
    }
}
