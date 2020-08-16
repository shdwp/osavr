using Interactor;
using UnityEngine;

namespace Osa
{
    public class SwitchController: AInteractorController
    {
        public override InteractionType[] InteractionTypes()
        {
            return new InteractionType[]
            {
                InteractionType.MoveUp,
                InteractionType.MoveDown
            };
        }

        public override void Handle(InteractionType type)
        {
            switch (type)
            {
                case InteractionType.MoveDown:
                    _animator.Play("Down");
                    break;
                case InteractionType.MoveUp:
                    _animator.Play("Up");
                    break;
            }
        }
    }
}