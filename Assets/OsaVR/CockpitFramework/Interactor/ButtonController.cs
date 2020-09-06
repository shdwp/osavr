namespace OsaVR.CockpitFramework.Interactor
{
    public class ButtonController: AInteractorController
    {
        public override InteractionType[] InteractionTypes()
        {
            return new InteractionType[] {InteractionType.Press, InteractionType.Release};
        }

        protected virtual void OnPressed()
        {
            
        }

        protected virtual void OnReleased()
        {
            
        }

        public override void Handle(InteractionType type)
        {
            switch (type)
            {
                case InteractionType.Press:
                    OnPressed();
                    _animator.Play("Press");
                    break;
            }
        }
    }
}