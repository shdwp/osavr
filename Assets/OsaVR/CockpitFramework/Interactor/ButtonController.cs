namespace OsaVR.CockpitFramework.Interactor
{
    public class ButtonController: AInteractorController
    {
        public override InteractionType[] InteractionTypes()
        {
            return new InteractionType[] {InteractionType.Press, InteractionType.Release};
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