using Interactor;
using UnityEngine;

namespace Osa
{
    public class ButtonController: AInteractorController
    {
        public override InteractionType[] InteractionTypes()
        {
            return new InteractionType[] {InteractionType.Press};
        }

        public override void Handle(InteractionType type)
        {
            switch (type)
            {
                case InteractionType.Press:
                    _animator.Play("Press");
                    break;
            }
        }
    }
}