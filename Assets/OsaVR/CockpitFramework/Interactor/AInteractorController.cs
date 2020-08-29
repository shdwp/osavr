using System;
using UnityEngine;

namespace OsaVR.CockpitFramework.Interactor
{
    public abstract class AInteractorController: MonoBehaviour, IInteractorController
    {
        protected Animator _animator;
        
        protected void Awake()
        {
            _animator = GetComponent<Animator>();
        }
        
        public abstract InteractionType[] InteractionTypes();

        public abstract void Handle(InteractionType type);
    }
}