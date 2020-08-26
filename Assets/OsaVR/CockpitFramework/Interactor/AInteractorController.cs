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

        public static IInteractorController AddControllerForInteractorId(GameObject o, string id)
        {
            switch (id)
            {
                case "emissions_off":
                case "emissions_on":
                    return o.AddComponent<ButtonController>();
                
                case "test_1":
                case "test_2":
                    return o.AddComponent<SwitchController>();
            }

            throw new NotImplementedException();
        }
    }
}