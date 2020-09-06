using System;
using UnityEngine;
using OsaVR.Utils;

namespace OsaVR.CockpitFramework.Interactor
{
    public class WheelController: AInteractorController
    {
        private float _currentTurnSpeed = 0f;
        private GameObject _pivot;
        
        protected float _turnSpeed = 60f;
        protected float _clickAmount = 1f;

        protected new void Awake()
        {
            _pivot = gameObject.FindChildNamed("@Pivot");
            if (_pivot == null)
            {
                _pivot = gameObject;
            }
            
            base.Awake();
        }
        
        public override InteractionType[] InteractionTypes()
        {
            return new[] {InteractionType.Release, InteractionType.MoveLeft, InteractionType.MoveRight, InteractionType.ClickLeft, InteractionType.ClickRight};
        }

        protected virtual void OnTurn(float value)
        {
        }

        public override void Handle(InteractionType type)
        {
            switch (type)
            {
                case InteractionType.MoveLeft:
                    _currentTurnSpeed = -_turnSpeed;
                    break;
                case InteractionType.MoveRight:
                    _currentTurnSpeed = _turnSpeed;
                    break;
                case InteractionType.ClickLeft:
                    _currentTurnSpeed = 0f;
                    RotateWheel(-_clickAmount);
                    break;
                case InteractionType.ClickRight:
                    _currentTurnSpeed = 0f;
                    RotateWheel(_clickAmount);
                    break;
                case InteractionType.Release:
                    _currentTurnSpeed = 0f;
                    break;
            }
        }

        private void Update()
        {
            if (_currentTurnSpeed != 0f)
            {
                float correlatedSpeed = _currentTurnSpeed * Time.deltaTime;
                RotateWheel(correlatedSpeed);
            }
        }

        private void RotateWheel(float speed)
        {
            OnTurn(speed);
            var angles = _pivot.transform.localEulerAngles;
            angles.x = speed;
            
            _pivot.transform.localEulerAngles = angles;
        }
    }
}