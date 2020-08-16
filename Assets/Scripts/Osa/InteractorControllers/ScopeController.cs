using System;
using Interactor;
using TMPro;
using UnityEngine;

namespace Osa
{
    public class ScopeController: AInteractorController
    {
        protected new void Awake()
        {
            base.Awake();
            
            var display = gameObject.FindChildNamed("@Shader_ScopeDisplay");
            var renderer = display.GetComponent<MeshRenderer>();

            var tex = new Texture2D(2, 2, TextureFormat.ARGB32, false);
            tex.SetPixel(0, 0, Color.blue);
            tex.SetPixel(1, 1, Color.black);
            tex.Apply();

            renderer.material.SetTexture("_BaseColorMap", tex);
        }

        public override InteractionType[] InteractionTypes()
        {
            return new InteractionType[] {};
        }

        public override void Handle(InteractionType type)
        {
            throw new NotImplementedException();
        }
    }
}